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
        private bool RewardIsItem = false;

        [SerializeField]
        private bool RewardIsSpell = false;

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("RewardIsSpell")]
        private EventReward<SpellActionSO> cp_spellReward ;

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("RewardIsItem")]
        private EventReward<BackpackItemSO> cp_itemReward;

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
                if (RewardIsSpell)
                {
                    return cp_spellReward; 
                }else if (RewardIsItem)
                {
                    return cp_itemReward; 
                }else
                {
                    throw new Exception("No reward has been assigned at requirePoint : " + cp_requirepoint); 
                }
            }
        } 
        public bool Received => cp_received;
    }
}
