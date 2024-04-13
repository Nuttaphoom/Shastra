using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class MissionBookSetup : MonoBehaviour
    {
        private List<MissionButtonObjectGUI> missionButtonList = new List<MissionButtonObjectGUI>();
        private int selectingIndex;
        [SerializeField] private MissionButtonObjectGUI missionButtonTemplate;
        [SerializeField] private GameObject verticalLayout;
        //[SerializeField] private 
        
        public void Init(RuntimeDungeon dungeon)
        {
            selectingIndex = 0;
            if (missionButtonList.Count > 0)
            {
                foreach (var buttonObject in missionButtonList)
                {
                    Destroy(buttonObject.gameObject);
                }

                missionButtonList.Clear();
            }
            foreach (DungeonMissionInstance mission in dungeon.AllDungeonMissionInstance)
            {
                MissionButtonObjectGUI newButton = Instantiate(missionButtonTemplate, verticalLayout.transform);
                newButton.Init(dungeon, mission);
                newButton.UnSelectThisMission();
                missionButtonList.Add(newButton);
            }
            missionButtonList[selectingIndex].SelectThisMission();
            missionButtonTemplate.gameObject.SetActive(false);
            Debug.Log(missionButtonList.Count);
        }

        public void NextMissionIndex()
        {
            if (selectingIndex < missionButtonList.Count - 1)
            {
                //foreach (MissionButtonObjectGUI obj in missionButtonList)
                //{
                //    obj.UnSelectThisMission();
                //}
                missionButtonList[selectingIndex].UnSelectThisMission();
                selectingIndex++;
                missionButtonList[selectingIndex].SelectThisMission();
            }
        }

        public void PrevMissionIndex()
        {
            if (selectingIndex > 0)
            {
                //foreach (MissionButtonObjectGUI obj in missionButtonList)
                //{
                //    obj.UnSelectThisMission();
                //}
                missionButtonList[selectingIndex].UnSelectThisMission();
                selectingIndex--;
                missionButtonList[selectingIndex].SelectThisMission();
            }
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                PrevMissionIndex();
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                NextMissionIndex();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                missionButtonList[selectingIndex].EnterTheMission();
                
            }
        }
    }
}
