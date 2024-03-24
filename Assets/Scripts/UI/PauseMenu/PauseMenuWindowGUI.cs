using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public abstract class PauseMenuWindowGUI : WindowGUI
    {
        protected PauseMenuWindowManager _pauseMenuWindowGUI;
        public void Init(PauseMenuWindowManager pauseMenuWindowGUI)
        {
            _pauseMenuWindowGUI = pauseMenuWindowGUI;
        }
    }
    public enum EPauseWindowGUI
    {
        Main, Party
    }
}
