﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public abstract class BaseRewardDisplayerPanel : MonoBehaviour
    {
        protected bool _uiAnimationDone = false; 
        public abstract IEnumerator SettingUpNumber();
        public abstract void ForceSetUpNumber();
        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public bool IsSettingUpSucessfully => _uiAnimationDone;

        #region PanelInteraction 
        public abstract void OnContinueButtonClick();

        #endregion
    }
}
