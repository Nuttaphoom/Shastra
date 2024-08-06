using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class MissionButtonObjectGUI : MonoBehaviour
    {
        [SerializeField] private Button missionButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Image selectIcon;
        [SerializeField] private Image missionIcon;
        [SerializeField] private TextMeshProUGUI missionName;
        [SerializeField] private TextMeshProUGUI missionNameTitle;
        [SerializeField] private TextMeshProUGUI missionDetail;
        [SerializeField] private TextMeshProUGUI missionTime;
        [SerializeField] private TextMeshProUGUI missionOrderNum;

        public Button MissionButton
        {
            get
            {
                return missionButton;
            }
        }
        private RuntimeDungeon dungeon;
        private int order;

        public void Init(RuntimeDungeon dungeon, DungeonMissionInstance mission, int order)
        {
            this.dungeon = dungeon;
            this.order = order;
            missionOrderNum.text = order.ToString();
            UnSelectThisMission();
            missionButton.onClick.AddListener(() => DungeonManagerSingleton.Instance.LoadSelectedMission(mission));

            missionName.text = dungeon.DungeonDataSO.GetMissionDataSOes[order-1].MissionDescription.FieldName;
            missionNameTitle.text = dungeon.DungeonDataSO.GetMissionDataSOes[order-1].MissionDescription.FieldName;
            missionDetail.text = dungeon.DungeonDataSO.GetMissionDataSOes[order-1].MissionDescription.FieldDescription;

            // Setting Button Navigation with input control
            //Navigation NewNav = new Navigation();
            //NewNav.mode = Navigation.Mode.Explicit;
            //NewNav.selectOnDown = missionButton;
            //closeButton.navigation = NewNav;
        }

        public void UpdateMissionDetail()
        {
            missionName.text = dungeon.DungeonDataSO.GetMissionDataSOes[order - 1].MissionDescription.FieldName;
            missionNameTitle.text = dungeon.DungeonDataSO.GetMissionDataSOes[order - 1].MissionDescription.FieldName;
            missionDetail.text = dungeon.DungeonDataSO.GetMissionDataSOes[order - 1].MissionDescription.FieldDescription;
        }

        public void SelectThisMission()
        {
            missionButton.Select();
            UpdateMissionDetail();
            missionButton.GetComponent<Image>().color = Color.yellow;
            selectIcon.gameObject.SetActive(true);
        }
        public void UnSelectThisMission()
        {
            missionButton.GetComponent<Image>().color = Color.white;
            selectIcon.gameObject.SetActive(false);
        }
        public void EnterTheMission()
        {
            missionButton.onClick.Invoke();
        }
    }
}
