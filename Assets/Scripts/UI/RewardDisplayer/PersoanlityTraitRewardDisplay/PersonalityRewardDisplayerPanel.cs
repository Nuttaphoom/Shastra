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
        private bool isHasReward = false;

        [SerializeField] private Image traitGauge1;

        [SerializeField] private Image traitGauge2;

        [SerializeField] private Image traitGauge3;

        [SerializeField] private Image traitGauge4;

        [SerializeField] private GameObject levelUpPanel;

        [SerializeField]
        private TextMeshProUGUI expReqText;
        private PersonalityRewardData _personalityReward;
        private int charmVal;
        private int kindnessVal;
        private int knowledgeVal;
        private int proficiencyVal;
        
        [SerializeField]
        private PlayableDirector introDirector;
        private PersonalityTrait personalityTrait;
        private Trait.Trait_Type trait;

        [Header("From now are temp value TODO : Delete this and get these value from backen database")]
        private float gotoValue = 1669.0f;
        private float trait1Reward = 999.0f;
        private float trait2Reward = 200.0f;
        private float trait3Reward = 20.0f;
        private float trait4Reward = 399.0f;

        [SerializeField] private TextMeshProUGUI levelCharmText;
        [SerializeField] private TextMeshProUGUI levelKindText;
        [SerializeField] private TextMeshProUGUI levelKnowText;
        [SerializeField] private TextMeshProUGUI levelProfText;

        public void SetPersonalDataReceive(PersonalityRewardData prd)
        {
            _personalityReward = prd;
            personalityTrait = PersistentPlayerPersonalDataManager.Instance.player_personalityTrait;
            levelUpPanel.SetActive(false);
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
            introDirector.Play();
            while(introDirector.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
            
            StartCoroutine(GuageDisplay(personalityTrait.GetStat(Trait.Trait_Type.Charm).GetCurrentexp(), trait1Reward, Trait.Trait_Type.Charm, traitGauge1, levelCharmText));
            StartCoroutine(GuageDisplay(personalityTrait.GetStat(Trait.Trait_Type.Kindness).GetCurrentexp(), trait2Reward, Trait.Trait_Type.Kindness, traitGauge2, levelKindText));
            StartCoroutine(GuageDisplay(personalityTrait.GetStat(Trait.Trait_Type.Knowledge).GetCurrentexp(), trait3Reward, Trait.Trait_Type.Knowledge, traitGauge3, levelKnowText));
            StartCoroutine(GuageDisplay(personalityTrait.GetStat(Trait.Trait_Type.Proficiency).GetCurrentexp(), trait4Reward, Trait.Trait_Type.Proficiency, traitGauge4, levelProfText));
            yield return new WaitForSeconds(_animationDuration);

            //Snap to finish
            _testTextUGUI.text = gotoValue.ToString();
            if (isHasReward)
            {
                StartCoroutine(DisplayLevelUp());
            }
        }

        private IEnumerator GuageDisplay(float currentVal , float rewardVal, Trait.Trait_Type type, Image gauge, TextMeshProUGUI lvText)
        {
            float currentTime = 0f;
            float startValue = 0f;
            float endValue = rewardVal;
            float curExpGain = currentVal;
            float previ = 0;
            float i = 0;
            float expReqVal = personalityTrait.GetCurrentTraitRequireEXP(type);

            while (currentTime < _animationDuration)
            {
                if (IsSettingUpSucessfully)
                    break;

                currentTime += Time.deltaTime;

                previ = i;
                i = Mathf.Lerp(startValue, endValue, currentTime / _animationDuration);
                curExpGain += (i - previ);

                //expReqText.text = curExpGain + "/" + personalityTrait.GetCurrentTraitRequireEXP(Trait.Trait_Type.Charm).ToString();
                gauge.fillAmount = (float)Math.Floor(curExpGain) / personalityTrait.GetCurrentTraitRequireEXP(type);

                if (Math.Floor(curExpGain) >= expReqVal) //level up condition
                {
                    Debug.Log(curExpGain + ", " + Math.Floor(curExpGain));
                    curExpGain = curExpGain - expReqVal;
                    Debug.Log("Lvl trait " + type.ToString() + " up to: " + (personalityTrait.GetStat(type).Getlevel() + 1).ToString());

                    isHasReward = true;
                    personalityTrait.SetStat(type, personalityTrait.GetStat(type).Getlevel()+1, curExpGain);   //plus level by 1
                    lvText.text = personalityTrait.GetStat(type).Getlevel().ToString();
                    expReqVal = personalityTrait.GetCurrentTraitRequireEXP(type);   //set to next lvl
                    //expReqText.text = curExpGain + "/" + expReqVal.ToString();
                    gauge.fillAmount = (float)Math.Floor(curExpGain) / expReqVal;
                }
                
                yield return null;
            }
        }

        private IEnumerator DisplayLevelUp()
        {
            levelUpPanel.SetActive(true);
            
            yield return new WaitForSeconds(5.0f);
            levelUpPanel.SetActive(false);
        }


    }
}
