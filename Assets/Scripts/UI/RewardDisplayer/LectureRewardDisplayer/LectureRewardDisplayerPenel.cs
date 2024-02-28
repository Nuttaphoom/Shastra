using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Unity.Mathematics;
using System;
using UnityEngine.EventSystems;
using TMPro;

namespace Vanaring
{
    public class LectureRewardDisplayerPenel : BaseRewardDisplayerPanel
    {
        [SerializeField]
        private PlayableDirector introDirector;
        [SerializeField]
        private Image filledBar;
        [SerializeField]
        private LectureRewardButtonObject rewardButton;
        [SerializeField]
        private BonusTraitObject templateBonusTraitObj;
        private List<BonusTraitObject> reachBonusTraitObjectList = new List<BonusTraitObject>();
        [SerializeField]
        private GameObject bonusVerticalLayout;
        
        [Header("PopupRewardPanel")]
        private List<LectureChechpoint> obtainedRewardList = new List<LectureChechpoint>();
        private List<Sprite> obtainedRewardIcon = new List<Sprite>();
        private List<string> obtainedRewardName = new List<string>();
        [SerializeField] private GameObject popUpRewardPanel;
        [SerializeField] private Image popUpRewardIcon;
        [SerializeField] private TextMeshProUGUI popUpRewardInformText;

        private List<LectureRewardButtonObject> rewardButtonList = new List<LectureRewardButtonObject>();
        private LectureRewardStruct lectureProgressBarData;

        public void ReceiveRewardDetail(LectureRewardStruct rewardData)
        {
            lectureProgressBarData = rewardData;
            PersonalityTrait personalityTrait = PersistentPlayerPersonalDataManager.Instance.GetPersonalityTrait;
            foreach (LectureRequieBoost boost in lectureProgressBarData.boostLecture)
            {
                BonusTraitObject newBonusTrait = Instantiate(templateBonusTraitObj, bonusVerticalLayout.transform);
                bool isLvReach = false;
                if(personalityTrait.GetStat(boost.GetTrait).Getlevel() >= boost.RequireLevel)
                {
                    isLvReach = true;
                }
                newBonusTrait.Init(boost.GetTrait, boost.RequireLevel.ToString(),
                    isLvReach, personalityTrait.GetStat(boost.GetTrait).Getlevel().ToString());
                if (newBonusTrait.IsLvReach)
                {
                    reachBonusTraitObjectList.Add(newBonusTrait);
                }
            }
            templateBonusTraitObj.gameObject.SetActive(false);
            //Debug.Log("Current exp: "+ lectureProgressBarData.currentEXP + " + Max: " + lectureProgressBarData.maxEXP);
            filledBar.fillAmount = (float)lectureProgressBarData.currentEXP / lectureProgressBarData.maxEXP;
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
            //Generate RewardButton according to score
            int rewardIndex = 0;
            rewardButton.gameObject.SetActive(false);
            foreach (LectureChechpoint checkPoint in lectureProgressBarData.checkpoints)
            {
                LectureRewardButtonObject newRewardButton = Instantiate(rewardButton, filledBar.transform);
                newRewardButton.gameObject.SetActive(true);
                RewardData newEventReward = lectureProgressBarData.allRewardData[rewardIndex].GetRewardData;
                newRewardButton.SetRewardDetail(newEventReward.RewardName, newEventReward.RewardIcon);
                /////////////////////////////////////////////// Need Listener
                newRewardButton.GetButtonComponent.onClick.AddListener(GetReward);
                ///////////////////////////////////////////////
                if (lectureProgressBarData.alreadyReceivedRewardData.Contains(lectureProgressBarData.allRewardData[rewardIndex]))
                {
                    newRewardButton.GetButtonComponent.interactable = false;
                }
                
                newRewardButton.GetButtonComponent.interactable = false;
                double iconXPos = ((float)checkPoint.RequirePoint * 360.0f) / 1000.0f;
                newRewardButton.gameObject.transform.localPosition = new Vector2((float)iconXPos-180f, 6.75f);

                rewardButtonList.Add(newRewardButton);
                rewardIndex++;
            }
            Debug.Log(lectureProgressBarData.alreadyReceivedRewardData.Count);

            introDirector.Play();
            yield return new WaitForSeconds(1.0f);

            //Play RewardButton;
            int reachScoreIndex = 0;
            while(lectureProgressBarData.checkpoints[reachScoreIndex].RequirePoint < lectureProgressBarData.currentEXP)
            {
                reachScoreIndex++;
            }
            float animationTime = Time.deltaTime * 0.001f;
            Debug.Log($"Cur exp: {lectureProgressBarData.currentEXP}/{lectureProgressBarData.maxEXP} + Gain: " +
                $"{lectureProgressBarData.gainedEXP}(+{lectureProgressBarData.bonusEXP}) = " +
                $"{lectureProgressBarData.currentEXP + lectureProgressBarData.gainedEXP}/{lectureProgressBarData.maxEXP}");

            float finalScore = (float)(lectureProgressBarData.gainedEXP + lectureProgressBarData.currentEXP) / lectureProgressBarData.maxEXP;
            float scoreWithoutBonus = (float)((lectureProgressBarData.gainedEXP + lectureProgressBarData.currentEXP) - lectureProgressBarData.bonusEXP) / lectureProgressBarData.maxEXP;
            
            int playTraitObjAnimIndex = 0;
            if (finalScore >= 1.0f)
            {
                finalScore = 1.0f;
            }
            Debug.Log(scoreWithoutBonus);
            while (filledBar.fillAmount < finalScore)
            {
                if (IsSettingUpSucessfully)
                {
                    filledBar.fillAmount = finalScore;
                    animationTime = 0.001f;
                }
                filledBar.fillAmount += 0.001f;
                //Bonus Animation
                if(filledBar.fillAmount >= scoreWithoutBonus && reachBonusTraitObjectList.Count >= 0)
                {
                    float bonusValEachTrait = (float)(lectureProgressBarData.bonusEXP / reachBonusTraitObjectList.Count) / lectureProgressBarData.maxEXP;
                    if (filledBar.fillAmount < (scoreWithoutBonus + (bonusValEachTrait * playTraitObjAnimIndex)))
                    {
                        yield return reachBonusTraitObjectList[playTraitObjAnimIndex - 1].StartPlayAnimation();
                    }
                    else
                    {
                        foreach (BonusTraitObject bonusText in reachBonusTraitObjectList)
                        {
                            bonusText.StopAnimation();
                        }
                        if (playTraitObjAnimIndex < reachBonusTraitObjectList.Count)
                            playTraitObjAnimIndex++;
                    }
                    
                }
                //Reach Reward Icon Animation
                while (filledBar.fillAmount >= lectureProgressBarData.checkpoints[reachScoreIndex].RequirePoint / 1000.0f)
                {
                    rewardButtonList[reachScoreIndex].TriggerAnimation();
                    rewardButtonList[reachScoreIndex].GetButtonComponent.interactable = true;
                    lectureProgressBarData.checkpoints[reachScoreIndex].ReceiveReward();
                    
                    obtainedRewardList.Add(lectureProgressBarData.checkpoints[reachScoreIndex]);
                    obtainedRewardIcon.Add(rewardButtonList[reachScoreIndex].GetRewardIconSprite);
                    obtainedRewardName.Add(rewardButtonList[reachScoreIndex].GetRewardNameString);
                    if (reachScoreIndex < lectureProgressBarData.checkpoints.Count - 1)
                    {
                        reachScoreIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
                yield return new WaitForSeconds(animationTime); 
            }
            foreach (BonusTraitObject bonusText in reachBonusTraitObjectList)
            {
                bonusText.StopAnimation();
            }
            _uiAnimationDone = true;
            if(obtainedRewardList.Count > 0)
            {
                StartCoroutine(DisplayPopUpRewardPanel(obtainedRewardList));
            }
        }

        private void GetReward()
        {
            Debug.Log("Get Reward");
        }

        private IEnumerator DisplayPopUpRewardPanel(List<LectureChechpoint> obtainedRewardList)
        {
            for (int i = 0; i < obtainedRewardList.Count; i++)
            {
                popUpRewardPanel.SetActive(true);
                popUpRewardIcon.sprite = obtainedRewardIcon[i];
                popUpRewardInformText.text = obtainedRewardName[i];
                while (popUpRewardPanel.activeSelf)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
            obtainedRewardList.Clear();
            yield return null;
        }
    }
}
