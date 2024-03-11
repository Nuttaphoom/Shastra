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
        private List<ItemActionFactorySO> _itemInventory = new List<ItemActionFactorySO>();

        private List<ItemAbilityRuntime> _runtimeItems = new List<ItemAbilityRuntime>();
        private List<int> _runtimeItemsAmount = new List<int>();

        public List<ItemAbilityRuntime> Items => _runtimeItems;
        public List<int> ItemsAmount => _runtimeItemsAmount;

        private CombatEntity _combatEntity;

       

        private void Awake()
        {

            _combatEntity = GetComponent<CombatEntity>();

            //FactorizeItemInInventory(); 
        }

        // TODO : Call FactorizeItemInInventory into Awake in correct order 
      
        public void FactorizeItemInInventory()
        {
            ColorfulLogger.LogWithColor(".factorize item ", Color.yellow );

            _itemInventory = ItemInventory.instance.GetItemInventoryAbility;

            _runtimeItemsAmount = ItemInventory.instance.GetItemInventoryAmount;

            Debug.Log("item " + _itemInventory.Count + " amount is " + _runtimeItemsAmount.Count);

            _runtimeItems = new List<ItemAbilityRuntime>(); 

            foreach (ItemActionFactorySO factory in _itemInventory)
            {
                _runtimeItems.Add(factory.FactorizeRuntimeAction(_combatEntity) as ItemAbilityRuntime);
            }
        }

        public void UseItem(ItemAbilityRuntime runtimeItem)
        {
            StartCoroutine(UseItemCoroutine(runtimeItem));
        }

        private IEnumerator UseItemCoroutine(ItemAbilityRuntime runtimeItem)
        {
            yield return TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme(_combatEntity, runtimeItem);
         }

        public void RemoveItem(ItemAbilityRuntime runtimeItem)
        {
            ItemInventory.instance.RemoveItem(runtimeItem); 
            ////TODO - visually remove item from inventory 
            //int count = 0;

            //foreach (ItemAbilityRuntime item in _runtimeItems)
            //{
            //    if (item.ItemName == runtimeItem.ItemName)
            //    {
            //        _runtimeItemsAmount[count]--;
            //        if (_runtimeItemsAmount[count] <= 0)
            //        {
            //            _runtimeItems.RemoveAt(count);
            //            _runtimeItemsAmount.RemoveAt(count);
            //        }
            //        break;
            //    }
            //    count++;
            //}

            ////TODO : fix this 


        }
    }
}