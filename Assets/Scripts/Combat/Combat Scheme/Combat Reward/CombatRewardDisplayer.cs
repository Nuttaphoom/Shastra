    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vanaring.CombatRewardManager;

namespace Vanaring
{

    public class CombatRewardDisplayer : BaseRewardDisplayer<CombatRewardData, CombatRewardDisplayerPanel>
    {
        private CombatRewardData _rewardData ; 

        public override IEnumerator DisplayRewardUICoroutine(CombatRewardData rewardData)
        {
            //_rewardData = rewardData;
            //yield return CreateRewardDisplayPanel();

            yield return null;

        }

        protected override IEnumerator SettingUpRewardDisplayPanel(CombatRewardDisplayerPanel combatRewardPanel)
        {
            combatRewardPanel.SetUpReward() ;
            yield return (combatRewardPanel.SettingUpNumber());
        }
        
    }
}
