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
        [SerializeField]
        private Trait.Trait_Type _rewardTrait ;

        [SerializeField]
        private int _rewardAmount;
        public int RewardAmount
        {
            get { return _rewardAmount; }
            set { _rewardAmount = value; }
        }
        public Trait.Trait_Type RewardTraitType
        {
            get { return _rewardTrait; }
            set { _rewardTrait = value; }
        }
    }

    [Serializable]
    public class PersonalityTraitRewardDisplayer : BaseRewardDisplayer<PersonalityRewardData, PersonalityRewardDisplayerPanel>
    {
        private List<PersonalityRewardData> _personalityRewardDatas;
        public override IEnumerator DisplayRewardUICoroutine(List<PersonalityRewardData> rewardType)
        {
            _personalityRewardDatas = rewardType;
            yield return CreateRewardDisplayPanel() ;  
        }

        public override IEnumerator SettingUpRewardDisplayPanel(PersonalityRewardDisplayerPanel personalityRewardDisplayerPanel)
        {
            personalityRewardDisplayerPanel.SetPersonalDataReceive(_personalityRewardDatas);
            yield return (personalityRewardDisplayerPanel.SettingUpNumber() ) ; 
        }
    }
}
