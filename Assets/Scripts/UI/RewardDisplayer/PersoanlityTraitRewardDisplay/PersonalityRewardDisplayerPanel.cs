﻿using NaughtyAttributes;
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
        private List<Trait.Trait_Type> traitRewardShowList = new List<Trait.Trait_Type>();

        [Header("TraitGauge")]
        [SerializeField] private Image traitGauge1;
        [SerializeField] private Image traitGauge2;
        [SerializeField] private Image traitGauge3;
        [SerializeField] private Image traitGauge4;
        [SerializeField] private TextMeshProUGUI levelCharmText;
        [SerializeField] private TextMeshProUGUI levelKindText;
        [SerializeField] private TextMeshProUGUI levelKnowText;
        [SerializeField] private TextMeshProUGUI levelProfText;

        [Header("TraitRewardPanel")]
        [SerializeField] private GameObject levelUpPanel;
        [SerializeField] private TextMeshProUGUI levelUpText;
        [SerializeField] private TextMeshProUGUI levelUpRewardText;

        private List<PersonalityRewardData> _personalityRewardList;
        
        [SerializeField]
        private PlayableDirector introDirector;
        private PersonalityTrait personalityTrait;

        

        public void SetPersonalDataReceive(List<PersonalityRewardData> personalRewardDataList)
        {
            _personalityRewardList = personalRewardDataList;
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
            //while(introDirector.state == PlayState.Playing)
            //{
            //    yield return new WaitForEndOfFrame();
            //}
            yield return new WaitForSeconds(1.0f);
            introDirector.Stop();
            foreach (PersonalityRewardData traitReward in _personalityRewardList)
            {
                switch (traitReward.RewardTraitType)
                {
                    case Trait.Trait_Type.Charm:
                        StartCoroutine(GuageDisplay(personalityTrait.GetStat(Trait.Trait_Type.Charm).GetCurrentexp(), 
                            traitReward.RewardAmount, Trait.Trait_Type.Charm, traitGauge1, levelCharmText));
                        break;
                    case Trait.Trait_Type.Kindness:
                        StartCoroutine(GuageDisplay(personalityTrait.GetStat(Trait.Trait_Type.Kindness).GetCurrentexp(), 
                            traitReward.RewardAmount, Trait.Trait_Type.Kindness, traitGauge2, levelKindText));
                        break;
                    case Trait.Trait_Type.Knowledge:
                        StartCoroutine(GuageDisplay(personalityTrait.GetStat(Trait.Trait_Type.Knowledge).GetCurrentexp(), 
                            traitReward.RewardAmount, Trait.Trait_Type.Knowledge, traitGauge3, levelKnowText));
                        break;
                    case Trait.Trait_Type.Proficiency:
                        StartCoroutine(GuageDisplay(personalityTrait.GetStat(Trait.Trait_Type.Proficiency).GetCurrentexp(), 
                            traitReward.RewardAmount, Trait.Trait_Type.Proficiency, traitGauge4, levelProfText));
                        break;
                }
                Debug.Log(traitReward.RewardTraitType + ": " + traitReward.RewardAmount.ToString());
            }
            yield return new WaitForSeconds(_animationDuration);
            //Snap
            if (traitRewardShowList.Count > 0)
            {
                Debug.Log(traitRewardShowList.Count);
                StartCoroutine(DisplayLevelUp(traitRewardShowList));
            }
        }

        private IEnumerator GuageDisplay(float currentVal , float rewardVal, Trait.Trait_Type type, Image gauge, TextMeshProUGUI lvText)
        {
            float currentTime = 0f;
            float startValue = 0f;
            float endValue = rewardVal;
            float curExpGain = currentVal;
            float i = 0;
            float expReqVal = personalityTrait.GetCurrentTraitRequireEXP(type);
            bool isTraitHasReward = false;

            while (currentTime < _animationDuration)
            {
                if (IsSettingUpSucessfully)
                    break;

                currentTime += Time.deltaTime;

                float previ = i;
                i = Mathf.Lerp(startValue, endValue, currentTime / _animationDuration);
                curExpGain += (i - previ);

                //expReqText.text = curExpGain + "/" + personalityTrait.GetCurrentTraitRequireEXP(Trait.Trait_Type.Charm).ToString();
                gauge.fillAmount = (float)Math.Floor(curExpGain) / personalityTrait.GetCurrentTraitRequireEXP(type);

                if (Math.Floor(curExpGain) >= expReqVal) //level up condition
                {
                    Debug.Log(curExpGain + ", " + Math.Floor(curExpGain));
                    curExpGain = curExpGain - expReqVal;
                    Debug.Log("Lvl trait " + type.ToString() + " up to: " + (personalityTrait.GetStat(type).Getlevel() + 1).ToString());

                    isTraitHasReward = true;
                    //Image test = Instantiate(stamp, gauge.GetComponentInChildren<HorizontalLayoutGroup>().transform);
                    personalityTrait.SetStat(type, personalityTrait.GetStat(type).Getlevel()+1, curExpGain);   //plus level by 1
                    lvText.text = personalityTrait.GetStat(type).Getlevel().ToString();
                    expReqVal = personalityTrait.GetCurrentTraitRequireEXP(type);   //set to next lvl
                    //expReqText.text = curExpGain + "/" + expReqVal.ToString();
                    gauge.fillAmount = (float)Math.Floor(curExpGain) / expReqVal;
                }
                
                yield return null;
            }
            if (isTraitHasReward)
            {
                //Debug.Log("has");
                traitRewardShowList.Add(type);
            }
        }
        private IEnumerator DisplayLevelUp(List<Trait.Trait_Type> displayTraitRewardList)
        {
            for (int i = 0; i < displayTraitRewardList.Count; i++)
            {
                levelUpPanel.SetActive(true);
                levelUpText.text = displayTraitRewardList[i].ToString() + " lv." 
                    + personalityTrait.GetStat(displayTraitRewardList[i]).Getlevel().ToString();
                levelUpRewardText.text = displayTraitRewardList[i].ToString() + " upgrades to rank " + personalityTrait.GetStat(displayTraitRewardList[i]).Getlevel().ToString() + "!";
                while (levelUpPanel.activeSelf)
                {
                    yield return new WaitForEndOfFrame();
                }
                //yield return new WaitForSeconds(3.5f);
                //levelUpPanel.SetActive(false);
            }
            traitRewardShowList.Clear();
            yield return null;
        }
    }
}
