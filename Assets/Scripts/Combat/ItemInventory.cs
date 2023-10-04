
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring 
{
    public class ItemInventory : MonoBehaviour
    {
        // TODO : Not singleton inventory
        public static ItemInventory instance = null;

        [Serializable]
        public struct ItemInventoryData
        {
            // TODO : Combine itemData & amount into the struct
            [SerializeField]
            public ItemActionFactorySO itemData;
            [SerializeField]
            public int amount;
        }

        [SerializeField]
        List<ItemInventoryData> _itemInventory;

        private List<ItemActionFactorySO> _itemInventoryAbility;
        private List<int> _itemInventoryAmount;

        #region GETTER

        public List<ItemActionFactorySO> GetItemInventoryAbility => _itemInventoryAbility; 
        
        public List<int> GetItemInventoryAmount => _itemInventoryAmount;
        public List<ItemInventoryData> GetItemInventory()
        {
            return _itemInventory;
        }

        #endregion

        private void Awake()
        {
            instance = this;
            _itemInventoryAbility = new List<ItemActionFactorySO>();
            _itemInventoryAmount = new List<int>();
            foreach (ItemInventoryData item in _itemInventory)
            {
                _itemInventoryAbility.Add(item.itemData);
                _itemInventoryAmount.Add(item.amount);
            }
        }
    }
}
