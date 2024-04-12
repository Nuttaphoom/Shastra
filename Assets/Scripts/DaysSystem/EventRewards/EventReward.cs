using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity;
using UnityEngine;
using JetBrains.Annotations;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
namespace Vanaring
{
    public abstract class EventReward
    {
        public abstract IRewardable GetEventRewards() ;

        public abstract RewardData GetRewardData();

    }

    [Serializable]
    public class EventRewardData
    {
        [SerializeField]
        private bool _rewardIsItem = false;

        [SerializeField]
        private bool _rewardIsSpell = false;

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("RewardIsSpell")]
        private EventReward<SpellActionSO> _spellReward;

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("RewardIsItem")]
        private EventReward<BackpackItemSO> _itemReward;

        #region Getter 
        public bool RewardIsItem => _rewardIsItem;
        public bool RewardIsSpell => _rewardIsSpell;

        public EventReward<SpellActionSO> SpellReward
        {
            get
            {
                return _spellReward; 
            }
        }

        public EventReward<BackpackItemSO> ItemReward
        {
            get
            {
                return _itemReward ; 
            }
        }

        public IRewardable GetReward()
        {
            if (RewardIsItem)
            {
                return _itemReward.GetEventRewards() ; 
            }else if (RewardIsSpell)
            {
                return _spellReward.GetEventRewards() ;
            }

            throw new Exception("Reward hasn't been properly assigned"); 
            

        }

        #endregion
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
        public string RewardDescription;
        public Sprite RewardIcon;
    }
}