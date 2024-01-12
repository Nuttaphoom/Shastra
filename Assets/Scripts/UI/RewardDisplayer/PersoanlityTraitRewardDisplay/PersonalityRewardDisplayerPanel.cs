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
using UnityEngine.UI;

namespace Vanaring 
{
    public class PersonalityRewardDisplayerPanel : BaseRewardDisplayerPanel
    {
        [SerializeField]
        protected TextMeshProUGUI _testTextUGUI;

        [SerializeField]
        private float _animationDuration = 3.0f;

        [SerializeField]
        private Image traitGauge1;
        [SerializeField]
        private Image traitGauge2;
        [SerializeField]
        private Image traitGauge3;
        [SerializeField]
        private Image traitGauge4;
        [SerializeField]
        private GameObject levelUpPanel;
        private PersonalityRewardData _personalityReward;
        //private float curTraitVal1;
        //private float curTraitVal2;
        //private float curTraitVal3;
        //private float curTraitVal4;
        //private float curTraitLvl1;
        //private float curTraitLvl2;
        //private float curTraitLvl3;
        //private float curTraitLvl4;
        //private float goTraitVal1;
        //private float goTraitVal2;
        //private float goTraitVal3;
        //private float goTraitVal4;
        [SerializeField]
        private PlayableDirector introDirector;

        [Header("From now are temp value TODO : Delete this and get these value from backen database")]
        private float gotoValue = 1207.2f;

        public void SetPersonalDataReceive(PersonalityRewardData prd)
        {
            _personalityReward = prd;
            //curTraitVal1 = _personalityReward.CurTrait1;
            //curTraitVal2 = _personalityReward.CurTrait2;
            //curTraitVal3 = _personalityReward.CurTrait3;
            //curTraitVal4 = _personalityReward.CurTrait4;
        }

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
            float currentTime = 0f;
            float startValue = 0f;
            float endValue = gotoValue;
            float nextLvlVal = 200;
            introDirector.Play();
            while(introDirector.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }

            while (currentTime < _animationDuration)
            {
                if (IsSettingUpSucessfully)
                    break;

                currentTime += Time.deltaTime;

                float i = Mathf.Lerp(startValue, endValue, currentTime / _animationDuration);

                //Debug.Log(i);

                CovertValueToGaugeRange(i, 1000, traitGauge1);
                CovertValueToGaugeRange(i, 500, traitGauge2);
                CovertValueToGaugeRange(i, 700, traitGauge3);
                CovertValueToGaugeRange(i, 2000, traitGauge4);

                _testTextUGUI.text = i.ToString();


                if(i > nextLvlVal)
                {
                    yield return DisplayLevelUp();
                    nextLvlVal += nextLvlVal;
                }

                yield return null;
            }

            //Snap to finish
            _testTextUGUI.text = gotoValue.ToString();
            CovertValueToGaugeRange(gotoValue, 1000, traitGauge1);
            CovertValueToGaugeRange(gotoValue, 500, traitGauge2);
            CovertValueToGaugeRange(gotoValue, 700, traitGauge3);
            CovertValueToGaugeRange(gotoValue, 2000, traitGauge4);
        }

        private void CovertValueToGaugeRange(float inputVal ,int maxVal, Image gauage)
        {
            float convertedNumber = Mathf.FloorToInt(inputVal) % maxVal;
            gauage.fillAmount = convertedNumber / maxVal;
        }

        private IEnumerator DisplayLevelUp()
        {
            levelUpPanel.SetActive(true);
            Debug.Log("Lvl up");
            yield return new WaitForSeconds(2.0f);
            levelUpPanel.SetActive(false);
            //Debug.Log("next");
        }
    }
}
