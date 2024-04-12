using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

namespace Vanaring
{
    /// <summary>
    /// Handle mission selection, enter mission, exit mission 
    /// </summary>
    public class DungeonManagerSingleton : MonoBehaviour, ISaveable , ISceneLoaderWaitForSignal
    {
        [SerializeField]
        private List<RuntimeDungeon> _dungeons;

        [SerializeField]
        private MissionManager _missionManager;

        private MissionSetupHandler _missionSetupHandler; 

        [SerializeField] 
        private AssetReferenceT<EssentialSceneDataSO> _base_missionScene ;

        private MissionCompletetionHandler _missionCompletetionHandler;

        private DungeonMissionInstance _currentActiveMission;

        private static DungeonManagerSingleton _instance;

        #region GETTER
        public DungeonMissionInstance CurrentActiveMission
        {
            get
            {
                return _currentActiveMission;
            }
        }
        public MissionSetupHandler MissionSetupHandler
        {
            get
            {
                if (_missionSetupHandler == null)
                    _missionSetupHandler = FindObjectOfType<MissionSetupHandler>(); 

                return _missionSetupHandler;
            }
        }

        public MissionManager MissionManager
        {
            get
            {
                if (_missionManager == null)
                    throw new Exception("MissionManager is null"); 

                return _missionManager; 
            }
        }

        public static DungeonManagerSingleton Instance { 
            get 
            {
                if (_instance == null)
                {
                    if (FindObjectsOfType<DungeonManagerSingleton>().Count() > 1)
                        throw new System.Exception("DungeonManagerSingleton can not exit more than one, check if previous dungeon has properly destroy DungeonManagerSingleton when exit mission");
                    
                    if (FindObjectsOfType<DungeonManagerSingleton>().Count() == 0)
                        throw new System.Exception("DungeonManagerSingleton can not be found");

                    _instance = FindObjectOfType<DungeonManagerSingleton>(); 

                }
                
                if (_instance == null)
                    Debug.Log("is instance equal null " + _instance);
                
                return _instance;
            } 
        }

        #endregion
        private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(_instance.gameObject);
            
            _instance  = this;
            _missionManager = new MissionManager();
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U)) {
                _dungeons[0].SelectThisDungeon();
                LoadSelectedMission(_dungeons[0].GetSelectMission(0)) ;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                _dungeons[0].SelectThisDungeon();
                LoadSelectedMission(_dungeons[0].GetSelectMission(1));
            }
        }

        /// <summary>
        /// This should be called before save / load scheme
        /// </summary>
        /// <param name="dungeon"></param>
        public void RegisterDungeon(DungeonMapObject dungeon)
        {
            if (_dungeons == null) 
                _dungeons = new List<RuntimeDungeon>() ; 
            
            _dungeons.Add(dungeon.FactorizeRuntimeDungeon());
        }
        #region Mission Selection Methods
        private void LoadSelectedMission(DungeonMissionInstance missionInstance)
        {
            //Visually dispaly confirm selection 
            PersistentSceneLoader.Instance.CreateLoaderDataUser<DungeonMissionInstance>("DungeonMissionInstanceFromDungeonManager", missionInstance) ;
            PersistentSceneLoader.Instance.LoadGeneralScene( PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(_base_missionScene) ) ;

        }

        #endregion

        #region Mission Scheme Methods
        /// <summary>
        /// Called before Save Load operation
        /// </summary>
        /// <param name="missionInstance"></param>
        private void StartMission(DungeonMissionInstance missionInstance)
        {
            _missionCompletetionHandler = new MissionCompletetionHandler(); 
            _missionManager.SetUpMission();

            _currentActiveMission = missionInstance;
            _currentActiveMission.OnStartMission(); 
        }

        public void ExitMission(MissionCompleteStatus missionCompleteStatus)
        {
            StartCoroutine(OnExitMission(missionCompleteStatus)); 
        }
        private IEnumerator OnExitMission(MissionCompleteStatus missionCompleteStatus)
        {
             EventRewardData eventRewardData = DungeonManagerSingleton.Instance._currentActiveMission.MissionData.EventRewardData; 
             
            //DisplayMission Complete UI and get reward accordingly 
            yield return _missionCompletetionHandler.ResolveMissionCompleteStatus(missionCompleteStatus);
            SubmitMissionRewardCoroutine(eventRewardData);


            //Handle OnExit for every dungeon componenets
            _missionManager.DungeonPartyHandler.OnExitMission();
            _missionSetupHandler.OnExitMission();

            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

            PersistentActiveDayDatabase.Instance.OnPostPerformSchoolAction(3); 
        }
        private void SubmitMissionRewardCoroutine(EventRewardData eventRewardData)
        {
            eventRewardData.GetReward().SubmitReward(); //;.GetEventRewards(); 

        }
        #endregion
        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            
           
            yield return null;
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            if (PersistentSceneLoader.Instance.IsSaveDataUserExit("DungeonMissionInstanceFromDungeonManager"))
            {
                DungeonMissionInstance missionInstance = PersistentSceneLoader.Instance.ExtractSavedData<DungeonMissionInstance>("DungeonMissionInstanceFromDungeonManager").GetData();

                StartMission(missionInstance);

            }
            if (_currentActiveMission != null)
            {
                if (MissionSetupHandler != null)
                    yield return MissionSetupHandler.SetUpMission(_currentActiveMission);
            }

            yield return null; //)
        }
        #region Mission Save/Load Methods
        public object CaptureState()
        {

            DungeonManagerSaveLoadDataStruct ret = new DungeonManagerSaveLoadDataStruct() {
                DungeonNamePair = new Dictionary<string, RuntimeDungeonSaveLoadData>(),
            } ; 
            
            //Capture Dungeon data
            foreach (var dungeon in _dungeons)
            {
                string dungeonName = dungeon.DungeonDataSO.DungeonName ;
                ret.DungeonNamePair.Add( dungeonName , dungeon.CaptureDungeonData())    ;
            }

            //Capture Mission Environment Data
            if (_currentActiveMission != null)
            {
                string missionName = _currentActiveMission.MissionData.MissionDescription.FieldName;

                
            }

            return ret ;

        }

        public void RestoreState(object state)
        {
            //Debug.Log("Restore data in dungeon manager singleton");
            DungeonManagerSaveLoadDataStruct saveLoadDataStruct = (DungeonManagerSaveLoadDataStruct) state;



            foreach (var dungeon in _dungeons)
            {
                dungeon.RestoreDungeonData(saveLoadDataStruct.DungeonNamePair[dungeon.DungeonDataSO.DungeonName]); 
            }

            

        }



        [Serializable]
        public struct DungeonManagerSaveLoadDataStruct
        {

            public Dictionary<string, RuntimeDungeonSaveLoadData> DungeonNamePair ; 
        }

        [Serializable]
        public struct RuntimeDungeonSaveLoadData
        {
            public Dictionary<string, RuntimeMissionSaveLoadData> MissionNamePair;
        }

        [Serializable]
        public struct RuntimeMissionSaveLoadData
        {
            public bool HasVisiteThisMission ;   
        }
      
        #endregion
    }
}
