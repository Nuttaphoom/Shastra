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
        private Image rewardIcon;
        [SerializeField]
        private LectureManager lectureMananger;

        [SerializeField]
        private float[] rewardScore;
        private List<Image> rewardIconList = new List<Image>();

        private void Start()
        {
            StartCoroutine(SettingUpNumber());
            //lectureMananger.SubOnReceiveExpEvent(ReceiveEXPIncrease);
            //lectureMananger.SubOnUnlockRewardEvent(ReceiveRewardStringList);
            
        }

        public void ReceiveRewardDetail(LectureRewardStruct reward)
        {

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
            foreach (float score in rewardScore)
            {

                Image newRewardIcon = Instantiate(rewardIcon, filledBar.transform);
                newRewardIcon.gameObject.SetActive(true);
                float scale = (score / 100.0f) * 360.0f;
                double scaled = Math.Floor(scale - 200);
                newRewardIcon.rectTransform.localPosition = new Vector3((float)scaled, -24.0f, 0f);
                rewardIconList.Add(newRewardIcon);
                //Debug.Log(scale);
            }
            yield return new WaitForEndOfFrame();
            introDirector.Play();
            while (introDirector.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
            lectureMananger.IncreaseExp("GAM300");
            rewardIcon.gameObject.SetActive(false);
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
            //lectureMananger.UnSubOnReceiveExpEvent(ReceiveEXPIncrease);
            //lectureMananger.UnSubOnUnlockRewardEvent(ReceiveRewardStringList);
            //throw new System.NotImplementedException();
        }
    }
}
