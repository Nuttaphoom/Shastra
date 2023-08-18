using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Vanaring_DepaDemo;

namespace Vanaring_DepaDemo
{
    public class MainWindowManager : HierarchyUIWindow, IInputReceiver
    {
        [SerializeField]
        private CombatGraphicalHandler _combatGraphicalHandler;

        [Header("Button to bind call back")]
        [SerializeField]
        private Button _itemButton;
        [SerializeField]
        private Button _spellButton;
        [SerializeField]
        private Button _weaponButton;

        private int _currentSelectedWindow = 0;

        private List<Button> _buttons;

        [SerializeField]
        private TargetSelectionGUI _targetSelectionGUI;

        private Coroutine _targetingCoroutine = null;

        private void Awake()
        {
            _targetSelectionGUI.Initialize(transform) ; 
            
            _buttons = new List<Button>();
            if (_itemButton == null || _spellButton == null || _weaponButton == null)
                throw new Exception("Buttons hasn't been correctly assigned");

            if (_combatGraphicalHandler == null)
                throw new Exception("Combat Graphical Handler hasn't been assgined");

            _itemButton.onClick.AddListener(() => { _combatGraphicalHandler.DisplayItemPanel(); });
            _spellButton.onClick.AddListener(() => { _combatGraphicalHandler.DisplaySpellPanel(); });
            _weaponButton.onClick.AddListener(() => { _combatGraphicalHandler.DisplayWeaponPanel(); });

            _buttons.Add(_spellButton);
            _buttons.Add(_itemButton);
            _buttons.Add(_weaponButton);
        }

      

        public void TakeInputControl()
        {

            _currentSelectedWindow = 0;
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);

            

        }

        public void ReleaseInputControl()
        {

            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);
            TargetSelectionFlowControl.Instance.ForceStop();



        }

        public void ReceiveKeys(KeyCode key)
        {
            int tempWindow = _currentSelectedWindow;

            if (key == KeyCode.W)
            {
                _currentSelectedWindow -= 1;
                if (_currentSelectedWindow < 0)
                    _currentSelectedWindow = 0;
            }
            else if (key == KeyCode.S)
            {
                _currentSelectedWindow += 1;
                if (_currentSelectedWindow > 2)
                    _currentSelectedWindow = 2;

            }
            else if (key == KeyCode.Space)
            {
                _buttons[_currentSelectedWindow].onClick?.Invoke();
            }
            else if (key == (KeyCode.Q))
            {
                TargetSelectionFlowControl.Instance.ForceStop();

                CombatReferee.instance.ChangeActiveEntityIndex(true, false) ;
            }
            else if (key == (KeyCode.E))
            {
                TargetSelectionFlowControl.Instance.ForceStop();

                CombatReferee.instance.ChangeActiveEntityIndex(false, true);
            }
            else if (key == (KeyCode.D))
            {
                TargetSelectionFlowControl.Instance.ReceiveKeys(KeyCode.D);
            }else if (key == (KeyCode.A))
            {
                TargetSelectionFlowControl.Instance.ReceiveKeys(KeyCode.A);
            }



            if (tempWindow != _currentSelectedWindow)
            {

            }
        }

        private IEnumerator TargettingTarget()
        {
            IEnumerator coroutine = (TargetSelectionFlowControl.Instance.InitializeTargetSelectionSchemeWithoutSelect(CombatReferee.instance.GetCompetatorsBySide(ECompetatorSide.Hostile)));
            while (coroutine.MoveNext())
            {
                if (coroutine.Current != null && coroutine.Current.GetType().IsSubclassOf(typeof(CombatEntity)))
                {
                    //POTAE DO SOMETHING HERE
                }
                yield return null;
            }
        } 
        public override void OnWindowDisplay(CombatGraphicalHandler graophicalHandler)
        {
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
            _targetingCoroutine = StartCoroutine(TargettingTarget());
            SetGraphicMenuActive(true);

        }

        public override void OnWindowOverlayed()
        {
            TargetSelectionFlowControl.Instance.ForceStop();
            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);

            SetGraphicMenuActive(false);
        }
    }
}


