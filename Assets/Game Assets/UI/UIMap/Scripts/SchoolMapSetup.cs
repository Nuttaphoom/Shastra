using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Vanaring
{
    public class SchoolMapSetup : MonoBehaviour
    {
        [System.Serializable]
        public struct Pin
        {
            public string name;
            public Sprite locationIcon;
            public List<BaseLocationActionCommand> commandList;
            public LocationName location;
        }
        public Pin[] mapPinList;
        [SerializeField]
        private List<Transform> pinTransformList = new List<Transform>();
        private List<PinGUI> pinObject;
        [SerializeField]
        private PinGUI pinTemplate;
        RuntimeDayData dayDataTmp;
        private List<RuntimeLocation> availableLocationList;

        private void Awake()
        {
            availableLocationList = PersistentActiveDayDatabase.Instance.GetActiveDayData.GetAvailableLocation();
            //LoadPin(PersistentActiveDayDatabase.Instance.GetActiveDayData);
            LoadAllPin(dayDataTmp);
        }
        private void LoadAllPin(RuntimeDayData dayData)
        {
            LoadMapBackground();

            //Instantiate PinTemplates
            pinObject = new List<PinGUI>();
            int locationIndex = 0;
            foreach (RuntimeLocation location in availableLocationList)
            {
                switch (location.LocationName)
                {
                    case LocationName.Front_Gate:
                        locationIndex = 0;
                        break;
                    case LocationName.Library:
                        locationIndex = 1;
                        break;
                    case LocationName.Stadium:
                        locationIndex = 2;
                        break;
                    case LocationName.Cottage:
                        locationIndex = 3;
                        break;
                    default:
                        Debug.LogError("Non setup tranform index");
                        break;
                }
                PinGUI newPin = Instantiate(pinTemplate, pinTransformList[locationIndex]);         
                newPin.Init(location);
                pinObject.Add(newPin);
            }
        }

        private void LoadMapBackground()
        {
            //Need Day/Night time data
        }
    }
}
