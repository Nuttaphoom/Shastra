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
 
public class ItemUserHandler : MonoBehaviour //inventory
{
    [Header("right now me manullay assign Item factory for quick demo")]
    [SerializeField]
    private List<ItemAbilityFactorySO> _itemInventory = new List<ItemAbilityFactorySO>() ;
    
    private  List<ItemAbilityRuntime> _runtimeItems = new List<ItemAbilityRuntime>() ;

    public List<ItemAbilityRuntime> Items => _runtimeItems;  

    private CombatEntity _combatEntity;


    private void Awake()
    {
        _combatEntity = GetComponent<CombatEntity>() ;

        FactorizeItemInInventory();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ColorfulLogger.LogWithColor("00000000", Color.cyan);
            _runtimeItems.RemoveAt(0);
        }
    }

    private void FactorizeItemInInventory()
    {
        //TODO : Load inventory from somewhere instead of manually assign them
        foreach (ItemAbilityFactorySO factory in _itemInventory)
        {
            _runtimeItems.Add(factory.FactorizeRuntimeItem());
        }
    }

    public void UseItem(ItemAbilityRuntime runtimeItem)
    {
        StartCoroutine(UseItemCoroutine(runtimeItem) ); 
    }

    private IEnumerator UseItemCoroutine(ItemAbilityRuntime runtimeItem)
    { 
        yield return TargetSelectionFlowControl.Instance.InitializeItemTargetSelectionScheme(_combatEntity, runtimeItem);
        ColorfulLogger.LogWithColor("USE ITEM", Color.cyan);
        yield return null; 
    }

    public void RemoveItem(ItemAbilityRuntime item)
    {
        //TODO - visually remove item from ivnentory 
        //_runtimeItems.Remove(item) ;
    }
}