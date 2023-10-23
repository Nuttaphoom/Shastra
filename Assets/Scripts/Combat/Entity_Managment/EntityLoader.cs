

using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Vanaring
{
    public class EntityLoader : MonoBehaviour
    {
        [SerializeField]
        private List<EntityLoaderPoolSO> _pools;

        [SerializeField]
        private List<Transform> _spawnPosition = new List<Transform>();
        public List<CombatEntity> LoadData()
        {
            List<CombatEntity> ret = new List<CombatEntity>();

            EntityLoaderPoolSO pool = _pools[0];
            _pools.RemoveAt(0);

            int index = 0;
            foreach (CombatEntity entity in pool.Entities)
            {
                ret.Add( SpawnPrefab(entity, index) );
                index++;
            }


            return ret;
        }

        public CombatEntity SpawnPrefab(CombatEntity prefab,int spawnPositionIndex)
        {
            Transform t = _spawnPosition[spawnPositionIndex];
            CombatEntity newEntity = GameObject.Instantiate(prefab);
            newEntity.transform.position = t.position;
            newEntity.transform.forward = t.forward;

            newEntity.name = newEntity.name + spawnPositionIndex ;

            return newEntity; 
        }
    }
}