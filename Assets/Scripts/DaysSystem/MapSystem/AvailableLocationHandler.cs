using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    [Serializable]
    public class AvailableLocationHandler
    {
        [Serializable]
        public struct LocationInfo
        {
            [SerializeReference]
            public LocationSO Location;

            [SerializeField]
            private List<LocationActionCommandRegister> _selectionCommandCenter;

            [SerializeField]
            private bool EnableAtMorning ;
            [SerializeField]
            private bool EnableAtNoon;
            [SerializeField]
            private bool EnableAtNight; 

            

            public  List<BaseLocationActionCommand>  FactorizeCommandLocation  
            {
                get {
                    List<BaseLocationActionCommand> ret = new List<BaseLocationActionCommand>();
                    foreach (var commandCenter in _selectionCommandCenter)
                        ret.Add(commandCenter.FactorizeLocationSelectionCommand( ) ); 
                    
                    return ret; 
                }
            }

            public bool IsCorrectLocation(LocationSO _locationData)
            {
                return (_locationData.LocationName == Location.LocationName) ;
            }

            public bool EnableAtGivenDayTime(EDayTime daytime)
            {
                switch (daytime)
                {
                    case (EDayTime.Morning):
                        if (EnableAtMorning)
                            return true; 
                        break;
                    case (EDayTime.Noon):
                        if (EnableAtNoon)
                            return true;
                        break;
                    case (EDayTime.Night):
                        if (EnableAtNight)
                            return true;
                        break;

                    default:
                        break; 

                }

                return false; 
            }
        }

        [SerializeField]
        public List<LocationInfo> _locationInfos;  

        public List<LocationSO> GetAvailableLocation(EDayTime dayTime)
        {
            List<LocationSO> locations = new List<LocationSO>() ;//= _locationInfos.Select(info => info.GetLocationSO).ToList();
            foreach (LocationInfo info in _locationInfos) {
                if (! info.EnableAtGivenDayTime(dayTime) ) 
                    continue;

                locations.Add(info.Location); 
            }
            return locations;
        }

        public List<BaseLocationActionCommand> FactorizeCommandActionWithinLocation(LocationSO location)
        {
            List<BaseLocationActionCommand> ret = new List<BaseLocationActionCommand>(); 
            foreach (var info in _locationInfos)
            {
                if (! info.IsCorrectLocation(location))
                    continue;

                if (ret.Count > 0)
                    throw new Exception("Multiple matching location with " + info.Location.LocationName);


                foreach (var command in info.FactorizeCommandLocation)
                {
                    ret.Add(command);
                }
            }

            if (ret.Count > 0)
                return ret; 

            throw new Exception("Location " + location + " is not in the day database"); 

        }
    }

}
