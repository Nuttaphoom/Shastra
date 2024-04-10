using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [Serializable]
    public class DungeonMapObject  : MonoBehaviour, ISceneLoaderWaitForSignal   
    {
        [SerializeField]
        private DungeonDataSO _dungeonData;

        public RuntimeDungeon FactorizeRuntimeDungeon()
        {
            return new RuntimeDungeon(_dungeonData.GetMissionDataSOes, _dungeonData); 
        }

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            FindObjectOfType<DungeonManagerSingleton>().RegisterDungeon(this);

            yield return null; 
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            yield return null; 
        }
    }
}
