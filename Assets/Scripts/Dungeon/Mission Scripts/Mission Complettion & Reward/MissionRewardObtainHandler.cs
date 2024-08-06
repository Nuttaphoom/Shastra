using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Vanaring 
{
    public class MissionRewardObtainHandler : MonoBehaviour
    {
        [SerializeField]
        private DungeonObtainDisplayer _dungeonObtainDisplayer;  

        public  IEnumerator ObtainReward(List<IRewardable> rewards)
        {
            yield return _dungeonObtainDisplayer.DisplayRewardUICoroutine(rewards); 

            foreach (var reward in rewards)
            {
                reward.SubmitReward();

                ColorfulLogger.LogWithColor("Submit reward " + reward.GetRewardData().RewardName, Color.yellow);

            }

            FindAnyObjectByType<QuickMissionBackpack>().UpdateItemAmount();
        }
    }
}
