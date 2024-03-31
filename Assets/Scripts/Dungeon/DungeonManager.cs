using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Vanaring
{
    public class DungeonManager : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private List<Dungeon> _dungeons;

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
        private void LoadSelectedMission(DungeonMissionInstance missionInstance)
        {
            //Visually dispaly confirm selection UI 

            PersistentSceneLoader.Instance.CreateLoaderDataUser<DungeonMissionInstance>("DungeonMissionInstanceFromDungeonManager", missionInstance) ;
            PersistentSceneLoader.Instance.LoadGeneralScene( PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(_base_missionScene) ) ; 

        }



        public object CaptureState()
        {
            throw new System.NotImplementedException();
        }

        public void RestoreState(object state)
        {
            throw new System.NotImplementedException();
        }

       
    }
}
