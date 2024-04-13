using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class MissionBookSetup : MonoBehaviour
    {
        private List<DungeonMissionInstance> missionList = new List<DungeonMissionInstance>();
        //[SerializeField] private 
        
        public void Init(List<DungeonMissionInstance> missionList)
        {
            foreach (DungeonMissionInstance mission in missionList)
            {
                //AllDungeonMissionInstance
            }
        }
    }
}
