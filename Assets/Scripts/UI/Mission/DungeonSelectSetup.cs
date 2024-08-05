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

        //[SerializeField] private Button closeButton;
        [SerializeField] private Button initialMissionButton;

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return null;
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            //Debug.Log(DungeonManagerSingleton.Instance.GetAllActiveDungeon.Count);
            dungeonList = DungeonManagerSingleton.Instance.GetAllActiveDungeon;
           
            int dungeonIndex = 0;
            yield return new WaitForSeconds(1.0f);
            foreach (RuntimeDungeon dungeon in dungeonList)
            {
                dungeonButton[dungeonIndex].onClick.AddListener(() => LoadAllMission(dungeon));
                Button button = dungeonButton[dungeonIndex];
                //dungeonButton[dungeonIndex].onClick.AddListener(delegate { PersistentButtonSelector.Instance.AddPreviousButton(button); } );
                dungeonIndex++;
            }
            //template.gameObject.SetActive(false);
            //closeButton.onClick.AddListener(PersistentButtonSelector.Instance.SelectPreviousButton);
            //closeButton.onClick.AddListener(PersistentButtonSelector.Instance.RemoveLastPreviousButton);
            yield return null;
        }

        private void LoadAllMission(RuntimeDungeon dungeon)
        {
            missionBook.gameObject.SetActive(true);
            missionBook.Init(dungeon);
            //dungeon.SelectThisDungeon();
            //DungeonManagerSingleton.Instance.LoadSelectedMission(dungeon.GetSelectMission(0));
        }

        public void SelectInitialButton()
        {
            initialMissionButton.Select();
        }

    }
}
