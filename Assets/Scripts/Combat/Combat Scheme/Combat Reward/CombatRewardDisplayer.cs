using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{

    public class CombatRewardDisplayer : BaseRewardDisplayer<int, CombatRewardDisplayerPanel>
    {
        private int _rewardData ; 

        //private List<PersonalityRewardData> _personalityRewardDatas;
        public override IEnumerator DisplayRewardUICoroutine(int rewardType)
        {
            _rewardData = rewardType;
            yield return CreateRewardDisplayPanel();

        }

        protected override IEnumerator SettingUpRewardDisplayPanel(CombatRewardDisplayerPanel combatRewardPanel)
        {
            combatRewardPanel.SetUpReward() ;
            yield return (combatRewardPanel.SettingUpNumber());
        }
        
    }
}
