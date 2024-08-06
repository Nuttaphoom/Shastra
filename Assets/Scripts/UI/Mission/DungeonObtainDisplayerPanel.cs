using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public struct RewardSocketData
    {
        public Sprite rewardImage;
        public int amount;
    }
    public class DungeonObtainDisplayerPanel : BaseRewardDisplayerPanel
    {
        [SerializeField] private GameObject gfx;
        [SerializeField] private GameObject socketGFX;
        [SerializeField] private GameObject glow;
        [SerializeField] private Button nextButton;
        [SerializeField] private List<IRewardable> allRewardList = new List<IRewardable>();
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardName;
        [SerializeField] private TextMeshProUGUI rewardAmount;
        [SerializeField] private Animator animator;

        private Dictionary<string, RewardSocketData> rewardObjectDictionary = new Dictionary<string, RewardSocketData>();

        private Coroutine rewardCorotine;

        private bool skipRequested = false;


        public void SetupData(List<IRewardable> allRewardList)
        {
            this.allRewardList = allRewardList;
            rewardCorotine = null;
        }

        public override IEnumerator SettingUpNumber()
        {
            //nextButton.onClick.AddListener(() => Destroy(gameObject));
            gfx.SetActive(true);
            yield return SetUpReward(allRewardList);
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

        private IEnumerator SetUpReward(List<IRewardable> rewardList)
        {
            foreach (IRewardable reward in rewardList)
            {
                bool isDuplicate = false;

                foreach (var existingReward in rewardObjectDictionary.Keys)
                {
                    if (existingReward == reward.GetRewardData().RewardName)
                    {
                        isDuplicate = true;

                        int newAmount = rewardObjectDictionary[reward.GetRewardData().RewardName].amount + 1;
                        rewardObjectDictionary[reward.GetRewardData().RewardName] = 
                            new RewardSocketData { rewardImage = reward.GetRewardData().RewardIcon, amount = newAmount };
                        break;
                    }
                }

                if(!isDuplicate)
                {
                    rewardObjectDictionary.Add(reward.GetRewardData().RewardName, 
                        new RewardSocketData { rewardImage = reward.GetRewardData().RewardIcon, amount = 1 });
                }
            }

            foreach (KeyValuePair<string, RewardSocketData> rewardData in rewardObjectDictionary)
            {
                rewardCorotine = StartCoroutine(PlayMissionRewardPopup(rewardData.Key, rewardData.Value));
                yield return rewardCorotine;
            }
            socketGFX.SetActive(false);
            glow.SetActive(false);
            gfx.SetActive(false);
            _uiAnimationDone = true;
            yield return null;
        }

        private IEnumerator PlayMissionRewardPopup(string name, RewardSocketData data)
        {
            skipRequested = false;

            socketGFX.SetActive(true);
            glow.SetActive(true);
            nextButton.Select();


            animator.Play("NodeRewardFadeUp");
            rewardImage.sprite = data.rewardImage;
            rewardName.text = name;
            rewardAmount.text = "x" + data.amount.ToString();

            float waitTime = 1.5f;
            float elapsedTime = 0.0f;

            while (elapsedTime < waitTime)
            {
                if (skipRequested)
                {
                    yield break;
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            

            //yield return new WaitForSeconds(1.5f);

        }

        public void OnSkipButtonClicked()
        {
            skipRequested = true;
        }
    }
}
