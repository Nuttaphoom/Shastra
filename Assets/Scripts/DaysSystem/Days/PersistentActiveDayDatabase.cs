using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using Vanaring.Assets.Scripts.Utilities;

namespace Vanaring
{
    public class PersistentActiveDayDatabase : PersistentInstantiatedObject<PersistentActiveDayDatabase>
    {
        [SerializeReference]
        private RuntimeDayData runtimeDayData = null;

    
        private DailyActionParticipationHandler _dailyActionParticipationHandler ; 

        public RuntimeDayData GetActiveDayData
        {
            get
            {
                if (runtimeDayData == null)
                    throw new System.Exception("runtimeDayData is null"); 

                return runtimeDayData;
            }
        }

        public void SetActiveDayData(RuntimeDayData value)
        {
            runtimeDayData = value;

            _dailyActionParticipationHandler = new DailyActionParticipationHandler(); 
        }
    }
}