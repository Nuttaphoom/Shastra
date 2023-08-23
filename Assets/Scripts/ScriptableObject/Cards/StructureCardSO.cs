using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StructureCardSO", menuName = "ScriptableObject/Cards/StructureCardSO")]
public class StructureCardSO : BaseCardSO
{
    [SerializeField]
    private RoomSO _roomSO; 

    //Card Effect would be implemented in here
   

    public override RuntimeCardEffect FactorizeRuntimeCardEffect()
    {
        return new StructureRuntimeCardEffect(_roomSO) ; 
    }
}

public class StructureRuntimeCardEffect : RuntimeCardEffect
{
    [SerializeField]
    private RoomSO _roomSO; 
    public StructureRuntimeCardEffect(RoomSO roomSO)
    {
        _roomSO = roomSO;
    }

    public override void ExecuteRuntime()
    {
        for (int i =0; i < _targets.Count; i++)
        {
            (_targets[i] as Tile).Build(_roomSO);
        }
    }

    public override System.Type GetTargetType()
    {
        return typeof(Tile); 
    }

    public override bool VerifyTarget(List<Component> directTarget)
    {
        //Check if every element is suitable for applying card 
        for (int i =0; i < directTarget.Count; i++)
        {
            if (directTarget[i].GetType() != typeof(Tile))
                return false; 
        }

        //Assign selected target to runtime target
        _targets = directTarget; 
        return true; 
    }
}
