using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [System.Serializable]
    public class DungeonObtainDisplayer : BaseRewardDisplayer<List<IRewardable>, DungeonObtainDisplayerPanel>
    {
        private List<IRewardable> _rewardData = new List<IRewardable>();

        public override IEnumerator DisplayRewardUICoroutine(List<IRewardable> rewardData)
        {
            _rewardData = rewardData;
            yield return CreateRewardDisplayPanel();
        }

        protected override IEnumerator SettingUpRewardDisplayPanel(DungeonObtainDisplayerPanel dungeonRewardPanel)
        {
            dungeonRewardPanel.SetupData(_rewardData);
            yield return dungeonRewardPanel.SettingUpNumber();
        }
    }
}
