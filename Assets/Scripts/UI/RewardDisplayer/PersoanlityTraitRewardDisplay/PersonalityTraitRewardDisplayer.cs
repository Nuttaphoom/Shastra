using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    public struct PersoanlityRewardData
    {

    }
    [Serializable]
    public class PersonalityTraitRewardDisplayer : BaseRewardDisplayer<PersoanlityRewardData, PersonalityRewardDisplayerPanel>
    {
        public override IEnumerator DisplayRewardUICoroutine()
        {

            yield return CreateRewardDisplayPanel() ;  
        }

        public override IEnumerator SettingUpRewardDisplayPanel(PersonalityRewardDisplayerPanel personalityRewardDisplayerPanel)
        {
            yield return (personalityRewardDisplayerPanel.SettingUpNumber() ) ; 
        }
    }
}
