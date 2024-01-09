using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [SerializeField] 
    public class RuntimeDayData
    {

        private List<RuntimeLocation> _runtimeLocations;

        private EDayTime _currentTime; 
        public RuntimeDayData(DayDataSO dayData) {

            _runtimeLocations = new List<RuntimeLocation>(); 

            foreach (var locationSO in dayData.GetAvailableLocation())
            {
                List<BaseLocationActionCommand> actionWithinLocation = dayData.FactorizeCommandActionWithinLocation(locationSO); 
                var runtimeLocation = locationSO.FactorizeRuntimeLocation(actionWithinLocation);
                _runtimeLocations.Add(runtimeLocation) ;
            }

            _currentTime = EDayTime.Morning; 
        }

        public void ProgressCurrentTime()
        {
            _currentTime = (EDayTime) (_currentTime + 1) ;
        }

        public List<RuntimeLocation> GetAvailableLocation()
        {
            return _runtimeLocations; 
        }

        public enum EDayTime
        {
            Morning = 0, 
            Eveing = 1, 
            Night = 2 
        }
    
    }
}
