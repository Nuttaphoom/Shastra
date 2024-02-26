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
        public RuntimeDayData(DayDataSO dayData) {

            _runtimeLocations = new Dictionary<EDayTime, List<RuntimeLocation>>() ;
            
            foreach (EDayTime time in Enum.GetValues(typeof(EDayTime))) {
                foreach (LocationSO locationSO in dayData.GetAvailableLocation(time))
                {
                    List<BaseLocationActionCommand> actionWithinLocation = dayData.FactorizeCommandActionWithinLocation(locationSO);
                    var runtimeLocation = locationSO.FactorizeRuntimeLocation(actionWithinLocation);
                    if (!_runtimeLocations.ContainsKey(time))
                        _runtimeLocations.Add(time, new List<RuntimeLocation>() ); 

                    _runtimeLocations[time].Add(runtimeLocation);
                }
            }
            

            _currentTime = EDayTime.Morning; 
        }

        public void ProgressCurrentTime()
        { Debug.Log("Before Progress : " + _currentTime); 
            _currentTime = (EDayTime)(_currentTime + 1);
            Debug.Log("After Progress : " + _currentTime);
        }

        public List<RuntimeLocation> GetAvailableLocationAccordingToDayTime()
        {
            ColorfulLogger.LogWithColor("_runtimeLocations[" + _currentTime + "].count  : " + _runtimeLocations[_currentTime].Count, Color.green);
            return _runtimeLocations[_currentTime]; 
        }

        
    
    }
}
