using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    /// <summary>
    /// Not Exactly Persistent, this object will destroy self when exit dungeon 
    /// </summary>
    public class MissionManagerSingleton 
    {
        private static MissionManagerSingleton _instance ;

        private MissionPartyHandler _dungeonPartyHandler;
        private MissionDataSO _currentMissionDataSO;

        public MissionDataSO CurrentMissionDataSO
        {
            get
            {
                if (_currentMissionDataSO == null)
                {
                    _currentMissionDataSO = PersistentSceneLoader.Instance.ExtractSavedData<DungeonMissionInstance>("DungeonMissionInstanceFromDungeonManager").GetData().MissionData ; 
                    if (_currentMissionDataSO == null)
                    {
                        throw new Exception("_currentMissionDataSO couldn't be extracted from the DataUser data"); 
                    }
                }
                return _currentMissionDataSO; 
            }
        }

 
        
        #region GETTER
        public static MissionManagerSingleton Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MissionManagerSingleton(); 

                return _instance; 
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

        public void OnEnterDungeon()
        {
            _dungeonPartyHandler = new MissionPartyHandler ();
            _dungeonPartyHandler.SetUpRuntimeParty();
        }

        public void OnExitDungeon()
        {
           _dungeonPartyHandler.OnExitDungeon();

            _instance = null; 

        }

        
    }
}
