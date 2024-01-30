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
    [Serializable]
    public class EventReward 
    {
        [SerializeField]
        private ScriptableObject _rewards; 

        public IRewardable GetEventRewards
        {
            get {
                if (_rewards == null)
                    throw new System.Exception("Reward hasn't never been assigned");

                if ((_rewards as IRewardable )== null)
                    throw new System.Exception("Reward is not IRewardable"); 
                
                return _rewards as IRewardable ; 
            } 
        }

        public RewardData GetRewardData
        {
            get
            {
                return GetEventRewards.GetRewardData();
            }
        }


    }

    /// <summary>
    /// should only be attached to a ScriptableObject only
    /// </summary>
    /// <typeparam name="RewardData"></typeparam>
    public interface IRewardable 
    {
        public RewardData GetRewardData() ;
        public void SubmitReward(); 
    }

    public struct RewardData
    {
        public string RewardName;
        public Sprite RewardIcon; 
        
    }
}
