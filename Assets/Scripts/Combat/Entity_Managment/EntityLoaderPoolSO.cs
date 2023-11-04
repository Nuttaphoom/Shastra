

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{

    [CreateAssetMenu(fileName = "EntityPool", menuName = "ScriptableObject/EntityManagement/EntityLoaderPoolSO")]
    public class EntityLoaderPoolSO : ScriptableObject
    {

        [Serializable]
        public struct EntityPrefabPoolStruct
        {
            public CombatEntity Prefab;
            public int DedicatedLocation; 
        }

        [SerializeField]
        private List<EntityPrefabPoolStruct> _entitiesPrefabPool;

        [SerializeField]
        private ECompetatorSide _competatorSide;

        public List<EntityPrefabPoolStruct> GetEntityPrefabPoolStruct => _entitiesPrefabPool;


    }
}
