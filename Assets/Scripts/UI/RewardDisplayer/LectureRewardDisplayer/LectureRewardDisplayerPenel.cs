using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Unity.Mathematics;
using System;
using UnityEngine.EventSystems;

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
        private LectureManager lectureMananger;

        private List<LectureRewardButtonObject> rewardButtonList = new List<LectureRewardButtonObject>();
        private LectureRewardStruct lectureProgressBarData;

        public void ReceiveRewardDetail(LectureRewardStruct rewardData)
        {
            lectureProgressBarData = rewardData;
            Debug.Log("Current exp: "+ lectureProgressBarData.currentEXP + " + Max: " + lectureProgressBarData.maxEXP);
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
                //Debug.Log(lectureProgressBarData.alreadyReceivedRewardData.Count);
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

            introDirector.Play();
            yield return new WaitForSeconds(1.0f);

            //Play RewardButton;
            int reachScoreIndex = 0;
            if(lectureProgressBarData.checkpoints[reachScoreIndex].RequirePoint < lectureProgressBarData.currentEXP)
            {
                reachScoreIndex++;
            }
            float animationTime = Time.deltaTime * 0.001f;
            Debug.Log("Gain: " + lectureProgressBarData.gainedEXP + " Cur: " + lectureProgressBarData.currentEXP);
            Debug.Log((float)(lectureProgressBarData.gainedEXP + lectureProgressBarData.currentEXP));
            Debug.Log(lectureProgressBarData.maxEXP);
            float finalScore = (float)(lectureProgressBarData.gainedEXP + lectureProgressBarData.currentEXP) / lectureProgressBarData.maxEXP;
            if(finalScore > 1.0f)
            {
                finalScore = 1.0f;
            }
            Debug.Log(finalScore);
            while (filledBar.fillAmount < finalScore)
            {
                if (IsSettingUpSucessfully)
                {
                    animationTime = Time.deltaTime;
                }
                filledBar.fillAmount += 0.001f;
                if (filledBar.fillAmount >= lectureProgressBarData.checkpoints[reachScoreIndex].RequirePoint / 1000.0f)
                {
                    rewardButtonList[reachScoreIndex].TriggerAnimation();
                    rewardButtonList[reachScoreIndex].GetButtonComponent.interactable = true;
                    //Debug.Log("Reach Reward");
                    if (reachScoreIndex < lectureProgressBarData.checkpoints.Count - 1)
                    {
                        reachScoreIndex++;
                    }
                }
                yield return new WaitForSeconds(animationTime); 
            }
            Debug.Log("Fisnish Animate");
        }

        private void GetReward()
        {
            Debug.Log("Get Reward");
        }
    }
}
