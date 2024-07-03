using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    [Serializable]
    public class BackpackItemData
    {
        public BackpackItemSO BackpackItem;

        public int Amount;
    }

    [Serializable]
    public class Backpack 
    {
        [Header("TODO : make this non-SerializeField later")]
        [Tooltip("manually assign BackpackItem  for testing")]
        [SerializeField]
        private List<BackpackItemData> _backpackItemSO   ;

        private InventoryDatabaseSO m_inventoryDatabase;

        private int _currentCash = 0 ;
        #region GETTER
        public List<BackpackItemData> GetCombatUseableItemSOs()
        {
            List<BackpackItemData> ret = new List<BackpackItemData>();
            foreach (var backpackItem in _backpackItemSO)
            {
                ret.Add(backpackItem);

                if ((backpackItem.BackpackItem) is not CombatUseableItemSO)
                {
                    ret.RemoveAt(ret.Count - 1);
                }
            }
            return _backpackItemSO;
        }

        public int GetCurrentCash
        {
            get
            {
                return _currentCash;
            }
        }
        public List<BackpackItemData> GetAllItemsInBackpack 
        {
            get {
                return _backpackItemSO; 
            }
        }
        #endregion
        
        #region Cash Mod Methods 
        public void ModifyCash(int modAmount)
        {
            if (modAmount < 0 && _currentCash < modAmount)
                throw new Exception("Current Cash can't be negative ! modAmount = " + modAmount + "  current cash is " + _currentCash);

            _currentCash += modAmount;

            Debug.Log("Current Cash  : " + _currentCash); 
        }

        #endregion

        #region Item Mod Methods 

        public void AddItemIntoBackpack(BackpackItemSO itemSO, int amount)
        {
            Debug.Log("add new item into backpacks"); 

            if (_backpackItemSO == null)
                _backpackItemSO = new List<BackpackItemData>();

            for (int i = 0; i < _backpackItemSO.Count; i++)
            {
                if (_backpackItemSO[i].BackpackItem.GetDescriptionBaseField().FieldName == itemSO.GetDescriptionBaseField().FieldName)
                {
                    // Update the item at index i
                    _backpackItemSO[i].Amount += (amount);
                    return;
                }
            }
            BackpackItemData backpackItemData = new BackpackItemData();
            backpackItemData.BackpackItem = (itemSO);
            backpackItemData.Amount += (amount);
            _backpackItemSO.Add(backpackItemData);
        }


        // this function call when update item from temporary save to restore the different item data
        public void UpdateItemInBackpack(BackpackItemSO itemSO, int amount)
        {
            for (int i = 0; i < _backpackItemSO.Count; i++)
            {
                if (_backpackItemSO[i].BackpackItem.GetDescriptionBaseField().FieldName == itemSO.GetDescriptionBaseField().FieldName)
                {
                    if (_backpackItemSO[i].Amount != amount)
                    {
                        _backpackItemSO[i].Amount = amount;
                    }
                    return;
                }
            }
            BackpackItemData backpackItemData = new BackpackItemData();
            backpackItemData.BackpackItem = (itemSO);
            backpackItemData.Amount = (amount);
            _backpackItemSO.Add(backpackItemData);
        }
        public void RemoveItemFromBackpack(BackpackItemSO itemSO, int amount)
        {
            for (int i = 0; i < _backpackItemSO.Count; i++)
            {
                if (_backpackItemSO[i].BackpackItem.GetDescriptionBaseField().FieldName == itemSO.GetDescriptionBaseField().FieldName)
                {
                    // Update the item at index i
                    _backpackItemSO[i].Amount += (-(int)MathF.Abs(amount));// += amount;
                    if (_backpackItemSO[i].Amount <= 0)
                        _backpackItemSO.RemoveAt(i);

                    return;
                }
            }
        }

        #endregion
        private void LoadItemDatabaseOP()
        {
            if (m_inventoryDatabase != null)
                return; 

            m_inventoryDatabase = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<InventoryDatabaseSO>(DatabaseAddressLocator.GetInventoryDatabaseAddress);
        }
       
        #region Save System

        public BackpackSaveData CaptureBackpackState()
        {
            if (m_inventoryDatabase == null)
            {
                LoadItemDatabaseOP();
            }
            
            List<string> keys = new List<string>();
            foreach (BackpackItemData backpackItem in _backpackItemSO)
            {
                for (int i = 0; i < backpackItem.Amount ; i++)
                {
                    var key = m_inventoryDatabase.GetRecordKey(backpackItem.BackpackItem);
                    keys.Add(key);
                    Debug.Log("capture key " + key + "with item " + backpackItem.BackpackItem);
                    Debug.Log("backpackItem.Amount : " + backpackItem.Amount);

                }
            }
            BackpackSaveData ret = new BackpackSaveData()
            {
                SavedItemUniqueID = keys,
                SavedCash = _currentCash
            };
            return ret ;
        }

        public void RestoreBackpackState(BackpackSaveData state)
        {
            BackpackSaveData saveData = (BackpackSaveData)state;

            if(_backpackItemSO != null)
            {
                RestoreBackpackItem(saveData.SavedItemUniqueID);
            }

            _currentCash = saveData.SavedCash;
        }
        private void RestoreBackpackItem(List<string> uniqueID)
        {
            // temporary data holder for update the backpack item
            Dictionary<BackpackItemSO, int> currentBackpackItem = new Dictionary<BackpackItemSO, int>();

            for (int i = 0; i < uniqueID.Count; i++)
            {
                BackpackItemSO item = m_inventoryDatabase.GetRecord(uniqueID[i]);
                Debug.Log("add " + item + "with uniqueID : " + uniqueID[i]) ;
                // already contains key add amount instead
                if (currentBackpackItem.ContainsKey(item))
                {
                    currentBackpackItem[item] += 1;
                    continue;
                }
                else
                {
                    // add unique
                    currentBackpackItem.Add(item, 1);
                }
            }

            foreach (var data in currentBackpackItem)
            {
                UpdateItemInBackpack(data.Key, data.Value);
            }
        }

        public struct BackpackSaveData
        {
            /// <summary>
            /// Duplicated Item (amount > 1) will be saved in here = number of items on that time
            /// </summary>
            public List<string> SavedItemUniqueID;

            public int SavedCash;
        }
        #endregion

    }
}
