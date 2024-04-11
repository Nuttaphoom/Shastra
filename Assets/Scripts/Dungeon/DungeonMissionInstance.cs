using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vanaring.DungeonManagerSingleton;

namespace Vanaring
{
    public class DungeonMissionInstance   
    {
        private MissionDataSO _missionDataSO;

        private bool _hasVisisteThisMission = false; 

        #region GETTER 
        public MissionDataSO MissionData {  get { return _missionDataSO; } }    

        
        #endregion
        public DungeonMissionInstance(MissionDataSO missionData) {
            _missionDataSO = missionData; 
        }

        public void OnStartMission()
        {
            //if (_hasVisisteThisMission)
            //    Debug.Log("this mission has been visisted");
            //else
            //    Debug.Log("This mission has NEVER been visisted");

            //Debug.Log("set visist status is true");
            _hasVisisteThisMission = true; 
            //This function is for testing only 
        }

        public RuntimeMissionSaveLoadData CaptureMissionData()
        {

            return new RuntimeMissionSaveLoadData() { HasVisiteThisMission = _hasVisisteThisMission  } ;
        }

        public void RestoreMissionData(RuntimeMissionSaveLoadData state)
        {
            _hasVisisteThisMission =  state.HasVisiteThisMission;
        }
    }
}
