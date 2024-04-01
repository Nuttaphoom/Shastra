using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring 
{
    public class RuntimeDungeon
    {
        List<MissionDataSO> _missionsOnThisDungeon;
        DungeonDataSO _dungeonDataSO;

        private List<DungeonMissionInstance> _dungeonMissionInstance = new List<DungeonMissionInstance>();

        public RuntimeDungeon(List<MissionDataSO> missionsOnThisDungeon, DungeonDataSO dungeonDataSO)
        {
            _missionsOnThisDungeon = missionsOnThisDungeon; 
            _dungeonDataSO = dungeonDataSO; 

        } 

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


    }

   
}
