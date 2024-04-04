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
    public class DungeonManagerSingleton : MonoBehaviour, ISaveable 
    {
        [SerializeField]
        private List<RuntimeDungeon> _dungeons;

        [SerializeField] 
        private AssetReferenceT<EssentialSceneDataSO> _base_missionScene ;

        private static DungeonManagerSingleton _instance; 
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

        private void Awake()
        {
            _instance  = this; 
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

            StartMission(); 
        }

        #endregion

        #region Mission Scheme Methods
        private void StartMission()
        {
            MissionManagerSingleton.Instance.SetUpMission(); 
        }

        public void OnExitMission()
        {
            Debug.Log("OnExitMission");
            _instance = null;
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

            PersistentActiveDayDatabase.Instance.OnPostPerformSchoolAction(3); 
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
