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
        public List<LectureChechpoint> checkpoints ; 
        public List<EventReward> allRewardData;
        public List<EventReward> alreadyReceivedRewardData;
    }

    [Serializable]
    public class LectureRewardDisplayer : BaseRewardDisplayer<LectureRewardStruct, LectureRewardDisplayerPenel>
    {
        private LectureRewardStruct _lectureRewardStruct; 
        //private List<PersonalityRewardData> _personalityRewardDatas;
        public override IEnumerator DisplayRewardUICoroutine(LectureRewardStruct rewardType)
        {
            _lectureRewardStruct = rewardType;
            yield return CreateRewardDisplayPanel();
        }

        public override IEnumerator SettingUpRewardDisplayPanel(LectureRewardDisplayerPenel personalityRewardDisplayerPanel)
        {
            personalityRewardDisplayerPanel.ReceiveRewardDetail(_lectureRewardStruct);
            yield return (personalityRewardDisplayerPanel.SettingUpNumber());
        }
    }
}
