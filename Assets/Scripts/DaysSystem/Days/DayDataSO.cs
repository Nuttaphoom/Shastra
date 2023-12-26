using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vanaring.AvailableLocationHandler;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "DayDataSO", menuName = "ScriptableObject/DayDataSO")]
    public class DayDataSO : ScriptableObject
    {
        [SerializeField]
        private AvailableLocationHandler _availableLocationHandler ;

        public List<LocationSO> GetAvailableLocation()
        {
            return _availableLocationHandler.GetAvailableLocation(); //.Select(info => info.GetLocationSO).ToList();
        }

        public List<BaseLocationSelectionCommand> FactorizeCommandActionWithinLocation(LocationSO location)
        {
            return _availableLocationHandler.FactorizeCommandActionWithinLocation(location);
        }
    }


     
}
