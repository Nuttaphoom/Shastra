using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class DungeonSelectSetup : MonoBehaviour, ISceneLoaderWaitForSignal
    {
        private List<RuntimeDungeon> dungeonList = new List<RuntimeDungeon>();
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
            yield return new WaitForSeconds(1.0f);
            foreach (RuntimeDungeon dungeon in dungeonList)
            {
                dungeon.SelectThisDungeon();
                DungeonManagerSingleton.Instance.LoadSelectedMission(_dungeons.GetSelectMission(0));
                //GameObject newDungeon = Instantiate(template)
                //GameObject newDungeon = Instantiate(template, hrzt.transform);
            }
            template.gameObject.SetActive(false);
            yield return null;
        }
    }
}
