using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "CharacterEntityPrefabDatabaseSO ", menuName = "ScriptableObject/Database/TestCharacterEntityPrefabDatabaseSO ")]
    public class CharacterEntityPrefabDatabaseSO : ScriptableObject
    {
        [SerializeField]
        private List<ControlableEntity> controlableEntities;

        public List<ControlableEntity> GetAllControlableEntiites => controlableEntities; 
    }
}
