using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

        private bool _activlySelecting = false;

        private int _currentSelectIndex = 0;

        private bool _forceStop = false;

        #region GETTER
        public EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
            }
            return _eventBroadcaster;
        }
        #endregion

        private void Awake()
        {
            Instance = this;
            _targetSelectionGUI.Initialize(_targetSelectionDisplayer);
            _eventBroadcaster = new EventBroadcaster();

            _eventBroadcaster.OpenChannel<bool>("OnTargetSelectionEnd")  ;
            _eventBroadcaster.OpenChannel<CombatEntity>("OnTargetSelectionEnter") ;

        }

        private void AssignPossibleTargets(CombatEntity caster, TargetSelector targetSelector)
        {
            _validTargets.Clear();

            ECompetatorSide eCompetatorSide = CombatReferee.instance.GetCompetatorSide(caster);  //CombatReferee.instance.GetCharacterSide(caster);
            ;  // CombatReferee.instance.GetCharacterSide(caster);

            foreach (ECompetatorSide side in Enum.GetValues(typeof(ECompetatorSide)))
            {
                foreach (CombatEntity target in CombatReferee.instance.GetCompetatorsBySide(side))
                {
                    if (targetSelector.CorrectTarget(caster, target))
                    {
                        if (!_validTargets.Contains(target))
                        {
                            _validTargets.Add(target);
                        }
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
            yield return action.Simulate(target);

            if (target.SpellCaster.CheckSimulation())
                _targetSelectionGUI.SelectBreakTarget(target); 
            
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
        public IEnumerator InitializeTargetSelectionSchemeWithoutSelect(List<CombatEntity> _targets)
        {
            if (_activlySelecting)
                throw new Exception("Try to active selection scheme while it is already active");

            _activlySelecting = true;
            ValidateData();

            foreach (var v in _targets)
            {
                _validTargets.Add(v);
            }

            transform.DOMove(Vector3.zero, 3.0f);
            while (true)
            {
                if (_forceStop)
                {
                    _forceStop = false;
                    ColorfulLogger.LogWithColor("Cancel Target Selection", Color.green);
                    goto End;
                }

                //_targetSelectionGUI.SelectTargetPointer(_validTargets[_currentSelectIndex]);

                yield return _validTargets[_currentSelectIndex];

            }

        End:
            _targetSelectionGUI.EndSelectionScheme();
            _activlySelecting = false;
        }
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

            CameraSetUPManager.Instance.CaptureVMCamera();

            CameraSetUPManager.Instance.SetBlendMode(CameraSetUPManager.CameraBlendMode.EASE_INOUT, 0.5f);

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
                 
                    continue;
                }
                if (! randomTarget)
                {
                    foreach (CombatEntity selectedEntity in _selectingTarget)
                    {
                        yield return EnergySimulation(selectedEntity, actorAction);
                    }
                    _targetSelectionGUI.SelectTargetPointer(_selectingTarget);
                    _enemyHUDWindowManager.DisplayEnemyHUD(_selectingTarget);

                    CameraSetUPManager.Instance.SetLookAtTarget(_selectingTarget[0].transform) ;

                }

                yield return new WaitForEndOfFrame();

            }

            if (_selectedTarget.Count > 0)
            { 
                actorAction.SetActionTarget(_selectedTarget);
                caster.AddActionQueue(actorAction);
            }

            _targetSelectionGUI.EndSelectionScheme();

            //OnTargetSelectionSchemeEnd.PlayEvent(caster);
            //Broadcast Ending of target selection with Sucesfful status
            _eventBroadcaster.InvokeEvent<bool>(!_forceStop, "OnTargetSelectionEnd");

            CameraSetUPManager.Instance.RestoreVMCameraState();
            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);

            _forceStop = false;
            _activlySelecting = false;


        }


        #endregion
    }


    [Serializable]
    public class TargetSelector
    {
        [Header("Maximum target that can be assigned to")]
        [SerializeField]
        private int _maxTargetSize = 1;

        public int MaxTarget => _maxTargetSize;

        [SerializeField]
        private bool _targetCaster = false;

        [Header("Toggle target status when selected")]
        [SerializeField]
        private bool _targetAlly;
        [SerializeField]
        private bool _targetOppose;
        [SerializeField]
        private bool _targetSelf;
        public bool CorrectTarget(CombatEntity caster, CombatEntity target)
        {
            ECompetatorSide casterSide = CombatReferee.instance.GetCompetatorSide(caster);
            ECompetatorSide targetSide = CombatReferee.instance.GetCompetatorSide(target);

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