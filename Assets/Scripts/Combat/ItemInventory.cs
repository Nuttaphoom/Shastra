
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
        public struct ItemInventoryData
        {
            [SerializeField]
            public ItemActionFactorySO itemData;
            [SerializeField]
            public int amount;
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
            List<SpellActionSO> spellList = new List<SpellActionSO>();

            List<BackpackItemData> backpackItems =  PersistentPlayerPersonalDataManager.Instance.GetBackpack.GetCombatUseableItemSOs();

            _itemInventory = new List<ItemInventoryData>(); 

            foreach (BackpackItemData backpackItem in backpackItems)
            {
                _itemInventory.Add(new ItemInventoryData() { 
                    itemData = (backpackItem.BackpackItem as CombatUseableItemSO).ItemActionFactory ,
                    amount = backpackItem.Amount ,
                });
            }

            SetUpRuntimeItemFromItemInventory(); 

             yield return null;
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
            ColorfulLogger.LogWithColor("._itemInventoryAbility is " + _itemInventoryAbility.Count, Color.yellow);
            ColorfulLogger.LogWithColor("._itemInventoryAmount is " + _itemInventoryAmount.Count, Color.yellow);

        }
    }
}
