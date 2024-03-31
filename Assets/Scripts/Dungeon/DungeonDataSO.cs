using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "DungeonDataSO", menuName = "ScriptableObject/DungeonMission/DungeonDataSO")]
    public class DungeonDataSO : ScriptableObject
    {
        [SerializeField]
        private string _dungeonName;

        #region GETTER 
        private string DungeonName => _dungeonName; 


        #endregion
    }
}
