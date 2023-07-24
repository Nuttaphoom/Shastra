using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Vanaring_DepaDemo;

/// <summary>
/// Used in CombatEntity class to handle casting, modify energy, and stuffs about item 
/// </summary>
/// 
[Serializable]
public class ItemUserHandler  //inventory
{
    [SerializeField]
    private List<ItemAbilitySO> _itemAbilities = new List<ItemAbilitySO>() ;
    public List<ItemAbilitySO> ItemAbilities => _itemAbilities;

    private CombatEntity _combatEntity; 
}