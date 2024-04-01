using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring
{
    /// <summary>
    /// Not Exactly Persistent, this object will destroy self when exit dungeon 
    /// </summary>
    public class MissionManagerSingleton : PersistentInstantiatedObject<MissionManagerSingleton>
    {
        private MissionPartyHandler _dungeonPartyHandler;

        private MissionCompletetionHandler _missionCompletetionHandler; 

        private MissionDataSO _currentMissionDataSO;

        #region GETTER
        public MissionDataSO CurrentMissionDataSO
        {
            get
            {
                if (_currentMissionDataSO == null)
                {
                    _currentMissionDataSO = PersistentSceneLoader.Instance.ExtractSavedData<DungeonMissionInstance>("DungeonMissionInstanceFromDungeonManager").GetData().MissionData;
                    if (_currentMissionDataSO == null)
                    {
                        throw new Exception("_currentMissionDataSO couldn't be extracted from the DataUser data");
                    }
                }
                return _currentMissionDataSO;
            }
        }

        public MissionPartyHandler DungeonPartyHandler
        {
            get
            {
                if (_dungeonPartyHandler == null)
                    throw new Exception("_dungeonPartyHandler hasn't never been assigned");
                return _dungeonPartyHandler; 
            }
        }
        #endregion

        #region Public Method 
        /// <summary>
        /// Call only once when player "select and enter mission from mission selection menu" 
        /// </summary>
        public void SetUpMission()
        {
            _dungeonPartyHandler = new MissionPartyHandler ();
            _missionCompletetionHandler = new MissionCompletetionHandler(); 

            _dungeonPartyHandler.SetUpRuntimeParty();

        }

        /// <summary>
        /// call only once when mission is completed 
        /// </summary>
        /// <param name="missionCompleteStatus"></param>
        public void ExitDungeon(MissionCompleteStatus missionCompleteStatus)
        {
            StartCoroutine(ExitDungeonCoroutine(missionCompleteStatus));
        }
        #endregion
        private IEnumerator ExitDungeonCoroutine(MissionCompleteStatus missionCompleteStatus)
        {
            //DisplayMission Complete UI and get reward accordingly 
            yield return _missionCompletetionHandler.ResolveMissionCompleteStatus(missionCompleteStatus);

            //Handle OnExit for every dungeon 
            _dungeonPartyHandler.OnExitMission() ;

            DungeonManagerSingleton.Instance.OnExitMission(); 



        }


    }
}
