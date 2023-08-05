

using System;
using System.Collections.Generic;
using UnityEngine;
using Vanaring_DepaDemo;

public class EntityLoader : MonoBehaviour
{
    [SerializeField]
    private List<EntityLoaderPoolSO> _pools ;

    [SerializeField]
    private  List<Transform>  _spawnPosition = new  List<Transform> ();
    public List<CombatEntity> LoadData()
    { 
        List<CombatEntity> ret = new List<CombatEntity>();

        EntityLoaderPoolSO pool = _pools[0] ;
        _pools.RemoveAt(0);

        int index = 0;
        foreach (CombatEntity entity in pool.Entities)
        {
            Transform t = _spawnPosition[index];
            CombatEntity newEntity = GameObject.Instantiate(entity) ;
            newEntity.transform.position = t.position;
            newEntity.transform.forward = t.forward ; 

            ret.Add(newEntity);
            
            index ++; 
        }

 
        return ret; 
    }

     


}