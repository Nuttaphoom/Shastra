

using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Vanaring
{
    public class EntityLoader : MonoBehaviour
    {
        private  EntityLoaderPoolSO  _pools;


        public void ReceiveEntityLoaderPool(EntityLoaderPoolSO pool)
        {
            _pools = pool; 
        }

        public List<CombatEntity> GetCombatEntitiesFromPool()
        {
            if (_pools == null)
                throw new Exception("_pools hasn't neven been assigned");
            List<CombatEntity> ret = new List<CombatEntity>();

            EntityLoaderPoolSO pool = _pools ;

            int index = 0;
            foreach (var entityPrefabPool in pool.GetEntityPrefabPoolStruct)
            {
                ret.Add( SpawnPrefab(entityPrefabPool.Prefab, entityPrefabPool.DedicatedLocation));
                ret[index].CombatEntityAnimationHandler.SetDedicateLocation(entityPrefabPool.DedicatedLocation) ;
                index++;
            }

            return ret;
        }

        public CombatEntity SpawnPrefab(CombatEntity prefab, int location = -1)
        {
            CombatEntity newEntity = GameObject.Instantiate(prefab);


            newEntity.name = newEntity.name + "Loaded" ;


            return newEntity; 
        }


        private void OccupyLocation(CombatEntity newEntity, int index )
        {
            if (index == -1)
            {
                //EntityPositionManager.Instance.OccupieAnyValidLocation( ECompetatorSide.Hostile, newEntity);
            }
            else
            {
                 EntityPositionManager.Instance.OccupieLocation(ECompetatorSide.Hostile, index, newEntity);
            }

            return; 

            
        }

   

    }
}