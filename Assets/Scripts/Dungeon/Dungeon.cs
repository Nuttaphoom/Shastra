using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [Serializable]
    public class Dungeon  : MonoBehaviour
    {
        [SerializeField]
        private DungeonDataSO _dungeonData;

        [SerializeField]
        private List<MissionDataSO> _missionsOnThisDungeon;

        private void Awake()
        {
            FindObjectOfType<DungeonManager>().RegisterDungeon(this) ; 
        }
        public RuntimeDungeon FactorizeRuntimeDungeon()
        {
            return new RuntimeDungeon(_missionsOnThisDungeon, _dungeonData); 
        }
    }
}
