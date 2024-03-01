using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity;
using UnityEngine;
namespace Vanaring
{
    public abstract class EventReward
    {
        public abstract IRewardable GetEventRewards() ;

        public abstract RewardData GetRewardData();

    }


    [System.Serializable]
    public class EventReward<RewardType> : EventReward  where RewardType: ScriptableObject
    {
        [SerializeField]
        private RewardType _rewards;

        public override IRewardable GetEventRewards()
        {
             
                if (_rewards == null)
                    throw new System.Exception("Reward hasn't never been assigned");

                if ((_rewards as IRewardable) == null)
                    throw new System.Exception("Reward is not IRewardable");

                return _rewards as IRewardable;
             
        }

        public override RewardData GetRewardData()
        {
            return GetEventRewards().GetRewardData();
        }

     
        
    }

    public interface IRewardable
    {
        RewardData GetRewardData();
        void SubmitReward();
    }

    public struct RewardData
    {
        public string RewardName;
        public Sprite RewardIcon;
    }
}