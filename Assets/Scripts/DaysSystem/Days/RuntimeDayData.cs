using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [SerializeField] 
    public class RuntimeDayData    
    {
        private List<RuntimeLocation> _runtimeLocations; 

        public RuntimeDayData( DayDataSO dayData) {
            _runtimeLocations = new List<RuntimeLocation>(); 

            foreach (var v in dayData.GetAvailableLocation())
            {
                Debug.Log(" count : " + dayData.FactorizeCommandActionWithinLocation(v).Count); 
                _runtimeLocations.Add(v.FactorizeRuntimeLocation(dayData.FactorizeCommandActionWithinLocation(v))) ;
            }
        }

        public List<RuntimeLocation> GetAvailableLocation()
        {
            return _runtimeLocations; 
        }

    
    }
}
