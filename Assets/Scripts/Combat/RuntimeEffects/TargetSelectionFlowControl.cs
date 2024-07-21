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

        #region Event Broadcaster 
        private EventBroadcaster _eventBroadcaster;

        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();

                _eventBroadcaster.OpenChannel<TargetSelectingData>("OnTargetSelectionEnd");
                _eventBroadcaster.OpenChannel<TargetSelectingData>("OnTargetSelectionEnter");
            }

            return _eventBroadcaster;
        }

        public void SubOnTargetSelectionEnd(UnityAction<TargetSelectingData> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnTargetSelectionEnd");
        }
        public void UnSubOnTargetSelectionEnd(UnityAction<TargetSelectingData> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnTargetSelectionEnd");
        }

        public void SubOnTargetSelectionEnter(UnityAction<TargetSelectingData> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnTargetSelectionEnter");
        }
        public void UnSubOnTargetSelectionEnter(UnityAction<TargetSelectingData> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnTargetSelectionEnter");
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
                }else
                {
                    _targetSelectionGUI.SelectWeakTarget(target);   
                }
            }
        }

        #region Public Method
        public void ReceiveKeys(InputCode key)
        {
            if (_activlySelecting)
            {
                if (key == (InputCode.Right))
                {
                    _currentSelectIndex = (_currentSelectIndex + 1) > (_validTargets.Count - 1) ? _currentSelectIndex : (_currentSelectIndex + 1);
                    _targetSelectionGUI.HideAllPointer( );

                }
                else if (key == (InputCode.Left))
                {
                    _currentSelectIndex = (_currentSelectIndex - 1) < 0 ? 0 : (_currentSelectIndex - 1);
                    _targetSelectionGUI.HideAllPointer(); 
                }
                else if (key == (InputCode.Select))
                {
                    foreach (var entity in _selectingTarget)
                    {
                        _selectedTarget.Add(entity);
                        _validTargets.Remove(entity) ;
                    }

                    if (_validTargets.Count != 0)
                        _currentSelectIndex = _currentSelectIndex % _validTargets.Count;

                }
                //else if (key == (InputCode.Skill))
                //{
                //    ForceStop();
                //    _enemyHUDWindowManager.RemoveSlotBreakHighlightOnHUD();
                //}
                else if (key == (InputCode.DeSelect))
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
                _activlySelecting = false   ;
                _forceStop = true   ;
            }
        }
        
        public IEnumerator InitializeActionTargetSelectionScheme(CombatEntity caster, ActorAction actorAction, bool randomTarget = false)
        {
            //ColorfulLogger.LogWithColor("Start target selection with " + actorAction , Color.green);

            if (_activlySelecting)
                throw new Exception(caster + " Try to active selection scheme while it is already active");

            ValidateData();

            _activlySelecting = true;

            TargetSelectingData targetSelectingData = new TargetSelectingData() { caster = caster, 
                targetSelector = actorAction.GetTargetSelector() ,
                isSucesfullySelected = false 
            };

            CentralInputReceiver.Instance.AddInputReceiverIntoStack(this); 

            AssignPossibleTargets(caster, actorAction.GetTargetSelector());

            PrepareCameraForTargetSelection(randomTarget, actorAction.GetTargetSelector().TargetAllyTeam, caster);


            _validTargets = ArrangeEntityListInXAxis(_validTargets, Vector3.zero);

            _eventBroadcaster.InvokeEvent(targetSelectingData, "OnTargetSelectionEnter");

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

                    //we automaticall display enemy when enter control
                    _enemyHUDWindowManager.DisplayEnemyHUD(_selectingTarget);

                    _characterWindowManager.DisplayArrowOnTargetCharacter(_selectingTarget);

                    foreach (CombatEntity selectedEntity in _selectingTarget)
                    {
                        yield return EnergySimulation(selectedEntity, actorAction);
                    }

                    //if (actorAction.GetTargetSelector().TargetAllyTeam)
                    //{

                    //    if (CombatReferee.Instance.GetCompetatorSide(caster) != ECompetatorSide.Ally)
                    //        continue;

                    //    if (_selectingTarget.Count != 1 || true)
                    //    {
                    //        var cam = CameraSetUPManager.Instance.SetVMTOAllAlly();
                    //        _validTargets = ArrangeEntityListInXAxis(_validTargets,cam.transform.right);
                    //        //_selectingTarget.Clear();
     
                    //        //_selectingTarget.Add(_validTargets[_currentSelectIndex]);  


                    //        continue;
                    //    }


                    //    //_selectingTarget[0].GetComponent<EntityCameraManager>().EnableShoulderCamera();
                    //    //throw new Exception("The problem is axis correction can't not be perform until we swap cam, meaning we can't set the first cam "); 
                    //    //We multuiply -1 because we want to change the right direction of vector 

                    //    //_validTargets = ArrangeEntityListInXAxis(_validTargets, _selectingTarget[0].GetComponent<EntityCameraManager>().GetRightVectorShoulderCam());
                    //    //_selectingTarget.Clear();
                    //    //_selectingTarget.Add(_validTargets[_currentSelectIndex]);
                    //    //if want to display shoulder camera separatly, do it here
                    //    //_selectingTarget[0].GetComponent<EntityCameraManager>().EnableShoulderCamera();


                    //}

                    //else
                    //{

                    //    //CameraSetUPManager.Instance.SetLookAtTarget(_selectingTarget[0].GetComponent<CombatEntityAnimationHandler>().GetGUISpawnTransform());
                    //}
                    _validTargets = ArrangeEntityListInXAxis(_validTargets, Vector3.zero);

                }

                yield return new WaitForEndOfFrame();

            }

            _enemyHUDWindowManager.DisableEnemyHUD();
            _targetSelectionGUI.EndSelectionScheme();
            _characterWindowManager.HideAllArrowTargetCharacter();

            if (_selectedTarget.Count > 0)
            {
                for (int k= 0; k < _selectedTarget.Count; k++ )
                {
                    EntityCameraManager entityCam = _selectedTarget[k].GetComponent<EntityCameraManager>(); 
                    entityCam.DisableAllAttachedCamera();
                }
                actorAction.SetActionTarget(_selectedTarget);
                
                caster.ActionHandler.AddActionQueue(actorAction);
                
            }

            if (!randomTarget)
            {
                CameraSetUPManager.Instance.RestoreVMCameraState();
            }

            //OnTargetSelectionSchemeEnd.PlayEvent(caster);
            //Broadcast Ending of target selection with Sucesfful status
           
            _eventBroadcaster.InvokeEvent(new TargetSelectingData()
            {
                caster = caster,
                targetSelector = actorAction.GetTargetSelector(),
                isSucesfullySelected = ! _forceStop
            }, "OnTargetSelectionEnd");


            CentralInputReceiver.Instance.RemoveInputReceiverIntoStack(this);

            _forceStop = false;
            _activlySelecting = false;


        }

        #endregion

        #region Camera / Arrange Entity Helper Function 

        private List<CombatEntity> ArrangeEntityListInXAxis(List<CombatEntity> entities, Vector3 rightVector)
        {
            if (rightVector != Vector3.zero)
            {
                if (rightVector.x > 0)
                    entities.Sort((entity1, entity2) => entity1.transform.position.x.CompareTo(entity2.transform.position.x));
                else
                    entities.Sort((entity1, entity2) => entity2.transform.position.x.CompareTo(entity1.transform.position.x));
            }
            else
            {
                if (Camera.main.transform.right.x > 0)
                    entities.Sort((entity1, entity2) => entity1.transform.position.x.CompareTo(entity2.transform.position.x));
                else
                    entities.Sort((entity1, entity2) => entity2.transform.position.x.CompareTo(entity1.transform.position.x));
            }
            return entities;
        }

        private void PrepareCameraForTargetSelection(bool randomTarget, bool targetAlly, CombatEntity caster)
        {
            //If random target, we don't have to do anything about the camera
            if (randomTarget)
                return;
                
    
            CameraSetUPManager.Instance.CaptureVMCamera();

            //If Target ally, and the caster is player's
            if (targetAlly)
            {

                if (CombatReferee.Instance.GetCompetatorSide(caster) != ECompetatorSide.Ally)
                    return ;

                if (_selectingTarget.Count != 1 || true)
                {
                    var cam = CameraSetUPManager.Instance.SetVMTOAllAlly();
                    _validTargets = ArrangeEntityListInXAxis(_validTargets, cam.transform.right);
                    return; ;
                }


                

            }
        }

        
        #endregion
    }


    public struct TargetSelectingData
    {
        public TargetSelector targetSelector;
        public CombatEntity caster;
        public bool isSucesfullySelected ; 
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