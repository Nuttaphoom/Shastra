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

        public override void ReceiveKeysFromWindowManager(InputCode key)
        {
            if (key == (InputCode.Skill))
            {
                _windowManager.OpenWindow(EWindowGUI.Spell);
            }

            if (key == (InputCode.Item))
            {
                _windowManager.OpenWindow(EWindowGUI.Item);
            }

            if (key == (InputCode.Left))
            {
                if (CombatReferee.Instance.ChangeActiveEntityIndex(true))
                    TargetSelectionFlowControl.Instance.ForceStop();

            }
            if (key == (InputCode.Right))
            {
                if (CombatReferee.Instance.ChangeActiveEntityIndex(false))
                    TargetSelectionFlowControl.Instance.ForceStop();
            }
            if (key == InputCode.T)
            {
                _windowManager.OpenWindow(EWindowGUI.Inspect);
            }
        }
    }
}
