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
    public class MissionManagerSingleton : MonoBehaviour, ISceneLoaderWaitForSignal
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

        //TODO : Create property entering dungeon state not just random bool
        private bool _firstTimeEnterDungeon = true ;
        
        #region GETTER
        public static MissionManagerSingleton Instance
        {
            get
            {
                if (_instance == null)
                    throw new System.Exception("Instance = null"); 

                return _instance; 
            }
        }

        public MissionPartyHandler DungeonPartyHandler
        {
            get
            {
                return _dungeonPartyHandler; 
            }
        }
        #endregion

        private void Awake()
        {
            if (_instance)
            {
                 if (_instance != this) 
                    Destroy(gameObject); 
            }else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        
        //These function should be call EVERYTIME we enter/back to dungeon 
        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return FindObjectOfType<MissionSetupHandler>().LoadEnvironmentData(CurrentMissionDataSO.MissionNodeEnvironment) ; 
            yield return null; 
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            if (_firstTimeEnterDungeon)
                yield return OnEnterDungeon() ;
        }

        ///////////


        public IEnumerator OnEnterDungeon()
        {
            _dungeonPartyHandler = new MissionPartyHandler ();
            yield return _dungeonPartyHandler.SetUpRuntimeParty();

            _firstTimeEnterDungeon = false; 
        }

        public IEnumerator OnExitDungeon()
        {
            yield return _dungeonPartyHandler.OnExitDungeon(); 
        }

        
    }
}
