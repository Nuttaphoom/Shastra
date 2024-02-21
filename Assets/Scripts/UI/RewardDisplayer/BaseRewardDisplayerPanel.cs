using System;
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
        public virtual IEnumerator SettingUpNumber()
        {
            throw new NotImplementedException();
        }
        public virtual void ForceSetUpNumber()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function doesn't require player to input 
        /// any keys or interaction to the DisplayerPanel
        /// </summary>
        public virtual IEnumerator AutoRunDisplayerScheme()
        {
            throw new NotImplementedException();
        }

        protected bool _displayingUIDone = false; 
       
        public bool IsFinishingDisplayUI => _displayingUIDone;  
        public bool IsSettingUpSucessfully => _uiAnimationDone;

        #region PanelInteraction 
        public virtual void OnContinueButtonClick()
        {
            throw new NotImplementedException();
        }



        #endregion
    }
}
