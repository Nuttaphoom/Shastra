using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    [Serializable]
    public struct PersonalityRewardData
    {
        private int rewardTrait1;
        private int rewardTrait2;
        private int rewardTrait3;
        private int rewardTrait4;
        public int RewardTrait1 => rewardTrait1;
        public int RewardTrait2 => rewardTrait2;
        public int RewardTrait3 => rewardTrait3;
        public int RewardTrait4 => rewardTrait4;

    }
    [Serializable]
    public class PersonalityTraitRewardDisplayer : BaseRewardDisplayer<PersonalityRewardData, PersonalityRewardDisplayerPanel>
    {
        private PersonalityRewardData _prd;
        public override IEnumerator DisplayRewardUICoroutine(PersonalityRewardData rewardType_DONTUSETHISONE_ORYOUAREGAY)
        {
            _prd = rewardType_DONTUSETHISONE_ORYOUAREGAY;
            yield return CreateRewardDisplayPanel() ;  
        }

        public override IEnumerator SettingUpRewardDisplayPanel(PersonalityRewardDisplayerPanel personalityRewardDisplayerPanel)
        {
            personalityRewardDisplayerPanel.SetPersonalDataReceive(_prd);
            yield return (personalityRewardDisplayerPanel.SettingUpNumber() ) ; 
        }
    }
}
