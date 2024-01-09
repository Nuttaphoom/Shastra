using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Vanaring 
{
    public class PersonalityRewardDisplayerPanel : BaseRewardDisplayerPanel
    {
        [SerializeField]
        protected TextMeshProUGUI _testTextUGUI;

        [SerializeField]
        private float _animationDuration = 3.0f; 

        [Header("From now are temp value TODO : Delete this and get these value from backen database")]
        private float gotoValue = 1207.2f;

        public override void ForceSetUpNumber()
        {
            _uiAnimationDone = true;

        }

        public override void OnContinueButtonClick()
        {
            if (IsSettingUpSucessfully)
                _displayingUIDone = true; 

            else
                ForceSetUpNumber(); 
        }

        public override IEnumerator SettingUpNumber()
        {
           
            float currentTime = 0f; // Variable to keep track of the time passed
            float startValue = 0f; // Starting value of i
            float endValue = gotoValue; // Target value of i

            while (currentTime < _animationDuration)
            {
                if (IsSettingUpSucessfully)
                    break;

                currentTime += Time.deltaTime; // Increment time by the time passed since the last frame

                // Calculate the value of i between start and end over time
                float i = Mathf.Lerp(startValue, endValue, currentTime / _animationDuration);

             
                _testTextUGUI.text = i.ToString(); // Update the UI or perform actions with the current value of i

                yield return null; // Wait for the next frame
            }

            _testTextUGUI.text = gotoValue.ToString();  

            //throw new NotImplementedException();
        }
    }
}
