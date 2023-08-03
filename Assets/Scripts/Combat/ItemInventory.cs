
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class ItemInventory : MonoBehaviour
    {
        public static ItemInventory instance = null;

        [Serializable]
        public struct ItemInventoryData
        {
            [SerializeField]
            public ItemAbilityFactorySO itemData;
            [SerializeField]
            public int amount;
        }

        [SerializeField]
        List<ItemInventoryData> _itemInventory;

        private List<ItemAbilityFactorySO> _itemInventoryAbility;
        private List<int> _itemInventoryAmount;

        #region GETTER

        public List<ItemAbilityFactorySO> GetItemInventoryAbility => _itemInventoryAbility;
        public List<int> GetItemInventoryAmount => _itemInventoryAmount;
        public List<ItemInventoryData> GetItemInventory()
        {
            return _itemInventory;
        }

        #endregion

        private void Awake()
        {
            instance = this;
            _itemInventoryAbility = new List<ItemAbilityFactorySO>();
            _itemInventoryAmount = new List<int>();
            foreach (ItemInventoryData item in _itemInventory)
            {
                _itemInventoryAbility.Add(item.itemData);
                _itemInventoryAmount.Add(item.amount);
            }
        }
    }
}
