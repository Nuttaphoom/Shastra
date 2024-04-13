using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Vanaring.DungeonManagerSingleton;

namespace Vanaring 
{
    public class RuntimeDungeon   
    {
        List<MissionDataSO> _missionsOnThisDungeon; 
        DungeonDataSO _dungeonDataSO;

        private List<DungeonMissionInstance> _dungeonMissionInstance = new List<DungeonMissionInstance>();
        #region GETTER

        public DungeonDataSO DungeonDataSO => _dungeonDataSO;
        public List<DungeonMissionInstance> AllDungeonMissionInstance => _dungeonMissionInstance;

        #endregion
        public RuntimeDungeon(List<MissionDataSO> missionsOnThisDungeon, DungeonDataSO dungeonDataSO)
        {
            _missionsOnThisDungeon = missionsOnThisDungeon; 
            _dungeonDataSO = dungeonDataSO;

            foreach (MissionDataSO missionDataSO in _missionsOnThisDungeon)
            {
                _dungeonMissionInstance.Add(new DungeonMissionInstance(missionDataSO));
            }


        } 

        //TEMP This function use for testing only
        public DungeonMissionInstance GetSelectMission(int index)
        {
               
            return _dungeonMissionInstance[index];
             
        }

        public void SelectThisDungeon()
        {
            //Display list of DungeonMissionInstance for player to select 
        }

        #region Save Load Methods
        public void RestoreDungeonData(RuntimeDungeonSaveLoadData runtimeDungeonSaveLoadData)
        {
            foreach (var missionInstance in _dungeonMissionInstance)
            {
                string missionName = missionInstance.MissionData.MissionDescription.FieldName;
                
                if (! runtimeDungeonSaveLoadData.MissionNamePair.ContainsKey(missionName))
                    continue ;


                missionInstance.RestoreMissionData(runtimeDungeonSaveLoadData.MissionNamePair[missionName]) ; 
            }
        }

        public RuntimeDungeonSaveLoadData CaptureDungeonData()
        {
            RuntimeDungeonSaveLoadData ret = new RuntimeDungeonSaveLoadData()
            { MissionNamePair = new Dictionary<string, RuntimeMissionSaveLoadData>(), };
            foreach (var missionInstance in _dungeonMissionInstance)
            {
                string missionName = missionInstance.MissionData.MissionDescription.FieldName;

                ret.MissionNamePair.Add(missionName, missionInstance.CaptureMissionData() ) ;

            }

            return ret; 
        }



        #endregion

    }

    


}
