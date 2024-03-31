using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class Dungeon : MonoBehaviour, ISaveable
    {
        [SerializeField]
        private DungeonDataSO _dungeonData;

        [SerializeField]
        private List<MissionDataSO> _missionsOnThisDungeon;

        private List<DungeonMissionInstance> _dungeonMissionInstance = new List<DungeonMissionInstance>(); 

        /// <summary>
        /// TEMP function use for testing
        /// </summary>
        public DungeonMissionInstance GetSelectMission
        {
            get
            {
                return _dungeonMissionInstance[0]; 
            }
        }

        public void SelectThisDungeon()
        {
            foreach (MissionDataSO missionDataSO in _missionsOnThisDungeon)
            {
                _dungeonMissionInstance.Add(new DungeonMissionInstance(missionDataSO));

            }

            //Display list of DungeonMissionInstance for player to select 
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
