using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;


namespace Vanaring
{
    public class TargetSelectionFlowControl : MonoBehaviour, IInputReceiver   
    {
        private EventBroadcaster _eventBroadcaster;

        [SerializeField]
        private TargetSelectionGUI _targetSelectionGUI ;


        [SerializeField] 
        private EnemyHUDWindowManager _enemyHUDWindowManager ;

        [SerializeField]
        private Transform _targetSelectionDisplayer; 

        private List<CombatEntity> _validTargets = new List<CombatEntity>();
        private List<CombatEntity> _selectedTarget = new List<CombatEntity>();

        private List<CombatEntity> _selectingTarget = new List<CombatEntity>(); 

        public static TargetSelectionFlowControl Instance;
        CharacterWindowManager _characterWindowManager; 
        private bool _activlySelecting = false;

        private int _currentSelectIndex = 0;

        private bool _forceStop = false;

        #region GETTER
        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();

                _eventBroadcaster.OpenChannel<bool>("OnTargetSelectionEnd");
                _eventBroadcaster.OpenChannel<CombatEntity>("OnTargetSelectionEnter");
            }



            return _eventBroadcaster;
        }
        #endregion

        private void Awake()
        {
            Instance = this;
            if (_targetSelectionDisplayer == null)
                throw new Exception("_targetSelectionDisplayer need to be assigned ! ");

            _characterWindowManager = FindObjectOfType<CharacterWindowManager>(); 

            _targetSelectionGUI.Initialize(_targetSelectionDisplayer);
        }

        private void AssignPossibleTargets(CombatEntity caster, TargetSelector targetSelector)
        {
            _validTargets.Clear();

            ECompetatorSide eCompetatorSide = CombatReferee.Instance.GetCompetatorSide(caster);   

            foreach (ECompetatorSide side in Enum.GetValues(typeof(ECompetatorSide)))
            {
                foreach (CombatEntity target in CombatReferee.Instance.GetCompetatorsBySide(side))
                {
                    if (! targetSelector.CorrectTarget(caster, target))
                        continue;   
                    
                    if (target.IsDead)
                        continue; 

                    if (!_validTargets.Contains(target))
                    {
                        _validTargets.Add(target);
                    }
                }
            }

            //Arrange entity index according to the camera
            //_validTargets = ArrangeEntityListInXAxis(_validTargets); 

        }

        private List<CombatEntity> ArrangeEntityListInXAxis(List<CombatEntity> entities)
        {
            if (Camera.main.transform.right.x > 0)
                entities.Sort((entity1, entity2) => entity1.transform.position.x.CompareTo(entity2.transform.position.x));
            else
                entities.Sort((entity1, entity2) => entity2.transform.position.x.CompareTo(entity1.transform.position.x));
            return entities;
        }

        private void ValidateData()
        {
            _validTargets = new List<CombatEntity>();
            _selectedTarget = new List<CombatEntity>();
            _selectingTarget = new List<CombatEntity>(); 
            _currentSelectIndex = 0;
        }

        /// <summary>
        /// Check whether when player do this action, it will make the target overflow or not
        /// </summary>
        /// <param name="target"></param>
        /// <param name="action"></param>
        private IEnumerator EnergySimulation(CombatEntity target, ActorAction action)
        {
            if (!target.SpellCaster.IsEnergyOverflow())
            {
                //Debug.Log("simulate energy");
                yield return action.Simulate(target);

                if (target.SpellCaster.CheckSimulation())
                {
                    _targetSelectionGUI.SelectBreakTarget(target);
                }
            }
        }

        #region Public Method
        public void ReceiveKeys(KeyCode key)
        {
            if (_activlySelecting)
            {
                if (key == (KeyCode.D))
                {
                    _currentSelectIndex = (_currentSelectIndex + 1) > (_validTargets.Count - 1) ? _currentSelectIndex : (_currentSelectIndex + 1);
                    _targetSelectionGUI.HideAllPointer( );

                }
                else if (key == (KeyCode.A))
                {
                    _currentSelectIndex = (_currentSelectIndex - 1) < 0 ? 0 : (_currentSelectIndex - 1);
                    _targetSelectionGUI.HideAllPointer(); 
                }
                else if (key == (KeyCode.Space))
                {
                    foreach (var entity in _selectingTarget)
                    {
                        _selectedTarget.Add(entity);
                        _validTargets.Remove(entity) ;
                    }

                    if (_validTargets.Count != 0)
                        _currentSelectIndex = _currentSelectIndex % _validTargets.Count;

                }
                else if (key == (KeyCode.Q))
                {
                    ForceStop();
                    _enemyHUDWindowManager.RemoveSlotBreakHighlightOnHUD();
                }
            }
        }

        public void ForceStop()
        {
            if (_activlySelecting)
            {
                _activlySelecting = false;
                _forceStop = true;
            }
        }
        //public IEnumerator InitializeTargetSelectionSchemeWithoutSelect(List<CombatEntity> _targets)
        //{
        //    if (_activlySelecting)
        //        throw new Exception("Try to active selection scheme while it is already active");

        //    _activlySelecting = true;
        //    ValidateData();

        //    foreach (var v in _targets)
        //    {
        //        _validTargets.Add(v);
        //    }

        //    transform.DOMove(Vector3.zero, 3.0f);
        //    while (true)
        //    {
        //        if (_forceStop)
        //        {
        //            _forceStop = false;
        //            ColorfulLogger.LogWithColor("Cancel Target Selection", Color.green);
        //            goto End;
        //        }

        //        //_targetSelectionGUI.SelectTargetPointer(_validTargets[_currentSelectIndex]);

        //        yield return _validTargets[_currentSelectIndex];

        //    }

        //End:
        //    _targetSelectionGUI.EndSelectionScheme();
        //    _activlySelecting = false;
        //}
        public IEnumerator InitializeActionTargetSelectionScheme(CombatEntity caster, ActorAction actorAction, bool randomTarget = false)
        {
            //ColorfulLogger.LogWithColor("Start target selection with " + actorAction , Color.green);

            if (_activlySelecting)
                throw new Exception(caster + " Try to active selection scheme while it is already active");

            _activlySelecting = true;

            _eventBroadcaster.InvokeEvent<CombatEntity>(caster, "OnTargetSelectionEnter");

            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);

            ValidateData();

            AssignPossibleTargets(caster, actorAction.GetTargetSelector());

            if (!randomTarget)
                CameraSetUPManager.Instance.CaptureVMCamera();


            while (_selectedTarget.Count < actorAction.GetTargetSelector().MaxTarget )
            {
                if (_validTargets.Count < actorAction.GetTargetSelector().MaxTarget && _validTargets.Count == 0)
                    break;

                if (_forceStop)
                    break;

                _selectingTarget.Clear();

                _selectingTarget.Add(_validTargets[_currentSelectIndex]);

         
                if (_validTargets.Count <= actorAction.GetTargetSelector().MaxTarget)
                {
                    _selectingTarget.Clear();
                    for (int i = 0; i < _validTargets.Count; i++)
                        _selectingTarget.Add(_validTargets[i]);
                }

                if (randomTarget)
                {
                    _currentSelectIndex = UnityEngine.Random.Range(0, _validTargets.Count);
                    _selectedTarget.Add(_validTargets[_currentSelectIndex]);
                    _validTargets.RemoveAt(_currentSelectIndex);
                    _currentSelectIndex = 0;
                    continue;
                }
                else
                {
                    //need to display every ui first before doing anythign 
                    _targetSelectionGUI.SelectTargetPointer(_selectingTarget);

                    _enemyHUDWindowManager.DisplayEnemyHUD(_selectingTarget);

                    _characterWindowManager.DisplayArrowOnTargetCharacter(_selectingTarget);

                    foreach (CombatEntity selectedEntity in _selectingTarget)
                    {
                        yield return EnergySimulation(selectedEntity, actorAction);
                    }

                    if (actorAction.GetTargetSelector().TargetAllyTeam)
                    {
                        if (CombatReferee.Instance.GetCompetatorSide(caster) != ECompetatorSide.Ally)
                            continue;

                        if (_selectingTarget.Count != 1)
                            continue;
                        
                        _selectingTarget[0].GetComponent<EntityCameraManager>().EnableShoulderCamera();
                        //_validTargets = ArrangeEntityListInXAxis(_validTargets);
                    }
                    else
                    {
                        //CameraSetUPManager.Instance.SetLookAtTarget(_selectingTarget[0].GetComponent<CombatEntityAnimationHandler>().GetGUISpawnTransform());
                    }
                }

                yield return new WaitForEndOfFrame();

            }

            _enemyHUDWindowManager.DisableEnemyHUD();
            _targetSelectionGUI.EndSelectionScheme();
            _characterWindowManager.HideAllArrowTargetCharacter(); 

            if (_selectedTarget.Count > 0)
            { 
                actorAction.SetActionTarget(_selectedTarget);
                caster.AddActionQueue(actorAction);
            }

    

            //OnTargetSelectionSchemeEnd.PlayEvent(caster);
            //Broadcast Ending of target selection with Sucesfful status
            _eventBroadcaster.InvokeEvent<bool>(!_forceStop, "OnTargetSelectionEnd");

            if (!randomTarget)
                CameraSetUPManager.Instance.RestoreVMCameraState();

            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);

            _forceStop = false;
            _activlySelecting = false;


        }

        #endregion

        #region SubEvents Methods 

 
        public void SubOnTargetSelectionEnd(UnityAction<bool> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnTargetSelectionEnd");
        }
        public void UnSubOnTargetSelectionEnd (UnityAction<bool> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnTargetSelectionEnd");
        }

        public void SubOnTargetSelectionEnter(UnityAction<CombatEntity> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnTargetSelectionEnter");
        }
        public void UnSubOnTargetSelectionEnter(UnityAction<CombatEntity> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnTargetSelectionEnter");
        }

        #endregion
    }


    [Serializable]
    public class TargetSelector
    {
        [Header("Maximum target that can be assigned to")]
        [SerializeField]
        private int _maxTargetSize = 1;

        #region Getter 
        public int MaxTarget => _maxTargetSize;
        #endregion
        [Header("Toggle target status when selected")]
        [SerializeField]
        private bool _targetAlly;
        [SerializeField]
        private bool _targetOppose;
        [SerializeField]
        private bool _targetSelf;
        public bool CorrectTarget(CombatEntity caster, CombatEntity target)
        {
            ECompetatorSide casterSide = CombatReferee.Instance.GetCompetatorSide(caster);
            ECompetatorSide targetSide = CombatReferee.Instance.GetCompetatorSide(target);

            if (TargetCasterItself)
            {
                return caster == target;
            }

            if (TargetOppose)
            {
                return (casterSide != targetSide);
            }

            if (TargetAllyTeam)
            {
                return (casterSide == targetSide);
            }


            return false;
        }

        private bool TargetOppose => (_targetOppose);
        private bool TargetCasterItself => (_targetSelf);
        public bool TargetAllyTeam => (_targetAlly);

        //Used when the target selection is requires  
    }
}