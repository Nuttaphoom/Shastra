using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class MissionBookSetup : MonoBehaviour, IInputReceiver
    {
        private List<MissionButtonObjectGUI> missionButtonList = new List<MissionButtonObjectGUI>();
        private int selectingIndex;
        [SerializeField] private MissionButtonObjectGUI missionButtonTemplate;
        [SerializeField] private GameObject verticalLayout;

        [SerializeField]
        private Button mapbutton; 

        private void OnEnable()
        {
            CentralInputReceiver.Instance.AddInputReceiverIntoStack(this);
        }

        private void OnDisable()
        {
            CentralInputReceiver.Instance.RemoveInputReceiverIntoStack(this);
        }

        public void ReceiveKeys(InputCode key)
        {
            if (key == InputCode.DeSelect)
            {
                mapbutton.onClick?.Invoke();
            }
            if (key == InputCode.Up)
            {
                PrevMissionIndex();
            }
            if (key == InputCode.Down)
            {
                NextMissionIndex();
            }
            if (key == InputCode.Select)
            {
                //missionButtonList[selectingIndex].EnterTheMission();
            }
        }

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
            int orderIndex = 1;
            foreach (DungeonMissionInstance mission in dungeon.AllDungeonMissionInstance)
            {
                MissionButtonObjectGUI newButton = Instantiate(missionButtonTemplate, verticalLayout.transform);
                newButton.Init(dungeon, mission, orderIndex);
                newButton.gameObject.SetActive(true);
                newButton.UnSelectThisMission();
                missionButtonList.Add(newButton);
                orderIndex++;
            }
            missionButtonList[selectingIndex].SelectThisMission();
            missionButtonTemplate.gameObject.SetActive(false);
            Navigation NewNav = new Navigation();
            NewNav.mode = Navigation.Mode.Explicit;
            NewNav.selectOnLeft = missionButtonList[0].MissionButton;
            mapbutton.navigation = NewNav;

            mapbutton.onClick.AddListener(delegate { PersistentButtonSelector.Instance.SelectPreviousButton(); });
            //Debug.Log(missionButtonList.Count);
        }

        public void NextMissionIndex()
        {
            if (selectingIndex < missionButtonList.Count - 1)
            {
                Debug.Log(selectingIndex);
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
                Debug.Log(selectingIndex);
                //foreach (MissionButtonObjectGUI obj in missionButtonList)
                //{
                //    obj.UnSelectThisMission();
                //}
                missionButtonList[selectingIndex].UnSelectThisMission();
                selectingIndex--;
                missionButtonList[selectingIndex].SelectThisMission();
            }
            
        }
    }
}
