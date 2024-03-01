using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public abstract class BackpackItemSO : ScriptableObject, IRewardable
    {
        public abstract DescriptionBaseField GetDescriptionBaseField();

        public RewardData GetRewardData()
        {
            return new RewardData()
            {
                RewardIcon = GetDescriptionBaseField().FieldImage,
                RewardName = GetDescriptionBaseField().FieldName
            }; 
        }

        public void SubmitReward()
        {
            PersistentPlayerPersonalDataManager.Instance.GetBackpack.AddItemIntoBackpack(this, 1); 
        }
    }
}
