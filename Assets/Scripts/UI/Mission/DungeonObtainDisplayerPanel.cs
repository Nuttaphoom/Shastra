using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Vanaring
{
    public class DungeonObtainDisplayerPanel : BaseRewardDisplayerPanel
    {
        // Start is called before the first frame update
        [SerializeField] private RewardIconObjectGUI guiTemplate;
        [SerializeField] private GameObject hrz;
        [SerializeField] private GameObject gfx;
        [SerializeField] private PlayableDirector introDirector;
        [SerializeField] private List<IRewardable> allRewardList = new List<IRewardable>();
        [SerializeField] private Button nextButton;

        private Dictionary<string, RewardIconObjectGUI> rewardObjectDictionary = new Dictionary<string, RewardIconObjectGUI>();


        public void SetupData(List<IRewardable> allRewardList)
        {
            this.allRewardList = allRewardList;
        }

        public override IEnumerator SettingUpNumber()
        {
            //nextButton.onClick.AddListener(() => Destroy(gameObject));
            gfx.SetActive(true);
            yield return GetReward(allRewardList);
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

        private IEnumerator GetReward(List<IRewardable> rewardList)
        {
            introDirector.Play();
            while (introDirector.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
            introDirector.Stop();

            foreach (IRewardable reward in rewardList)
            {
                bool isDuplicate = false;

                // Check for duplicates based on RewardName
                foreach (var existingReward in rewardObjectDictionary.Keys)
                {
                    if (existingReward == reward.GetRewardData().RewardName)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if (isDuplicate)
                {
                    // If the reward is already in the dictionary, call AddAmount
                    rewardObjectDictionary[reward.GetRewardData().RewardName].AddAmount();
                }
                else
                {
                    RewardIconObjectGUI newIcon = Instantiate(guiTemplate, hrz.transform);
                    newIcon.gameObject.name = reward.GetRewardData().RewardName;
                    newIcon.gameObject.SetActive(true);
                    newIcon.Init(reward);
                    rewardObjectDictionary.Add(reward.GetRewardData().RewardName, newIcon);
                    yield return new WaitForSeconds(0.1f);
                }

                
            }

            guiTemplate.gameObject.SetActive(false);
            yield return null;
        }
    }
}
