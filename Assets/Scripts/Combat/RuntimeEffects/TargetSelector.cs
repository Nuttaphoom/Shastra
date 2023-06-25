using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using Vanaring_DepaDemo;

public enum EffectTargetType
{
    Tile,
    Playground,
    Building,
    FrontierTile,
    Bot,
    Room
}

[Serializable]
public class TargetSelector    
{
    [Header("Maximum target that can be assigned to")]
    [SerializeField]
    private int _maxTargetSize = 1  ;

    //Used when the target selection is requires  
    public IEnumerator TargetSelectionCoroutine(List<CombatEntity> assignedTarget)
    {
        yield return null; 

        assignedTarget.Clear();
       
        while (assignedTarget.Count < _maxTargetSize)
        {
            CombatEntity detected;
            if ((detected = TickTargetSelected()) != null)
            {
                assignedTarget.Add(detected);    
            }

            yield return null;
        }

    }

    public CombatEntity TickTargetSelected()
    {
        //TODO :: Right now we get access from Input.getmousedown here
        //Which is not correct since we want to centralize the input systme

        CombatEntity ret = null ; 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            List<CombatEntity> hitTarget = new List<CombatEntity>();

            foreach (var hit in Physics.SphereCastAll(ray, 10.0f, Mathf.Infinity))
            {    
                if (hit.collider.TryGetComponent(out ret))
                {
                    break;    
                }
            }
        }

        return ret ;
    }

    public bool IsRequireSelectionQuery => _maxTargetSize > 0; 
}
