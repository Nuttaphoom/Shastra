

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
        private List<Transform> _spawnPositionSet = new List<Transform>();

        private Dictionary<CombatEntity, Transform> _spawnPositionDict = new Dictionary<CombatEntity, Transform>(); 
        private List<Transform> _available = new List<Transform>();

        private void Awake()
        {
            _available = new List<Transform>(); 

            foreach (Transform tf in _spawnPositionSet)
                _available.Add(tf); 
        }
        public List<CombatEntity> LoadData()
        {
            List<CombatEntity> ret = new List<CombatEntity>();

            EntityLoaderPoolSO pool = _pools[0];
            _pools.RemoveAt(0);

            int index = 0;
            foreach (var entityPrefabPool in pool.GetEntityPrefabPoolStruct)
            {
                ret.Add( SpawnPrefab(entityPrefabPool.Prefab, entityPrefabPool.DedicatedLocation)); 
                index++;
            }

            return ret;
        }

        public CombatEntity SpawnPrefab(CombatEntity prefab, int location = -1)
        {
            if (_available.Count == 0)
                throw new Exception("Available.Count is equal to 0");
            
            CombatEntity newEntity = GameObject.Instantiate(prefab);


            if (location == -1)
            {
                // If location is -1, use the first available position
                UseAvailablePosition(newEntity,0);
            }
            else
            {
                int index = location - _spawnPositionDict.Count;

                if (index >= 0 && _available[index] != null)
                {
                    // If the index is valid and the available position is not null, use it
                    UseAvailablePosition(newEntity, index);
                }
                else
                {
                    // Otherwise, use the first available position
                    UseAvailablePosition(newEntity,0);
                }
            }

            Transform t = _spawnPositionDict[newEntity] ; 

            newEntity.transform.position = t.position;
            newEntity.transform.forward = t.forward;

            newEntity.name = newEntity.name + "Loaded" ;

            Debug.Log("load " + newEntity);

            return newEntity; 
        }


        void UseAvailablePosition(CombatEntity newEntity, int index)
        {
            _spawnPositionDict.Add(newEntity, _available[index]);
            _available.RemoveAt(index);
        }

    

        public void ReleasePosition(CombatEntity ce)
        {
            _available.Add(_spawnPositionDict[ce]) ;
            _spawnPositionDict.Remove(ce) ;


        }

    }
}