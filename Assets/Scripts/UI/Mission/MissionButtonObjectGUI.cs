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
        [SerializeField] private Image selectIcon;
        [SerializeField] private Image missionIcon;
        [SerializeField] private TextMeshProUGUI missionName;
        [SerializeField] private TextMeshProUGUI missionDetail;
        [SerializeField] private TextMeshProUGUI missionTime;

        public void Init(RuntimeDungeon dungeon, DungeonMissionInstance mission)
        {
            UnSelectThisMission();
            missionButton.onClick.AddListener(() => DungeonManagerSingleton.Instance.LoadSelectedMission(mission));      
        }

        public void SelectThisMission()
        {
            missionButton.GetComponent<Image>().color = Color.yellow;
        }
        public void UnSelectThisMission()
        {
            missionButton.GetComponent<Image>().color = Color.white;
        }
        public void EnterTheMission()
        {
            missionButton.onClick.Invoke();
        }
    }
}
