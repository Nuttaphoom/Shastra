using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring  { 
    public abstract class WindowGUI : MonoBehaviour 
    {
        protected PlayerWindowManager _windowManager ;

        /// <summary>
        /// Init will be call on "Start"
        /// </summary>
        /// <param name="windowManager"></param>
        public void Init(PlayerWindowManager windowManager)
        {
            _windowManager = windowManager;            
        }

        public abstract void OnWindowActive();
        public abstract void OnWindowDeActive();
        public abstract void LoadWindowData(CombatEntity entity);

        public abstract void ReceiveKeysFromWindowManager(KeyCode key); 
    }

    public enum EWindowGUI
    {
        Main, Item, Spell, Weapon 
    }
}
