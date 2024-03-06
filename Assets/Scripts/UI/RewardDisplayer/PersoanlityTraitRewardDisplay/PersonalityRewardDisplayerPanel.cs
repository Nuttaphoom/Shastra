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
        private float _animationDuration = 3.0f;
        private List<Trait.Trait_Type> traitRewardShowList = new List<Trait.Trait_Type>();
        private int coroutineRunningCount = 0;

        [Header("TraitGauge")]
        [SerializeField] private PersonalityTraitRewardUIObject uiObjectTemplate;
        private List<PersonalityTraitRewardUIObject> gaugeFillList = new List<PersonalityTraitRewardUIObject>();
        [SerializeField] private List<Transform> uiTransformList = new List<Transform>();

        [Header("TraitRewardPanel")]
        [SerializeField] private GameObject levelUpPanel;
        [SerializeField] private Image levelUpTraitIcon;
        [SerializeField] private TextMeshProUGUI levelUpRewardText;

        private List<PersonalityRewardData> _personalityRewardList;
        
        [SerializeField]
        private PlayableDirector introDirector;
        private PersonalityTrait personalityTrait;

        public void SetPersonalDataReceive(List<PersonalityRewardData> personalRewardDataList)
        {
            _personalityRewardList = personalRewardDataList;
            personalityTrait = PersistentPlayerPersonalDataManager.Instance.GetPersonalityTrait ;
            Trait.Trait_Type[] allTrait = (Trait.Trait_Type[])Enum.GetValues(typeof(Trait.Trait_Type));
            for (int i = 0; i < allTrait.Length; i++)
            {
                PersonalityTraitRewardUIObject newUIObj = Instantiate(uiObjectTemplate, uiTransformList[i]);
                newUIObj.Init(allTrait[i], personalityTrait.GetStat(allTrait[i]).GetPersonalityTraitIcon, 
                    personalityTrait.GetStat(allTrait[i]).Getlevel(), 
                    (float)Math.Floor(personalityTrait.GetStat(allTrait[i]).GetCurrentexp()) / personalityTrait.GetCurrentTraitRequireEXP(allTrait[i]));
                gaugeFillList.Add(newUIObj);
            }
            uiObjectTemplate.gameObject.SetActive(false);
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
            yield return new WaitForSeconds(1.0f);
            introDirector.Stop();
            int i = 0;
            foreach (PersonalityRewardData traitReward in _personalityRewardList)
            {
                foreach (PersonalityTraitRewardUIObject gauge in gaugeFillList)
                {
                    if (gauge.IsTraitTypeEqual(traitReward.RewardTraitType))
                    {
                        //Debug.Log("Found " + traitReward.RewardTraitType + ": " + personalityTrait.GetStat(traitReward.RewardTraitType).GetCurrentexp());
                        StartCoroutine(GuageDisplay(personalityTrait.GetStat(traitReward.RewardTraitType).GetCurrentexp(),
                                traitReward.RewardAmount, traitReward.RewardTraitType, gauge));
                    }
                }
                coroutineRunningCount++;
            }
            while (coroutineRunningCount > 0)
            {
                yield return null;
            }
            _uiAnimationDone = true;
            //Snap
            yield return new WaitForSeconds(0.5f);
            if (traitRewardShowList.Count > 0)
            {
                StartCoroutine(DisplayLevelUp(traitRewardShowList));
            }
        }
        private IEnumerator GuageDisplay(float currentVal , float rewardVal, Trait.Trait_Type type, PersonalityTraitRewardUIObject gaugeObj)
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
                {
                    currentTime += 1.0f;
                }
                else
                {
                    currentTime += Time.deltaTime;
                }
                float previ = i;
                i = Mathf.Lerp(startValue, endValue, currentTime / _animationDuration);
                curExpGain += (i - previ);

                gaugeObj.FillBar.fillAmount = (float)Math.Floor(curExpGain) / personalityTrait.GetCurrentTraitRequireEXP(type);

                if (Math.Floor(curExpGain) >= expReqVal) //level up condition
                {
                    curExpGain = curExpGain - expReqVal;
                    isTraitHasReward = true;
                    personalityTrait.SetStat(type, personalityTrait.GetStat(type).Getlevel()+1, curExpGain);
                    gaugeObj.TraitLevel.text = personalityTrait.GetStat(type).Getlevel().ToString();
                    expReqVal = personalityTrait.GetCurrentTraitRequireEXP(type);
                    gaugeObj.FillBar.fillAmount = (float)Math.Floor(curExpGain) / expReqVal;
                    gaugeObj.TriggerLevelUPAnimation();
                }
                yield return null;
            }
            personalityTrait.SetStat(type, personalityTrait.GetStat(type).Getlevel(), curExpGain);
            if (isTraitHasReward)
            {
                traitRewardShowList.Add(type);
            }
            coroutineRunningCount--;
        }

        private IEnumerator DisplayLevelUp(List<Trait.Trait_Type> displayTraitRewardList)
        {
            for (int i = 0; i < displayTraitRewardList.Count; i++)
            {
                levelUpPanel.SetActive(true);
                levelUpTraitIcon.sprite = personalityTrait.GetStat(displayTraitRewardList[i]).GetPersonalityTraitIcon;
                levelUpRewardText.text = displayTraitRewardList[i].ToString() + " has reached rank " + 
                    personalityTrait.GetStat(displayTraitRewardList[i]).Getlevel().ToString() + "!";
                while (levelUpPanel.activeSelf)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            traitRewardShowList.Clear();
            yield return null;
        }
    }
}
