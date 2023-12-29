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
            public List<BaseLocationSelectionCommand> commandList;
            public LocationName location;
        }
        public Pin[] mapPinList;
        [SerializeField]
        private List<Transform> pinTransformList = new List<Transform>();
        private List<PinGUI> pinObject;
        [SerializeField]
        private PinGUI pinTemplate;
        RuntimeDayData dayDataTmp;

        private void Start()
        {
            //LoadPin(PersistentActiveDayDatabase.Instance.GetActiveDayData);
            LoadAllPin(dayDataTmp);
        }

        private void LoadActiveLocation()
        {
            //Load from LocationScript
        }

        //
        private void LoadAllPin(RuntimeDayData dayData)
        {
            //List<LocationSO> locationList = dayData.GetAvailableLocation();
            //LocationName location = locationList[0].LocationName;

            LoadActiveLocation();
            LoadMapBackground();

            //Instantiate PinTemplates
            pinObject = new List<PinGUI>();
            int locationIndex = 0;
            foreach (Pin pin in mapPinList)
            {
                switch (pin.location)
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
                newPin.Init(pin.locationIcon);
                pinObject.Add(newPin);
            }


            //List<BaseLocationSelectionCommand> commandList = dayData.FactorizeCommandActionWithinLocation(locationList[0]);

            //foreach (BaseLocationSelectionCommand command in commandList)
            //{
            //    Sprite news = command.GetActionIconSprite;
            //    command.ExecuteCommand();
            //}
        }

        private void LoadMapBackground()
        {
            //Need Day/Night time data
        }
    }
}
