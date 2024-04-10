using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{

    public class CombatRewardDisplayer : BaseRewardDisplayer<CombatRewardManager.CombatRewardData, CombatRewardDisplayerPanel>
    {
        private CombatRewardManager.CombatRewardData _rewardData ; 

        public override IEnumerator DisplayRewardUICoroutine(CombatRewardManager.CombatRewardData rewardData)
        {
            _rewardData = rewardData;
            yield return CreateRewardDisplayPanel();
        }

        protected override IEnumerator SettingUpRewardDisplayPanel(CombatRewardDisplayerPanel combatRewardPanel)
        {
            combatRewardPanel.SetUpReward(_rewardData) ;
            yield return (combatRewardPanel.SettingUpNumber());
        }
        
    }
}
