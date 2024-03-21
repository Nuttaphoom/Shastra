using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public abstract class WindowGUI : MonoBehaviour
    {
        public abstract void ClearData();
        public abstract void OnWindowActive();
        public abstract void OnWindowDeActive();
        public abstract void LoadWindowData(CombatEntity entity);

        public abstract void ReceiveKeysFromWindowManager(KeyCode key);


    }
}
