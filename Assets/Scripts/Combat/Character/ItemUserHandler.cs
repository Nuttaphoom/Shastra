using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Vanaring_DepaDemo;

/// <summary>
/// Used in CombatEntity class to handle casting, modify energy, and stuffs about item 
/// </summary>
/// 
[Serializable]
public class ItemUserHandler : RequireInitializationHandler<CombatEntity,Null,Null>  //inventory
{
    [Header("right now me manullay assign Item factory for quick demo")]
    [SerializeField]
    private List<ItemAbilityFactorySO> _itemInventory = new List<ItemAbilityFactorySO>() ;
    
    private  List<ItemAbilityRuntime> _runtimeItems = new List<ItemAbilityRuntime>() ;

    public List<ItemAbilityRuntime> Items => _runtimeItems;  

    private CombatEntity _combatEntity;

    public override void Initialize(CombatEntity argc, Null argv = null, Null argg = null)
    {
        if (IsInit)
            throw new Exception("Trying to initialize ItemUserHandler more than once");

        _combatEntity = argc; 

        SetInit(true) ;
        FactorizeItemInInventory(); 

    }

    public void FactorizeItemInInventory()
    {
        if (! IsInit)
            throw new Exception("ItemUserHandler hasn't never been inited") ;

        //TODO : Load inventory from somewhere instead of manually assign them
        foreach (ItemAbilityFactorySO factory in _itemInventory)
        {
            _runtimeItems.Add(factory.FactorizeRuntimeItem());
        }
    }
}