﻿using DG.Tweening;
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
        [Header("Broadcast to ")]
        [SerializeField]
        private CombatEntityEventChannel OnTargetSelectionSchemeStart;

        [SerializeField]
        private CombatEntityEventChannel OnTargetSelectionSchemeEnd;

        [SerializeField]
        private TargetSelectionGUI _targetSelectionGUI;

        [Header("Indicator")]
        [SerializeField]
        ButtonIndicatorWindow _buttonIndicatorWindow;

        private List<CombatEntity> _validTargets = new List<CombatEntity>();
        private List<CombatEntity> _selectedTarget = new List<CombatEntity>();

        public static TargetSelectionFlowControl Instance;

        private bool _activlySelecting = false;

        private int _currentSelectIndex = 0;

        private bool _forceStop = false;


        private void Awake()
        {
            Instance = this;
            _targetSelectionGUI.Initialize(transform);
        }

        public void ForceStop()
        {
            ColorfulLogger.LogWithColor("ForceStop ", Color.red); 
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

                _targetSelectionGUI.SelectTargetPointer(_validTargets[_currentSelectIndex]);

                yield return null;

                yield return _validTargets[_currentSelectIndex];

            }

        End:
            _targetSelectionGUI.EndSelectionScheme();
            _activlySelecting = false;
        }

        #region Private
        public IEnumerator InitializeActionTargetSelectionScheme(CombatEntity caster, IActorAction actorAction, bool randomTarget = false)
        {
            ColorfulLogger.LogWithColor("Start target selection ", Color.green);
            Debug.Log("_activlySelecting is " + _activlySelecting);
            if (_activlySelecting)
                throw new Exception(caster + " Try to active selection scheme while it is already active");

            _activlySelecting = true;

            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);

            OnTargetSelectionSchemeStart.PlayEvent(caster);

            ValidateData();

            AssignPossibleTargets(caster, actorAction.GetTargetSelector());

            CameraSetUPManager.Instance.CaptureVMCamera();

            CameraSetUPManager.Instance.SetBlendMode(CameraSetUPManager.CameraBlendMode.EASE_INOUT, 0.5f);

            _buttonIndicatorWindow.SetIndicatorButtonShow(ButtonIndicatorWindow.IndicatorButtonShow.TARGET, true);

            while (_selectedTarget.Count < actorAction.GetTargetSelector().MaxTarget)
            {
                CombatEntity selected = _validTargets[_currentSelectIndex];
                if (selected.TryGetComponent(out CombatGraphicalHandler cgh))
                {
                    cgh.EnableQuickMenuBar(true);
                }

                if (!randomTarget)
                {
                    CameraSetUPManager.Instance.SetupTargatModeLookAt(selected.gameObject);
                    _targetSelectionGUI.SelectTargetPointer(selected);
                }

                if (_forceStop)
                {
                    _forceStop = false;
                    if (cgh != null)
                    {
                        cgh.EnableQuickMenuBar(false);
                    }
                    goto End;
                }
                if (randomTarget)
                {
                    _currentSelectIndex = UnityEngine.Random.Range(0, _validTargets.Count);
                    _selectedTarget.Add(_validTargets[_currentSelectIndex]);
                    _validTargets.RemoveAt(_currentSelectIndex);
                    if (cgh != null)
                    {
                        cgh.EnableQuickMenuBar(false);
                    }
                    continue;
                }
                yield return new WaitForEndOfFrame();

                if (cgh != null)
                {
                    cgh.EnableQuickMenuBar(false);
                }
            }
            actorAction.SetActionTarget(_selectedTarget);
            caster.AddActionQueue(actorAction);
        End:
            _targetSelectionGUI.EndSelectionScheme();

            OnTargetSelectionSchemeEnd.PlayEvent(caster);

            CameraSetUPManager.Instance.RestoreVMCameraState();
            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);


                 
            _activlySelecting = false;


        }

        //TODO : Assign possible target should have more detail, like "dead or not dead" , "got some status effect or not
        private void AssignPossibleTargets(CombatEntity caster, TargetSelector targetSelector)
        {
            ECompetatorSide eCompetatorSide = CombatReferee.instance.GetCompetatorSide(caster);  //CombatReferee.instance.GetCharacterSide(caster);
            ;  // CombatReferee.instance.GetCharacterSide(caster);

            if (targetSelector.TargetOppose)
                eCompetatorSide = (ECompetatorSide)(((int)eCompetatorSide + 1) % 2);

            foreach (CombatEntity target in CombatReferee.instance.GetCompetatorsBySide(eCompetatorSide))
                _validTargets.Add(target);

            if (targetSelector.TargetBoth)
            {
                eCompetatorSide = (ECompetatorSide)(((int)eCompetatorSide + 1) % 2);

                foreach (CombatEntity target in CombatReferee.instance.GetCompetatorsBySide(eCompetatorSide))
                    _validTargets.Add(target);
            }

            if (targetSelector.TargetCasterItself)
            {
                _validTargets.Clear();
                _validTargets.Add(caster);
            }

            //If no need for dead target, remove them
            for (int i = 0; i < _validTargets.Count; i++)
            {
                if (_validTargets[i].IsDead)
                {
                    _validTargets.RemoveAt(i);
                    i--;
                }
            }
        }

        private void ValidateData()
        {
            _validTargets = new List<CombatEntity>();
            _selectedTarget = new List<CombatEntity>();
            _currentSelectIndex = 0;
        }

        public void ReceiveKeys(KeyCode key)
        {
            if (_activlySelecting)
            {
                if (key == (KeyCode.D))
                {
                    _currentSelectIndex = (_currentSelectIndex + 1) > (_validTargets.Count - 1) ? _currentSelectIndex : (_currentSelectIndex + 1);

                }
                else if (key == (KeyCode.A))
                {
                    _currentSelectIndex = (_currentSelectIndex - 1) < 0 ? 0 : (_currentSelectIndex - 1);

                }
                else if (key == (KeyCode.Space))
                {
                    _buttonIndicatorWindow.SetIndicatorButtonShow(ButtonIndicatorWindow.IndicatorButtonShow.MAIN, false);
                    _buttonIndicatorWindow.ClosePanel();
                    _selectedTarget.Add(_validTargets[_currentSelectIndex]);
                    _validTargets.RemoveAt(_currentSelectIndex);
                    if (_validTargets.Count != 0)
                        _currentSelectIndex = _currentSelectIndex % _validTargets.Count;

                }
                else if (key == (KeyCode.Q))
                {
                    ForceStop();
                    _buttonIndicatorWindow.SetIndicatorButtonShow(ButtonIndicatorWindow.IndicatorButtonShow.MAIN, true);
                }
            }
        }
        #endregion
    }


    [Serializable]
    public class TargetSelector
    {
        private enum TargetSide
        {
            Self, Oppose, Both
        }
        [Header("Maximum target that can be assigned to")]
        [SerializeField]
        private int _maxTargetSize = 1;

        public int MaxTarget => _maxTargetSize;

        [SerializeField]
        private TargetSide _targetSide = TargetSide.Self;

        [SerializeField]
        private bool _targetCaster = false;



        public bool TargetOppose => (_targetSide == TargetSide.Oppose);
        public bool TargetCasterItself => _targetCaster;

        public bool TargetBoth => (_targetSide == TargetSide.Both);


        //Used when the target selection is requires  
    }
}