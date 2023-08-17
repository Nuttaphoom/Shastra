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
    public class MainWindowManager : MonoBehaviour, IInputReceiver
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

        private void Awake()
        {
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
            else if (key == (KeyCode.D))
            {

                CombatReferee.instance.ChangeActiveEntityIndex(true, false) ;
            }
            else if (key == (KeyCode.A))
            {

                CombatReferee.instance.ChangeActiveEntityIndex(false, true);
            }



            if (tempWindow != _currentSelectedWindow)
            {

            }
        }




    }
}


