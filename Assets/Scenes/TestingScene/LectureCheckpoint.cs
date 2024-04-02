using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

namespace Vanaring
{
    [Serializable]
    public class LectureChechpoint
    {
        [SerializeField]
        private int cp_requirepoint;

        [SerializeField]
        private EventRewardData _eventRewardData; 

        [NonSerialized]
        private bool cp_received;

        public void ReceiveReward() { 
            cp_received = true ;
            Reward.GetEventRewards().SubmitReward(); 
        }
        
        public int RequirePoint => cp_requirepoint;
        public EventReward Reward {
            get
            {
                if (_eventRewardData.RewardIsSpell)
                {
                    return _eventRewardData.SpellReward; 
                }else if (_eventRewardData.RewardIsItem)
                {
                    return _eventRewardData.ItemReward; 
                }else
                {
                    throw new Exception("No reward has been assigned at requirePoint : " + cp_requirepoint); 
                }
            }
        } 
        public bool Received => cp_received;
    }
}
