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
        [SerializeField]
        private TextMeshProUGUI expReqText;
        private PersonalityRewardData _personalityReward;
        private int charmVal;
        private int kindnessVal;
        private int knowledgeVal;
        private int proficiencyVal;
        private float expReqVal;
        [SerializeField]
        private PlayableDirector introDirector;
        private PersonalityTrait personalityTrait;
        private Trait.Trait_Type trait;

        [Header("From now are temp value TODO : Delete this and get these value from backen database")]
        private float gotoValue = 1000.0f;

        public void SetPersonalDataReceive(PersonalityRewardData prd)
        {
            _personalityReward = prd;
            personalityTrait = PersistentPlayerPersonalDataManager.Instance.player_personalityTrait;


            expReqVal = personalityTrait.GetCurrentTraitRequireEXP(0);

            charmVal = personalityTrait.GetStat(Trait.Trait_Type.Charm);
            kindnessVal = personalityTrait.GetStat(Trait.Trait_Type.Kindness);
            knowledgeVal = personalityTrait.GetStat(Trait.Trait_Type.Knowledge);
            proficiencyVal = personalityTrait.GetStat(Trait.Trait_Type.Proficiency);
            //Stat = lvl, ExpReq = exp need;
            //Debug.Log("charmVal stat: " + charmVal);
            //Debug.Log("Charm req: " + personalityTrait.GetCurrentTraitRequireEXP(Trait.Trait_Type.Charm));
            //personalityTrait.SetStat(Trait.Trait_Type.Charm, 2);
            
            //curTraitVal1 = _personalityReward.CurTrait1;
            //curTraitVal2 = _personalityReward.CurTrait2;
            //curTraitVal3 = _personalityReward.CurTrait3;
            //curTraitVal4 = _personalityReward.CurTrait4;
        }

        //Get trait reward obtain each type
        //Get current trait value
        //Get exp req for each one


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
            float curExpGain = 0f;
            float previ = 0;
            float i = 0;
            expReqVal = personalityTrait.GetCurrentTraitRequireEXP(Trait.Trait_Type.Charm);
            //curExpGain = //current exp has;
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

                previ = i;
                i = Mathf.Lerp(startValue, endValue, currentTime / _animationDuration);
                curExpGain += (i - previ);

                int toExp = personalityTrait.GetCurrentTraitRequireEXP(Trait.Trait_Type.Charm);
                expReqText.text = curExpGain + "/" + toExp.ToString();
                traitGauge1.fillAmount = (float)Math.Floor(curExpGain) / toExp;

                _testTextUGUI.text = i.ToString();

                if (Math.Floor(curExpGain) >= expReqVal) //level up
                {
                    Debug.Log(curExpGain + ", " + Math.Floor(curExpGain));
                    curExpGain = curExpGain - expReqVal;
                    Debug.Log("Lvl trait up to: " + (personalityTrait.GetStat(Trait.Trait_Type.Charm) + 1).ToString());
                    yield return DisplayLevelUp();
                    personalityTrait.SetStat(Trait.Trait_Type.Charm, personalityTrait.GetStat(Trait.Trait_Type.Charm) + 1);   //plus level by 1
                    expReqVal = personalityTrait.GetCurrentTraitRequireEXP(Trait.Trait_Type.Charm);
                    expReqText.text = curExpGain + "/" + expReqVal.ToString();
                    traitGauge1.fillAmount = (float)Math.Floor(curExpGain) / expReqVal;
                }
                yield return null;
            }
            //Snap to finish
            _testTextUGUI.text = gotoValue.ToString();
            //CovertValueToGaugeRange(gotoValue, 1000, traitGauge1);
            //CovertValueToGaugeRange(gotoValue, 500, traitGauge2);
            //CovertValueToGaugeRange(gotoValue, 700, traitGauge3);
            //CovertValueToGaugeRange(gotoValue, 2000, traitGauge4);
        }

        private void CovertValueToGaugeRange(float inputVal ,int maxVal, Image gauage)
        {
            float convertedNumber = inputVal % maxVal;
            //float convertedNumber = Mathf.FloorToInt(inputVal) % maxVal;
            gauage.fillAmount = convertedNumber / maxVal;
        }

        private IEnumerator DisplayLevelUp()
        {
            levelUpPanel.SetActive(true);

            yield return new WaitForSeconds(0.1f);
            levelUpPanel.SetActive(false);
        }
    }
}
