using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    [Serializable]
    public struct BackpackItemData
    {
        [SerializeField]
        private BackpackItemSO _backpackItem;

        [SerializeField]
        private int _amount;

        public int GetItemAmount() { return _amount; }
        public BackpackItemSO GetBackpackItem() { return _backpackItem; }

        public void ModifyAmount(int amount)
        {
            _amount += amount;
        }

        public void SetBackpackItem (BackpackItemSO backpackItemSO)
        {
            _backpackItem = backpackItemSO;
        }
    }

    [Serializable]
    public class Backpack 
    {
        [Header("TODO : make this non-SerializeField later")]
        [Tooltip("manually assign BackpackItem  for testing")]
        [SerializeField]
        private List<BackpackItemData> _backpackItemSO = new List<BackpackItemData>() ;


        public Backpack()
        {
            LoadBackpackItemFromLocal(); 
        }

        public void LoadBackpackItemFromLocal()
        {
            //TODO : Load Data from the local and assign it to _backpackItemSO
        }

        public void AddItemIntoBackpack(BackpackItemSO itemSO, int amount)
        {
            for (int i = 0; i < _backpackItemSO.Count; i++)
            {
                if (_backpackItemSO[i].GetBackpackItem().GetDescriptionBaseField().FieldName == itemSO.GetDescriptionBaseField().FieldName)
                {
                    // Update the item at index i
                    _backpackItemSO[i].ModifyAmount(amount);// += amount;
                    return;
                }
            }

            BackpackItemData backpackItemData = new BackpackItemData();
            backpackItemData.SetBackpackItem(itemSO);
            backpackItemData.ModifyAmount(amount);
            _backpackItemSO.Add(backpackItemData);
            return; 
        }

        public void RemoveItemFromBackpack(BackpackItemSO itemSO, int amount)
        {
            for (int i = 0; i < _backpackItemSO.Count; i++)
            {
                if (_backpackItemSO[i].GetBackpackItem().GetDescriptionBaseField().FieldName == itemSO.GetDescriptionBaseField().FieldName)
                {
                    // Update the item at index i
                    _backpackItemSO[i].ModifyAmount(-(int) MathF.Abs(amount));// += amount;
                    return;
                }
            }
        }


    }
}
