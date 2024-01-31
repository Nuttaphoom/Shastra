using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

namespace Vanaring
{
    public class PersistentActiveDayDatabase : PersistentInstantiatedObject<PersistentActiveDayDatabase>
    {
        [SerializeReference]
        private RuntimeDayData _runtimeDayData = null; 

        private DayProgressionHandler _dayProgressionHandler;

        public void SetActiveDayData(RuntimeDayData value)
        {
            _runtimeDayData = value;
            GetDayProgressionHandler.NewDayBegin(); 
        }



        #region Getter 
        public DayProgressionHandler GetDayProgressionHandler
        {
            get
            {
                if (_dayProgressionHandler == null)
                    _dayProgressionHandler = new DayProgressionHandler();

                return _dayProgressionHandler;
            }
        }

        public RuntimeDayData GetActiveDayData
        {
            get
            {
                if (_runtimeDayData == null)
                    throw new System.Exception("RuntimeDayData is null");

                return _runtimeDayData; 
            }
        }
        #endregion
    }
}