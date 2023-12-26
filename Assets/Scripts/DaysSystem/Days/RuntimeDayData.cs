using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class RuntimeDayData    
    {
        private DayDataSO _dayDataSO;

        public RuntimeDayData(ref DayDataSO dayData) { 
            _dayDataSO = dayData; 
        }

        public List<LocationSO> GetAvailableLocation()
        {
            return _dayDataSO.GetAvailableLocation();  
        }

        public List<BaseLocationSelectionCommand> FactorizeCommandActionWithinLocation(LocationSO location)
        {
            return _dayDataSO.FactorizeCommandActionWithinLocation(location);
        }
    }
}
