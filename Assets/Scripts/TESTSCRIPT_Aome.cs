using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Vanaring
{
    public class TESTSCRIPT_Aome : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private RewardIconObjectGUI guiTemplate;
        [SerializeField] private GameObject hrz;
        [SerializeField] private GameObject gfx;
        [SerializeField] private PlayableDirector introDirector;
        [SerializeField] private List<EventReward<SpellActionSO>> testList = new List<EventReward<SpellActionSO>>();
        [SerializeField] private Button nextButton;

        private Dictionary<string, RewardIconObjectGUI> rewardObjectDictionary = new Dictionary<string, RewardIconObjectGUI>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(GetReward(testList));
                nextButton.onClick.AddListener(() => Destroy(gameObject));
                gfx.SetActive(true);
                
            }
        }

        private IEnumerator GetReward(List<EventReward<SpellActionSO>> rewardList)
        {
            introDirector.Play();
            while (introDirector.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
            introDirector.Stop();

            foreach (EventReward<SpellActionSO> reward in rewardList)
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
                    Debug.Log("Found");
                    rewardObjectDictionary[reward.GetRewardData().RewardName].AddAmount();
                }
                else
                {
                    Debug.Log(reward.GetHashCode());
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
