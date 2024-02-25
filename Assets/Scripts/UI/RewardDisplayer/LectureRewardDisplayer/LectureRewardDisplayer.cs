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
        //All of the EXP gained from every source
        public int gainedEXP;
        //EXP gained from the bonus booster
        public int bonusEXP;
        public List<LectureChechpoint> checkpoints ; 
        public List<EventReward> allRewardData;
        public List<EventReward> alreadyReceivedRewardData;
        public List<LectureRequieBoost> boostLecture;
        
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

        protected override IEnumerator SettingUpRewardDisplayPanel(LectureRewardDisplayerPenel personalityRewardDisplayerPanel)
        {
            personalityRewardDisplayerPanel.ReceiveRewardDetail(_lectureRewardStruct);
            yield return (personalityRewardDisplayerPanel.SettingUpNumber());
        }
    }
}
