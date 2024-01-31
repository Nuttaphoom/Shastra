using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Unity.Mathematics;
using System;

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

        [SerializeField]
        private float[] rewardScore;
        private List<Button> rewardIconList = new List<Button>();
        private List<EventReward> lectureRewardList = new List<EventReward>();
        private List<EventReward> obtainedLectureRewardList = new List<EventReward>();
        private LectureRewardStruct lectureRewardData;

        private void Start()
        {

        }

        public void ReceiveRewardDetail(LectureRewardStruct rewardData)
        {
            lectureRewardData = rewardData;
            filledBar.fillAmount = lectureRewardData.currentEXP/ lectureRewardData.maxEXP;
            lectureRewardList = lectureRewardData.allRewardData;
            Sprite rewardIcon = lectureRewardList[0].GetRewardData.RewardIcon;
            string rewardName = lectureRewardList[0].GetRewardData.RewardName;
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
            foreach (EventReward reward in lectureRewardList)
            {
                Button newRewardButton = Instantiate(rewardIcon, filledBar.transform);
                //
                
                newRewardButton.gameObject.SetActive(true);
                //float scale = (score / 100.0f) * 360.0f;
                //double scaled = Math.Floor(scale - 200);
                string rewardName = reward.GetRewardData.RewardName;
                newRewardButton.GetComponent<Image>().sprite = reward.GetRewardData.RewardIcon;
                //newRewardButton.GetComponent<Image>().rectTransform.localPosition = new Vector3((float)scaled, -24.0f, 0f);
                rewardIconList.Add(newRewardButton);
                //Debug.Log(scale);
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
            while (filledBar.fillAmount < 1)
            {
                filledBar.fillAmount += 1 * Time.deltaTime;
                if ((filledBar.fillAmount * 100) >= rewardScore[reachScoreIndex])
                {
                    rewardIconList[reachScoreIndex].gameObject.GetComponent<Animator>().enabled = true;
                    //Debug.Log("Reach Reward");
                    if (reachScoreIndex < rewardScore.Length - 1)
                    {
                        reachScoreIndex++;
                    }
                }
                yield return new WaitForSeconds(0.01f); 
            }
        }
    }
}
