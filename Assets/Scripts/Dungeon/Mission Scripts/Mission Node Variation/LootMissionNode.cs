using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

 
 

namespace Vanaring
{
    public class LootMissionNode : BaseMissionNode
    {
  

        [SerializeField]
        private List<EventRewardData> _lootRewardDatas ;

        protected override IEnumerator OnVisiteThisNodeFirstTimeOnMission()
        {
            yield return base.OnVisiteThisNodeFirstTimeOnMission();
            yield return new WaitForSeconds(.75f);

            List<IRewardable> rewards = new List<IRewardable>() ; //= _lootRewardData.GetReward();
            foreach (var rewardData in _lootRewardDatas)
            {
                rewards.Add(rewardData.GetReward());
            }
            yield return  FindObjectOfType<MissionRewardObtainHandler>().ObtainReward(rewards) ;
            
        }
    }

     
}
