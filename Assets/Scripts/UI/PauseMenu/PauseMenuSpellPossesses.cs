using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class PauseMenuSpellPossesses : PauseMenuWindowGUI
    {
        public override void ClearData()
        {
            
        }

        public override void LoadWindowData(CombatEntity entity)
        {
            
        }

        public override void OnWindowActive()
        {
            
        }

        public override void OnWindowDeActive()
        {
            
        }

        public override void ReceiveKeysFromWindowManager(KeyCode key)
        {
            if (key == KeyCode.Q)
            {
                _pauseMenuWindowGUI.OpenWindow(EPauseWindowGUI.Party);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
