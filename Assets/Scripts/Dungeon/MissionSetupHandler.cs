using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class MissionSetupHandler : MonoBehaviour , ISceneLoaderWaitForSignal
    {
        /// <summary>
        /// TODO : Pass this data from the quest selection menu
        /// </summary>
        [SerializeField]
        private MissionNodeEnvironment _dungeonNodeEnvTemplate;

        private MissionNodeEnvironment _dungeonEvn;

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
            MissionNodeManager dm = FindObjectOfType<MissionNodeManager>();
            //Set up logic transition detail
            dm.StartCoroutine(FindObjectOfType<MissionNodeManager>().SetUpDungeonCoroutine(FindObjectOfType<MissionNodeEnvironment>().GetFirstNode));
            yield return null;
            
        }
    }
}
