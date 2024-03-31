using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class DungeonMissionInstance  
    {
        private MissionDataSO _missionDataSO;

        #region GETTER 
        public MissionDataSO MissionData {  get { return _missionDataSO; } }    

        #endregion
        public DungeonMissionInstance(MissionDataSO missionData) {
            _missionDataSO = missionData; 
        } 
    }
}
