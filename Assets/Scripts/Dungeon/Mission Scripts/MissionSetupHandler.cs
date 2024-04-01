using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring
{
    public class MissionSetupHandler : MonoBehaviour , ISceneLoaderWaitForSignal
    {
        #region Event Broadcaster 
        private EventBroadcaster _eventBroadcaster;

        protected EventBroadcaster EventBroadcaster
        {
            get
            {
                if (_eventBroadcaster == null)
                {
                    _eventBroadcaster = new EventBroadcaster();
                    _eventBroadcaster.OpenChannel<Null>("OnMissionSetUpComplete");
                }

                return _eventBroadcaster;

            }
        }

        public void SubOnEnvironmentSetup(UnityAction<Null> func)
        {
            EventBroadcaster.SubEvent(func, "OnMissionSetUpComplete");
        }

        public void UnSubOnEnvironmentSetup(UnityAction<Null> func)
        {
            EventBroadcaster.UnSubEvent(func, "OnMissionSetUpComplete");
        }

        #endregion 

        /// <summary>
        /// TODO : Pass this data from the quest selection menu
        /// </summary>
        //[SerializeField]
        //private MissionNodeEnvironment _dungeonNodeEnvTemplate;

        private MissionNodeEnvironment _dungeonEvn;

        #region GETTER

        public MissionNodeEnvironment DungeonEnvironment
        {
            get
            {
                if (_dungeonEvn == null)
                    throw new Exception("_dungeonEvn is null"); 

                return _dungeonEvn; 
            }
        }
        #endregion

        /// <summary>
        /// Call when player get into dungeon on the first time
        /// </summary>
        /// <returns></returns>

        public IEnumerator LoadEnvironmentData(MissionNodeEnvironment nodeEnvironment )
        {
            _dungeonEvn = Instantiate(nodeEnvironment, transform);

            yield return null;
        }

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return LoadEnvironmentData(MissionManagerSingleton.Instance.CurrentMissionDataSO.MissionNodeEnvironment) ;
            
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            MissionNodeManager dm = FindObjectOfType<MissionNodeManager>();
            //Set up logic transition detail
            dm.StartCoroutine(dm.SetUpDungeonCoroutine(DungeonEnvironment.GetFirstNode));
            
            EventBroadcaster.InvokeEvent<Null>(null, "OnMissionSetUpComplete");

            yield return null; 
        }
    }
}
