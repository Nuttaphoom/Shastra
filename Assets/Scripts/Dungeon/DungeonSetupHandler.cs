using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class DungeonSetupHandler : MonoBehaviour, ISceneLoaderWaitForSignal
    {
        /// <summary>
        /// TODO : Pass this data from the quest selection menu
        /// </summary>
        [SerializeField]
        private DungeonNodeEnvironment _dungeonNodeEnvTemplate;

        private DungeonNodeEnvironment _dungeonEvn;

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            _dungeonEvn = Instantiate(_dungeonNodeEnvTemplate, transform);

            yield return null; 
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            DungeonManager dm = FindObjectOfType<DungeonManager>(); 

            dm.StartCoroutine(FindObjectOfType<DungeonManager>().SetUpDungeonCoroutine(_dungeonEvn.GetFirstNode)); 

            yield return null;
        }
    }
}
