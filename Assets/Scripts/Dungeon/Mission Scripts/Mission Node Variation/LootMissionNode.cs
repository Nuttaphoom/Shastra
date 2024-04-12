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
        private EventRewardData _lootRewardData ;

        protected override IEnumerator OnVisiteThisNodeFirstTimeOnMission()
        {
            yield return base.OnVisiteThisNodeFirstTimeOnMission();
            yield return new WaitForSeconds(.75f);

            IRewardable rewards = _lootRewardData.GetReward();
            rewards.SubmitReward();
            ColorfulLogger.LogWithColor("Submit reward " + rewards.GetRewardData().RewardName, Color.yellow); 

        }
    }

     
}
