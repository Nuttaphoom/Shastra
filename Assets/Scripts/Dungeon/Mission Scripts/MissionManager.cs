using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring
{
 
    public class MissionManager 
    {
        private MissionPartyHandler _dungeonPartyHandler;


        #region GETTER

        public MissionPartyHandler DungeonPartyHandler
        {
            get
            {
                if (_dungeonPartyHandler == null)
                    throw new Exception("_dungeonPartyHandler hasn't never been assigned");
                return _dungeonPartyHandler; 
            }
        }
        #endregion

        #region Public Method 
        /// <summary>
        /// Call only once when player "select and enter mission from mission selection menu" 
        /// </summary>
        public void SetUpMission()
        {
            _dungeonPartyHandler = new MissionPartyHandler ();

            _dungeonPartyHandler.SetUpRuntimeParty();

        }

        
        #endregion
        

      


    }
}
