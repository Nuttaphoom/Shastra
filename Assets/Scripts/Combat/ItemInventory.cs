
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vanaring.Assets.Scripts.Combat.Utilities;

namespace Vanaring 
{
    public class ItemInventory : MonoBehaviour, ICombatRequireLoadData
    {
        // TODO : Not singleton inventory
        public static ItemInventory instance = null;

        [SerializeField]
        private bool DebuggingMode = false;

        [Serializable]
        public class ItemInventoryData
        {
            [SerializeField]
            public ItemActionFactorySO itemData;
            [SerializeField]
            public int amount;
            [SerializeField]
            public BackpackItemData backpackItemData;
        }

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("DebuggingMode")]
        List <ItemInventoryData> _itemInventory;

        private List<ItemActionFactorySO> _itemInventoryAbility;
        private List<int> _itemInventoryAmount;

        #region GETTER

        public List<ItemActionFactorySO> GetItemInventoryAbility => _itemInventoryAbility; 
        
        public List<int> GetItemInventoryAmount => _itemInventoryAmount;
  
        #endregion

        private void Awake()
        {
            instance = this;
            if (DebuggingMode)
            {
                SetUpRuntimeItemFromItemInventory(); 
            }
        }

        public IEnumerator LoadDataFromDatabase()
        {
            List<BackpackItemData> backpackItems =  PersistentPlayerPersonalDataManager.Instance.GetBackpack.GetCombatUseableItemSOs();

            if (_itemInventory == null)
                _itemInventory = new List<ItemInventoryData>(); 

            for (int i = 0; i < backpackItems.Count; i++)
            {
                BackpackItemData backpackItem = backpackItems[i]; 

                _itemInventory.Add(new ItemInventoryData() { 
                    itemData = (backpackItem.BackpackItem as CombatUseableItemSO).ItemActionFactory ,
                    amount = backpackItem.Amount ,
                    backpackItemData = backpackItem 
                });

                PersistentPlayerPersonalDataManager.Instance.GetBackpack.RemoveItemFromBackpack(backpackItem.BackpackItem, backpackItem.Amount);

            }

            SetUpRuntimeItemFromItemInventory(); 

             yield return null;
        }

        public void RestoreRemainingItemIntoDatabase()
        {
            if (_itemInventory == null)
                _itemInventory = new List<ItemInventoryData>();

            for (int i = 0; i < _itemInventory.Count; i++)
            {
                ItemInventoryData itemInventoryData = _itemInventory[i];

                PersistentPlayerPersonalDataManager.Instance.GetBackpack.AddItemIntoBackpack(itemInventoryData.backpackItemData.BackpackItem, itemInventoryData.amount);

            }
        }

        public void RemoveItem(ItemAbilityRuntime itemToRemove)
        {
            int count = 0;
            
            foreach (var item in _itemInventoryAbility)
            {
                if (item.AbilityName == itemToRemove.ItemName)
                {
                    _itemInventoryAmount[count]--; 

                    if (_itemInventoryAmount[count] <= 0)
                    {
                        _itemInventoryAmount.RemoveAt(count);
                        _itemInventoryAbility.RemoveAt(count);
                    }
                    break;
                }
                count++;
            }

            //Remove item in which runtime instantiate from as we need this to restore back into inventory
            for (int i = 0; i < _itemInventory.Count; i++)
            {
                if (_itemInventory[i].backpackItemData.BackpackItem.GetDescriptionBaseField().FieldName == itemToRemove.ItemName)
                {
                    _itemInventory[i].amount -= 1 ;
                    if (_itemInventory[i].amount <= 0)
                    {
                        _itemInventory.RemoveAt(i); 
                    }
                    break;
                }


            }
        }

        public void SetUpRuntimeItemFromItemInventory()
        {
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
