using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
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
            private LocationSO _location;

            [SerializeField]
            private List<LocationActionCommandRegister> _selectionCommandCenter;

            public LocationSO GetLocationSO
            {
                get
                {
                    return _location; 
                }


            }

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
                return (_locationData.LocationName == GetLocationSO.LocationName) ;
            }
        }

        [SerializeField]
        public List<LocationInfo> _locationInfos;  

        public List<LocationSO> GetAvailableLocation()
        { 
            return _locationInfos.Select(info => info.GetLocationSO).ToList();
        }

        public List<BaseLocationActionCommand> FactorizeCommandActionWithinLocation(LocationSO location)
        {
            List<BaseLocationActionCommand> ret = new List<BaseLocationActionCommand>(); 
            foreach (var info in _locationInfos)
            {
                if (! info.IsCorrectLocation(location))
                    continue;

                if (ret.Count > 0)
                    throw new Exception("Multiple matching location with " + info.GetLocationSO.LocationName);


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
