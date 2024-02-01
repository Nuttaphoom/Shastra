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
    public class LectureRewardDisplayerPenel : BaseRewardDisplayerPanel //, IPointerEnterHandler, IPointerExitHandler
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
        private List<EventReward> obtainedLectureRewardList = new List<EventReward>();
        private LectureRewardStruct lectureProgressBarData;

        private void Start()
        {

        }

        public void ReceiveRewardDetail(LectureRewardStruct rewardData)
        {
            lectureProgressBarData = rewardData;
            filledBar.fillAmount = lectureProgressBarData.currentEXP/ lectureProgressBarData.maxEXP;
            

            Sprite rewardIcon = lectureProgressBarData.allRewardData[0].GetRewardData.RewardIcon;
            string rewardName = lectureProgressBarData.allRewardData[0].GetRewardData.RewardName;
            LectureChechpoint checkpoints = lectureProgressBarData.checkpoints[0];
            //rewardData.gainedEXP;
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

        private void ReceiveEXPIncrease(int amount)
        {
            Debug.Log("Increase Amount: " + amount);
        }

        private void ReceiveRewardStringList(List<string> rewardList)
        {
            if (rewardList.Count <= 0)
            {
                return;
            }
            Debug.Log("Reward receive amount: " + rewardList.Count);
            foreach (string reward in rewardList)
            {
                Debug.Log("Reward" + reward);
            }
        }

        public override IEnumerator SettingUpNumber()
        {
            //Generate RewardButton according to score
            int rewardIndex = 0;
            foreach (LectureChechpoint checkPoint in lectureProgressBarData.checkpoints)
            {
                Button newRewardButton = Instantiate(rewardIcon, filledBar.transform);
                newRewardButton.gameObject.SetActive(true);

                double iconXPos = Math.Floor(((checkPoint.RequirePoint / 100.0f) * 360.0f) - 200);
                //newRewardButton.GetComponent<Image>().rectTransform.localPosition = new Vector3((float)iconXPos, -24.0f, 0f);

                string rewardName = lectureProgressBarData.allRewardData[rewardIndex].GetRewardData.RewardName;

                newRewardButton.GetComponent<Image>().sprite = lectureProgressBarData.allRewardData[rewardIndex].GetRewardData.RewardIcon;

                rewardButtonList.Add(newRewardButton);
                rewardIndex++;
            }
            yield return new WaitForEndOfFrame();

            introDirector.Play();
            while (introDirector.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
            rewardIcon.gameObject.SetActive(false);

            //Play RewardButton;
            int reachScoreIndex = 0;
            if(lectureProgressBarData.checkpoints[reachScoreIndex].RequirePoint < lectureProgressBarData.currentEXP)
            {
                reachScoreIndex++;
            }
            filledBar.fillAmount = lectureProgressBarData.maxEXP;
            while (filledBar.fillAmount < (lectureProgressBarData.gainedEXP + lectureProgressBarData.currentEXP) / lectureProgressBarData.maxEXP)
            {
                filledBar.fillAmount += 1 * Time.deltaTime;
                if (filledBar.fillAmount >= lectureProgressBarData.checkpoints[reachScoreIndex].RequirePoint)
                {
                    rewardButtonList[reachScoreIndex].gameObject.GetComponent<Animator>().enabled = true;
                    //Debug.Log("Reach Reward");
                    if (reachScoreIndex < lectureProgressBarData.checkpoints.Count - 1)
                    {
                        reachScoreIndex++;
                    }
                }
                yield return new WaitForSeconds(0.01f); 
            }
        }

        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    throw new NotImplementedException();
        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
