using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vanaring
{
    [SerializeField] 
    public class RuntimeDayData
    {
        private EDayTime _currentTime;

        private Dictionary<EDayTime, List<RuntimeLocation>> _runtimeLocations;

        #region GETTER 
        public EDayTime GetCurrentDayTime
        {
           get
            {
                return _currentTime; 
            }
        }
        #endregion
        public RuntimeDayData(DayDataSO dayData) {

            _runtimeLocations = new Dictionary<EDayTime, List<RuntimeLocation>>() ;
            
            foreach (EDayTime time in Enum.GetValues(typeof(EDayTime))) {
                foreach (LocationSO locationSO in dayData.GetAvailableLocation(time))
                {
                    List<BaseLocationActionCommand> actionWithinLocation = dayData.FactorizeCommandActionWithinLocation(locationSO, time);
                    var runtimeLocation = locationSO.FactorizeRuntimeLocation(actionWithinLocation);
                    if (!_runtimeLocations.ContainsKey(time))
                        _runtimeLocations.Add(time, new List<RuntimeLocation>() ); 

                    _runtimeLocations[time].Add(runtimeLocation);
                }
            }
            

            _currentTime = EDayTime.Morning; 
        }

        public void ProgressCurrentTime()
        { 
            _currentTime = (EDayTime)(_currentTime + 1);
            
        }

        public List<RuntimeLocation> GetAvailableLocationAccordingToDayTime()
        {
            return _runtimeLocations[_currentTime]; 
        }

        
    
    }
}
