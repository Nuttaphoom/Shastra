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
        [SerializeField] private TextMeshProUGUI missionOrderNum;

        public void Init(RuntimeDungeon dungeon, DungeonMissionInstance mission, int order)
        {
            missionOrderNum.text = order.ToString();
            UnSelectThisMission();
            missionButton.onClick.AddListener(() => DungeonManagerSingleton.Instance.LoadSelectedMission(mission));      
        }

        public void SelectThisMission()
        {
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
