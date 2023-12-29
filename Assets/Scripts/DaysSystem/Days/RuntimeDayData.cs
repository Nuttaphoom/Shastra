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

            foreach (var locationSO in dayData.GetAvailableLocation())
            {
                List<BaseLocationActionCommand> actionWithinLocation = dayData.FactorizeCommandActionWithinLocation(locationSO); 
                var runtimeLocation = locationSO.FactorizeRuntimeLocation(actionWithinLocation);
                _runtimeLocations.Add(runtimeLocation) ;
            }
        }

        public List<RuntimeLocation> GetAvailableLocation()
        {
            return _runtimeLocations; 
        }

    
    }
}
