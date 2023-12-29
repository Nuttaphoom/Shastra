using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "LocationSO", menuName = "ScriptableObject/DaySystem/LocationSO")]
    public class LocationSO : ScriptableObject
    {
        [SerializeField]
        private LocationName _locationName;

        [SerializeField]
        private LoadLocationMenuCommand _locationLoadCommand; 

        public LocationName LocationName { get { return _locationName; } }

    }

    public enum LocationName
    {
        Front_Gate, 
        Library
    }
}
