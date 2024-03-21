using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class MainWindowGUI : CombatWindowGUI
    {
        [SerializeField]
        private Button _spellButton;

        [SerializeField]
        private Button _itemButton; 

        public override void OnWindowActive()
        {

        }

        public override void OnWindowDeActive()
        {

        }
        public override void LoadWindowData(CombatEntity entity)
        {
            _spellButton.onClick.AddListener(() => _windowManager.OpenWindow(EWindowGUI.Spell) );
            _itemButton.onClick.AddListener(() => _windowManager.OpenWindow(EWindowGUI.Item)); 

        }

        public override void ClearData()
        {
            return; 
        }

        public override void ReceiveKeysFromWindowManager(KeyCode key)
        {
            if (key == (KeyCode.Q))
            {
                _windowManager.OpenWindow(EWindowGUI.Spell);
            }

            if (key == (KeyCode.E))
            {
                _windowManager.OpenWindow(EWindowGUI.Item);
            }

            if (key == (KeyCode.A))
            {
                if (CombatReferee.Instance.ChangeActiveEntityIndex(true))
                    TargetSelectionFlowControl.Instance.ForceStop();

            }
            if (key == (KeyCode.D))
            {
                if (CombatReferee.Instance.ChangeActiveEntityIndex(false))
                    TargetSelectionFlowControl.Instance.ForceStop();
            }
        }
    }
}
