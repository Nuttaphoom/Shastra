using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    /// <summary>
    /// This CashSO is reused throughtout life time of application 
    /// </summary>
    /// 
    [Serializable] 
    [CreateAssetMenu(fileName = "CashRewardableSO", menuName = "ScriptableObject/CustomReward/Cash")]
    public class CashRewardableSO : ScriptableObject,IRewardable
    {
        [SerializeField]
        private RewardData _rewardData;

        [SerializeField] 
        private int _amount = -1 ; 


        public RewardData GetRewardData()
        {
            return _rewardData; 
        }

        public void SetRewardAmount(int amount)
        {
            _amount = amount ;  
        }

        public void SubmitReward()
        {
            if (_amount == -1   )
                throw new Exception("Cash Reward hasn't been properly set up");

            PersistentPlayerPersonalDataManager.Instance.GetBackpack.ModifyCash(_amount) ;

            _amount = -1; 



        }

     }
}
