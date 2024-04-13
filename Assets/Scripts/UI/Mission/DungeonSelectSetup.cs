using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class DungeonSelectSetup : MonoBehaviour, ISceneLoaderWaitForSignal
    {
        private List<RuntimeDungeon> dungeonList = new List<RuntimeDungeon>();
        [SerializeField] private MissionBookSetup missionBook;
        [SerializeField] private List<Button> dungeonButton = new List<Button>();
        [SerializeField] private GameObject template;
        [SerializeField] private GameObject hrzt;
        private void Start()
        {
            
            //Debug.Log(dungeonList.Count);
        }

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return null;
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            dungeonList = DungeonManagerSingleton.Instance.GetAllActiveDungeon;
            int dungeonIndex = 0;
            yield return new WaitForSeconds(1.0f);
            foreach (RuntimeDungeon dungeon in dungeonList)
            {
                dungeonButton[dungeonIndex].onClick.AddListener(() => LoadAllMission(dungeon));
                dungeonIndex++;
            }
            //template.gameObject.SetActive(false);
            yield return null;
        }

        private void LoadAllMission(RuntimeDungeon dungeon)
        {
            missionBook.gameObject.SetActive(true);
            missionBook.Init(dungeon);
            //dungeon.SelectThisDungeon();
            //DungeonManagerSingleton.Instance.LoadSelectedMission(dungeon.GetSelectMission(0));
        }

        
    }
}
