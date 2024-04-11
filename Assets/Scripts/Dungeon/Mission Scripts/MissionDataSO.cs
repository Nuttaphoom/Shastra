using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu (fileName = "MissionDataSO", menuName = "ScriptableObject/DungeonMission/MissionDataSO")]
    public class MissionDataSO : ScriptableObject
    {
        [SerializeField]
        private EventRewardData _eventRewardData ; 

        [SerializeField]
        private MissionNodeEnvironment _missionNodeEnvironment;

        [SerializeField]
        private DescriptionBaseField _missionDescription; 
        #region GETTERS
        public MissionNodeEnvironment MissionNodeEnvironment { get { return _missionNodeEnvironment; }   }
        public EventRewardData EventRewardData { get { return _eventRewardData  ; } }

        public DescriptionBaseField MissionDescription 
        { 
            get 
            { 
                return _missionDescription; 
            } 
        } 
        #endregion
    }
}
