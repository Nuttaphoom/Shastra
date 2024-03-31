using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu (fileName = "MissionDataSO", menuName = "ScriptableObject/DungeonMission/MissionDataSO")]
    public class MissionDataSO : ScriptableObject
    {
        [SerializeField]
        private MissionNodeEnvironment _missionNodeEnvironment;

        #region GETTER
        public MissionNodeEnvironment MissionNodeEnvironment { get { return _missionNodeEnvironment; }   }

        #endregion
    }
}
