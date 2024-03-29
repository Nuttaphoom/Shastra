using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class DungeonSetupHandler : MonoBehaviour , ISceneLoaderWaitForSignal
    {
        /// <summary>
        /// TODO : Pass this data from the quest selection menu
        /// </summary>
        [SerializeField]
        private DungeonNodeEnvironment _dungeonNodeEnvTemplate;

        private DungeonNodeEnvironment _dungeonEvn;

        /// <summary>
        /// Call when player get into dungeon on the first time
        /// </summary>
        /// <returns></returns>
        


        public IEnumerator LoadEnvironmentData()
        {
            _dungeonEvn = Instantiate(_dungeonNodeEnvTemplate, transform);

            yield return null;
        }

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return LoadEnvironmentData();
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            DungeonNodeManager dm = FindObjectOfType<DungeonNodeManager>();
            //Set up logic transition detail
            dm.StartCoroutine(FindObjectOfType<DungeonNodeManager>().SetUpDungeonCoroutine(FindObjectOfType<DungeonNodeEnvironment>().GetFirstNode));
            yield return null;
            
        }
    }
}
