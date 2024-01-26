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
    }

    [Serializable]
    public class PersonalityTraitRewardDisplayer : BaseRewardDisplayer<PersonalityRewardData, PersonalityRewardDisplayerPanel>
    {
        private List<PersonalityRewardData> _prd;
        public override IEnumerator DisplayRewardUICoroutine(List<PersonalityRewardData> rewardType_DONTUSETHISONE_ORYOUAREGAY)
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
