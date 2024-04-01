using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Vanaring
{
    public class DungeonManager : MonoBehaviour, ISaveable 
    {
        [SerializeField]
        private List<RuntimeDungeon> _dungeons;

        [SerializeField] 
        private AssetReferenceT<EssentialSceneDataSO> _base_missionScene ;


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) {
                _dungeons[0].SelectThisDungeon();
                LoadSelectedMission(_dungeons[0].GetSelectMission) ;
            }
        }

        public void RegisterDungeon(Dungeon dungeon)
        {
            if (_dungeons == null) 
                _dungeons = new List<RuntimeDungeon>() ; 
            
            _dungeons.Add(dungeon.FactorizeRuntimeDungeon());
        }
        private void LoadSelectedMission(DungeonMissionInstance missionInstance)
        {
            //Visually dispaly confirm selection 
            PersistentSceneLoader.Instance.CreateLoaderDataUser<DungeonMissionInstance>("DungeonMissionInstanceFromDungeonManager", missionInstance) ;
            PersistentSceneLoader.Instance.LoadGeneralScene( PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(_base_missionScene) ) ;

            StartMission(); 
        }

        private void StartMission()
        {
            MissionManagerSingleton.Instance.SetUpMission(); 
        }

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
            throw new System.NotImplementedException();


        }

        public void RestoreState(object state)
        {
            //foreach (Dungeon dungeon in _dungeons)
            //{
            //    if (dungeon == null) continue;

            //    int someData = 1;
            //    dungeon.RestoreData(someData) ;
             
            //}
            throw new System.NotImplementedException();
        }

       
    }
}
