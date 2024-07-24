using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Vanaring.DungeonManagerSingleton;
using static Vanaring.MissionSetupHandler;

namespace Vanaring
{
    public class MissionSetupHandler : MonoBehaviour, ISaveable
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

        //Use for Save / Load 
        private Dictionary<string, List<NodeRuntimeData>> _nodeRuntimeData;

        #region GETTER
        private DungeonMissionInstance GetCurrentActiveDungeon
        {
            get
            {
                return DungeonManagerSingleton.Instance.CurrentActiveMission; 
            }
        }
        public string MissionName => GetCurrentActiveDungeon.MissionData.MissionDescription.FieldName;
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
        /// Call when player get into dungeon on everytime dungeon scene is loaded
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadEnvironmentData(MissionNodeEnvironment nodeEnvironment)
        {
            _dungeonEvn = MonoBehaviour.Instantiate(nodeEnvironment);
            string missionName = GetCurrentActiveDungeon.MissionData.MissionDescription.FieldName;

            if (_nodeRuntimeData != null && _nodeRuntimeData.ContainsKey(missionName))
                _dungeonEvn.RestoreEnvironmentData(_nodeRuntimeData[missionName]); 

            yield return null;
        }


        /// <summary>
        /// Called AFTER save / load scene is completed
        /// </summary>
        /// <returns></returns>
        public IEnumerator SetUpMission(DungeonMissionInstance dungeonMissionInstance)
        {
            MissionNodeManager missionNodeManager = FindObjectOfType<MissionNodeManager>();
 
            yield return LoadEnvironmentData(dungeonMissionInstance.MissionData.MissionNodeEnvironment);

            //Set up logic transition detail
            missionNodeManager.StartCoroutine(missionNodeManager.SetUpDungeonCoroutine(DungeonEnvironment.GetLastVisitedNode));

            EventBroadcaster.InvokeEvent<Null>(null, "OnMissionSetUpComplete");

            yield return null;
        }
       

        /// <summary>
        /// Clear saved data in Dungeon Environment, ensure next time player visite Mission will be clean
        /// </summary>
        public void OnExitMission()
        {
            _dungeonEvn.OnMissionExit();
        }

        #region Save Load Method
        public object CaptureState()
        {
            if (GetCurrentActiveDungeon == null)
                throw new Exception("Current Active Dungeon shouldn't be null when player is inside Mission");

            string fieldName = GetCurrentActiveDungeon.MissionData.MissionDescription.FieldName;

            List<NodeRuntimeData> nodeRuntimeDatas = _dungeonEvn.CaptureEnvironmentData();

            Dictionary<string, List<NodeRuntimeData>> ret = new Dictionary<string, List<NodeRuntimeData>>();
            ret.Add(fieldName, nodeRuntimeDatas);
            //ColorfulLogger.LogWithColor("Capture state in MissionSetupHandler with data.count : " + ret.Count, Color.blue);
            return ret;
        }

        public void RestoreState(object state)
        {
            //if (GetCurrentActiveDungeon == null)
            //    throw new Exception("Current Active Dungeon shouldn't be null when player is inside Mission");
            
            _nodeRuntimeData = (Dictionary<string, List<NodeRuntimeData>>)state; //new Dictionary<string, List<NodeRuntimeData>>();
             
            //ColorfulLogger.LogWithColor("Restore state in MissionSetupHandler with data.count : " + _nodeRuntimeData.Count, Color.blue);

        }
        #endregion

        [Serializable]
        public struct NodeRuntimeData
        {
            public bool IsVisited;
            public bool CurrentlyVisited;
            public bool MissionDirty;
        }
    }
}
