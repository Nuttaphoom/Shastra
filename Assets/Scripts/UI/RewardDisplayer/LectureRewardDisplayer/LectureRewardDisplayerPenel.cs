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
        private Button rewardIcon;
        [SerializeField]
        private LectureManager lectureMananger;

        private List<Button> rewardButtonList = new List<Button>();
        private LectureRewardStruct lectureProgressBarData;

        public void ReceiveRewardDetail(LectureRewardStruct rewardData)
        {
            lectureProgressBarData = rewardData;
            Debug.Log("fill: "+ lectureProgressBarData.currentEXP + " + " + lectureProgressBarData.maxEXP);
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
            rewardIcon.gameObject.SetActive(false);
            foreach (LectureChechpoint checkPoint in lectureProgressBarData.checkpoints)
            {
                Button newRewardButton = Instantiate(rewardIcon, filledBar.transform);
                newRewardButton.gameObject.SetActive(true);
                RewardData newEventReward = lectureProgressBarData.allRewardData[rewardIndex].GetRewardData;
                newRewardButton.GetComponent<ButtonHover>().InitWindow(newEventReward.RewardName, newEventReward.RewardIcon);
                /////////////////////////////////////////////// Need Listener
                newRewardButton.onClick.AddListener(GetReward);
                ///////////////////////////////////////////////
                Debug.Log(lectureProgressBarData.alreadyReceivedRewardData.Count);
                if (lectureProgressBarData.alreadyReceivedRewardData.Contains(lectureProgressBarData.allRewardData[rewardIndex]))
                {
                    newRewardButton.interactable = false;
                }
                newRewardButton.interactable = false;
                double iconXPos = ((float)checkPoint.RequirePoint * 360.0f) / 1000.0f;
                newRewardButton.GetComponent<Image>().rectTransform.localPosition = new Vector3((float)iconXPos-200f, -24.0f, 0f);

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
            float animationTime = Time.deltaTime / 2000.0f;
            while (filledBar.fillAmount < (float)((float)(lectureProgressBarData.gainedEXP + lectureProgressBarData.currentEXP) / lectureProgressBarData.maxEXP))
            {
                if (IsSettingUpSucessfully)
                {
                    animationTime = Time.deltaTime;
                }
                filledBar.fillAmount += 0.01f;
                if (filledBar.fillAmount >= lectureProgressBarData.checkpoints[reachScoreIndex].RequirePoint / 1000.0f)
                {
                    rewardButtonList[reachScoreIndex].gameObject.GetComponent<Animator>().enabled = true;
                    rewardButtonList[reachScoreIndex].interactable = true;
                    Debug.Log("Reach Reward");
                    if (reachScoreIndex < lectureProgressBarData.checkpoints.Count - 1)
                    {
                        reachScoreIndex++;
                    }
                }
                yield return new WaitForSeconds(animationTime); 
            }
        }

        private void GetReward()
        {
            Debug.Log("Get Reward");
        }
    }
}
