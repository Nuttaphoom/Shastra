using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "BackpackItem", menuName = "ScriptableObject/Backpack/CombatUseableItemSO")]

    public class CombatUseableItemSO :  BackpackItemSO 
    {
        [SerializeField]
        private ItemActionFactorySO _itemActionFactory; 
        public override DescriptionBaseField GetDescriptionBaseField()
        {
            return _itemActionFactory.DescriptionBaseField; 
        }

        


    }
}
