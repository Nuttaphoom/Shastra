using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class LocationSO : ScriptableObject
    {
        [SerializeField]
        private LocationName _locationName; 

        public LocationName LocationName { get { return _locationName; } }

    }

    public enum LocationName
    {
        Front_Gate, 
        Library
    }
}
