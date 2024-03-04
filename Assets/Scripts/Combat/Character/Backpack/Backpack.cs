using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vanaring.Assets.Scripts.Utilities.StringConstant;

namespace Vanaring 
{
    [Serializable]
    public class BackpackItemData
    {
        public BackpackItemSO BackpackItem;

        public int Amount;

 

        //public void ModifyAmount(int amount)
        //{
        //    _amount += amount;
        //}

        //public void SetBackpackItem (BackpackItemSO backpackItemSO)
        //{
        //    _backpackItem = backpackItemSO;
        //}
    }

    [Serializable]
    public class Backpack 
    {
        [Header("TODO : make this non-SerializeField later")]
        [Tooltip("manually assign BackpackItem  for testing")]
        [SerializeField]
        private List<BackpackItemData> _backpackItemSO   ;

        private InventoryDatabaseSO m_inventoryDatabase;
 
        //public void SaveBackpackItems()
        //{
        //    //Save the Unique id from all of the BackpackItemData in _backpackItemSO
        //    //Duplicate element is allowed in the local save file 
        //    //for emaple, list of uniqueID saved in the local might be 
        //    //ITEM_1, ITEM_2, ITEM_1 => this indicates that ITEM_A amount equal to 2 
        //    //Getting Address of the record in database =>
        //    //m_inventoryDatabase.GetRecordKey(_backpackItemSO[0].GetBackpackItem());



        //}

        public void LoadBackpackItemFromDatabase(List<string> uniqueID)
        {
            LoadItemDatabaseOP();

            if (_backpackItemSO != null)
                throw new System.Exception("Try to laod spell from data base multiple time.This isn't allowed. " +
                    "The system should be loaded only 1 time when the save is loaded, and modified the SpellAction thoughtout the lifetime of application, " +
                    "and save the uniqueID when the game is saved");

            _backpackItemSO = new List<BackpackItemData>();

            for (int i = 0; i < uniqueID.Count; i++)
            {
                AddItemIntoBackpack(m_inventoryDatabase.GetRecord(uniqueID[i]), 1);
            }
        }

        public void RestoreBackpackItem(List<string> uniqueID)
        {
            // temporary data holder for update the backpack item
            Dictionary<BackpackItemSO, int> currentBackpackItem = new Dictionary<BackpackItemSO, int>();

            for (int i = 0; i < uniqueID.Count; i++)
            {
                BackpackItemSO item = m_inventoryDatabase.GetRecord(uniqueID[i]);
                // already contains key add amount instead
                if (currentBackpackItem.ContainsKey(item))
                {
                    currentBackpackItem[item] += 1;
                    continue;
                }
                // add unique
                currentBackpackItem.Add(item, 1);
            }

            foreach (var data in currentBackpackItem)
            {
                UpdateItemInBackpack(data.Key, data.Value);
            }
        }

        private void LoadItemDatabaseOP()
        {
            if (m_inventoryDatabase != null)
                return; 

            m_inventoryDatabase = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<InventoryDatabaseSO>(DatabaseAddressLocator.GetInventoryDatabaseAddress);
        }

      

        public void AddItemIntoBackpack(BackpackItemSO itemSO, int amount)
        {
            if ( _backpackItemSO == null)
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
                    _backpackItemSO[i].Amount += (-(int) MathF.Abs(amount));// += amount;
                    if (_backpackItemSO[i].Amount <= 0)
                        _backpackItemSO.RemoveAt(i);

                    return;
                }
            }
        }

        public List<BackpackItemData> GetCombatUseableItemSOs()
        {
            List<BackpackItemData> ret = new List<BackpackItemData>();
            foreach (var backpackItem in _backpackItemSO)
            {
                ret.Add(backpackItem); 

                if ((backpackItem.BackpackItem) is not CombatUseableItemSO)
                {
                    ret.RemoveAt(ret.Count - 1)  ;
                }
            }

            return _backpackItemSO ; 
        }
        #region Save System

        public object CaptureState()
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
                    Debug.Log("here surview"); 
                    keys.Add(m_inventoryDatabase.GetRecordKey(backpackItem.BackpackItem));
                    Debug.Log("end surview") ;
                }
            }
            
            return keys;
        }

        public void RestoreState(object state)
        {
            List<string> saveData = (List<string>)state;

            if(_backpackItemSO != null)
            {
                RestoreBackpackItem(saveData);
                return;
            }
            //Debug.LogError("LoadBackpackItemFromDatabase");
            //LoadBackpackItemFromDatabase(saveData);
            //_partyDataLocator.RestoreState(saveData.savePartyDataLocator);
        }

        #endregion

    }
}
