using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring
{
    /// <summary>
    /// Used in CombatEntity class to handle casting, modify energy, and stuffs about item 
    /// </summary>
    /// 

    public class ItemUserHandler : MonoBehaviour //inventory
    {
        //[Header("right now me manullay assign Item factory for quick demo")]
        //[SerializeField]
        private List<ItemAbilityFactorySO> _itemInventory = new List<ItemAbilityFactorySO>();

        private List<ItemAbilityRuntime> _runtimeItems = new List<ItemAbilityRuntime>();
        private List<int> _runtimeItemsAmount = new List<int>();

        public List<ItemAbilityRuntime> Items => _runtimeItems;
        public List<int> ItemsAmount => _runtimeItemsAmount;

        private CombatEntity _combatEntity;

        [SerializeField]
        private ItemWindowManager _itemWindowManager;

        private void Awake()
        {
            _combatEntity = GetComponent<CombatEntity>();
        }
        // TODO : Call FactorizeItemInInventory into Awake in correct order 
        private void Start()
        {
            FactorizeItemInInventory();
            if (_itemWindowManager == null)
            {
                Debug.Log("item window manager is null");
            }
            _itemWindowManager.UpdateItemSocket(Items, ItemsAmount);
        }

        private void FactorizeItemInInventory()
        {
            //TODO : Load inventory from somewhere instead of manually assign them
            _itemInventory = ItemInventory.instance.GetItemInventoryAbility;
            _runtimeItemsAmount = ItemInventory.instance.GetItemInventoryAmount;
            foreach (ItemAbilityFactorySO factory in _itemInventory)
            {
                _runtimeItems.Add(factory.FactorizeRuntimeItem());
            }
        }

        public void UseItem(ItemAbilityRuntime runtimeItem)
        {
            StartCoroutine(UseItemCoroutine(runtimeItem));
        }

        private IEnumerator UseItemCoroutine(ItemAbilityRuntime runtimeItem)
        {
            //ColorfulLogger.LogWithColor("USE ITEM", Color.cyan);
            //RemoveItem(runtimeItem);
            //_itemWindowManager.UpdateItemSocket(_runtimeItems, _runtimeItemsAmount);

            yield return TargetSelectionFlowControl.Instance.InitializeItemTargetSelectionScheme(_combatEntity, runtimeItem);
            yield return null;
        }

        public void RemoveItem(ItemAbilityRuntime runtimeItem)
        {
            //TODO - visually remove item from inventory 
            int count = 0;
            foreach (ItemAbilityRuntime item in _runtimeItems)
            {
                if (item.ItemName == runtimeItem.ItemName)
                {
                    _runtimeItemsAmount[count]--;
                    if (_runtimeItemsAmount[count] <= 0)
                    {
                        _runtimeItems.RemoveAt(count);
                        _runtimeItemsAmount.RemoveAt(count);
                    }
                    break;
                }
                count++;
            }

            //TODO : fix this 

            foreach (ItemWindowManager windowManager in FindObjectsOfType<ItemWindowManager>())
            {
                windowManager.UpdateItemSocket(_runtimeItems, _runtimeItemsAmount);
            }
        }
    }
}