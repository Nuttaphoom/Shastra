using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [Serializable]
    public struct LectureRewardStruct
    {
        public int maxEXP;
        public int currentEXP;
        //string reward 
        public int gainedEXP;
        public List<EventReward> rewardData;
    }

    [Serializable]
    public class LectureRewardDisplayer : BaseRewardDisplayer<LectureRewardStruct, LectureRewardDisplayerPenel>
    {
        private LectureRewardStruct _lectureRewardStruct; 
        //private List<PersonalityRewardData> _personalityRewardDatas;
        public override IEnumerator DisplayRewardUICoroutine(LectureRewardStruct rewardType)
        {
            //_personalityRewardDatas = rewardType;
            yield return CreateRewardDisplayPanel();
        }

        public override IEnumerator SettingUpRewardDisplayPanel(LectureRewardDisplayerPenel personalityRewardDisplayerPanel)
        {
            personalityRewardDisplayerPanel.ReceiveRewardDetail(_lectureRewardStruct);
            yield return (personalityRewardDisplayerPanel.SettingUpNumber());
        }
    }
}
