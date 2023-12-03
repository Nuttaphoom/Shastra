using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


namespace Vanaring 
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

        private Color _hightlightedColor = Color.yellow ;
        private Color _defaultColor;

        private List<Vector3> _defaultLocalScale;
        private float modifiedSize = 1.25f ;

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


            _defaultLocalScale = new List<Vector3>(); 
            for (int i = 0;  i < _buttons.Count; i++)
            {
                _defaultLocalScale.Add(_buttons[i].transform.localScale) ;
            }
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
            else if (key == (KeyCode.A))
            {
                if (CombatReferee.Instance.ChangeActiveEntityIndex(true))
                    TargetSelectionFlowControl.Instance.ForceStop();

            }
            else if (key == (KeyCode.D))
            {
                //throw new NotImplementedException();

                if (CombatReferee.Instance.ChangeActiveEntityIndex(false))
                    TargetSelectionFlowControl.Instance.ForceStop();
            }
            else if (key == (KeyCode.RightArrow))
            {
                TargetSelectionFlowControl.Instance.ReceiveKeys(KeyCode.D);
            }else if (key == (KeyCode.LeftArrow))
            {
                TargetSelectionFlowControl.Instance.ReceiveKeys(KeyCode.A);
            }

            if (tempWindow != _currentSelectedWindow)
            {

            }


            HightLightButton(_currentSelectedWindow);
        }

        //private IEnumerator TargettingTarget()
        //{
        //    CombatEntity selectedEntity = null ;
        //    List<CombatEntity> entities = CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Hostile); 
        //    for (int i = 0;  i < entities.Count; i++)
        //    {
        //        if (entities[i].IsDead)
        //        {
        //            entities.RemoveAt(i);
        //            i--; 
        //        } 
        //    }
        //    IEnumerator coroutine = (TargetSelectionFlowControl.Instance.InitializeTargetSelectionSchemeWithoutSelect(entities));
        //    while (coroutine.MoveNext())
        //    {
        //        if (coroutine.Current != null)
        //        {
        //            if (coroutine.Current is (CombatEntity))
        //            {
        //                if (selectedEntity != coroutine.Current as CombatEntity)
        //                {

        //                    selectedEntity = coroutine.Current as CombatEntity;
        //                    TargetInfoWindowManager.instance.ShowCombatEntityInfoUI(selectedEntity);
        //                }
        //            }
        //        }
        //        yield return null;
        //    }

        //    TargetInfoWindowManager.instance.HideCombatEntityInfoUI() ;
        //}
        public override void OnWindowDisplay(CombatGraphicalHandler graophicalHandler)
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                UnhightlightButton(i); 
            }

            _currentSelectedWindow = 0; 
            HightLightButton(_currentSelectedWindow);  
             
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
            //_targetingCoroutine = StartCoroutine(TargettingTarget());
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
            _buttons[index].transform.DOScale(_defaultLocalScale[index] * modifiedSize, 0.1f); 
            //_buttons[index].GetComponent<Image>().color = _hightlightedColor;
        }

        private void UnhightlightButton(int index)
        {
            _buttons[index].transform.DOScale(_defaultLocalScale[index], 0.1f);

            //_buttons[index].GetComponent<Image>().color = _defaultColor; 
        }
    }
}


