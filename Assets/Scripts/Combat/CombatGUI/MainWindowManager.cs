using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Editor;
using UnityEngine.UI;
using Vanaring_DepaDemo;

namespace Vanaring_DepaDemo
{
    public class MainWindowManager : HierarchyUIWindow, IInputReceiver
    {

        [Header("Button to bind call back")]
        [SerializeField]
        private Button _itemButton;
        [SerializeField]
        private Button _spellButton;
        [SerializeField]
        private Button _weaponButton;

        private int _currentSelectedWindow = 0;

        private List<Button> _buttons;

        private Coroutine _targetingCoroutine = null;

        private Color _hightlightedColor = Color.yellow;
        private Color _defaultColor;

        private void Awake()
        {
            _buttons = new List<Button>();
            if (_itemButton == null || _spellButton == null || _weaponButton == null)
                throw new Exception("Buttons hasn't been correctly assigned");
 

            _itemButton.onClick.AddListener(() => { _combatGraphicalHandler.DisplayItemPanel(); });
            _spellButton.onClick.AddListener(() => { _combatGraphicalHandler.DisplaySpellPanel(); });
            _weaponButton.onClick.AddListener(() => { _combatGraphicalHandler.DisplayWeaponPanel(); });
            _defaultColor = _spellButton.GetComponent<Image>().color;
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
            UnhightlightButton(_currentSelectedWindow);
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
                if (CombatReferee.instance.ChangeActiveEntityIndex(true, false))
                    TargetSelectionFlowControl.Instance.ForceStop();

            }
            else if (key == (KeyCode.E))
            {
                if (CombatReferee.instance.ChangeActiveEntityIndex(false, true))
                    TargetSelectionFlowControl.Instance.ForceStop();
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


            HightLightButton(_currentSelectedWindow);
        }

        private IEnumerator TargettingTarget()
        {
            CombatEntity selectedEntity = null ;
            List<CombatEntity> entities = CombatReferee.instance.GetCompetatorsBySide(ECompetatorSide.Hostile); 
            for (int i = 0;  i < entities.Count; i++)
            {
                if (entities[i].IsDead)
                {
                    entities.RemoveAt(i);
                    i--; 
                } 
            }
            IEnumerator coroutine = (TargetSelectionFlowControl.Instance.InitializeTargetSelectionSchemeWithoutSelect(entities));
            while (coroutine.MoveNext())
            {
                if (coroutine.Current != null)
                {
                    if (coroutine.Current is (CombatEntity))
                    {
                        if (selectedEntity != coroutine.Current as CombatEntity)
                        {

                            selectedEntity = coroutine.Current as CombatEntity;
                            TargetInfoWindowManager.instance.ShowCombatEntityInfoUI(selectedEntity);
                        }
                    }
                }
                yield return null;
            }

            TargetInfoWindowManager.instance.HideCombatEntityInfoUI() ;
        }
        public override void OnWindowDisplay(CombatGraphicalHandler graophicalHandler)
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                UnhightlightButton(i); 
            }

            _currentSelectedWindow = 0; 
            HightLightButton(_currentSelectedWindow);  
             
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
            _targetingCoroutine = StartCoroutine(TargettingTarget());
            SetGraphicMenuActive(true);

        }

        public override void OnWindowOverlayed()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                UnhightlightButton(i);
            }

            TargetSelectionFlowControl.Instance.ForceStop();
            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);

            SetGraphicMenuActive(false);
        }

        private void HightLightButton(int index)
        {
            _buttons[index].GetComponent<Image>().color = _hightlightedColor;
        }

        private void UnhightlightButton(int index)
        {
            _buttons[index].GetComponent<Image>().color = _defaultColor; 
        }
    }
}


