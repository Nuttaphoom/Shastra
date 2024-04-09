using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class DungeonManagerSingleton : MonoBehaviour, ISaveable 
    {
        [SerializeField]
        private List<RuntimeDungeon> _dungeons;

        [SerializeField]
        private MissionManager _missioManager;

        [SerializeField] 
        private AssetReferenceT<EssentialSceneDataSO> _base_missionScene ;

        private MissionCompletetionHandler _missionCompletetionHandler;

        private static DungeonManagerSingleton _instance;

        private MissionDataSO _currentMissionDataSO;

        #region GETTER

        public MissionManager MissionManager
        {
            get
            {
                if (_missioManager == null)
                    throw new Exception("MissionManager is null"); 

                return _missioManager; 
            }
        }
         
        public MissionDataSO CurrentMissionDataSO
        {
            get
            {
                if (_currentMissionDataSO == null)
                {
                     
                        throw new Exception("_currentMissionDataSO couldn't be extracted from the DataUser data");
                    
                }
                return _currentMissionDataSO;
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
            _missioManager = new MissionManager();
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

        public void RegisterDungeon(Dungeon dungeon)
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
            
            StartMission(missionInstance); 
        }

        #endregion

        #region Mission Scheme Methods
        private void StartMission(DungeonMissionInstance missionInstance)
        {
            _currentMissionDataSO = missionInstance.MissionData;

            _missioManager.SetUpMission();


        }

        public void ExitMission(MissionCompleteStatus missionCompleteStatus)
        {
            StartCoroutine(OnExitMission(missionCompleteStatus)); 
        }
        private IEnumerator OnExitMission(MissionCompleteStatus missionCompleteStatus)
        {
            MissionSetupHandler missionSetupHandler = FindObjectOfType<MissionSetupHandler>();
            EventRewardData eventRewardData = DungeonManagerSingleton.Instance.CurrentMissionDataSO.EventRewardData; 
             
            //DisplayMission Complete UI and get reward accordingly 
            yield return _missionCompletetionHandler.ResolveMissionCompleteStatus(missionCompleteStatus);
            SubmitMissionRewardCoroutine(eventRewardData);


            //Handle OnExit for every dungeon componenets
            _missioManager.DungeonPartyHandler.OnExitMission();
            missionSetupHandler.OnExitMission();

            _instance = null;
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

            PersistentActiveDayDatabase.Instance.OnPostPerformSchoolAction(3); 
        }
        private void SubmitMissionRewardCoroutine(EventRewardData eventRewardData)
        {
            eventRewardData.GetAllRewards().SubmitReward(); //;.GetEventRewards(); 

        }
        #endregion

        #region Mission Save/Load Methods
        public object CaptureState()
        {
            //List<object> captured = new List<object>(); 
            //foreach (Dungeon dungeon in _dungeons)
            //{
            //    if (dungeon == null) continue;

            //    var capturedData = dungeon.CaptureData(); 
            //    //TODO : 
            //    //Cast capture data to something else
            //    //save that data into array
            //}
            //return captured;

            int captured = 1; 
            return captured;

        }

        public void RestoreState(object state)
        {
            //foreach (Dungeon dungeon in _dungeons)
            //{
            //    if (dungeon == null) continue;

            //    int someData = 1;
            //    dungeon.RestoreData(someData) ;
             
            //}


            //throw new System.NotImplementedException();
        }

        #endregion
    }
}
