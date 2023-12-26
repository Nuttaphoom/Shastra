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
            public PinGUI _pinLocation;
        }
        public Pin[] mapPinList;
        [SerializeField]
        private List<Transform> pinTransformList = new List<Transform>();
        private List<PinGUI> pinObject;
        [SerializeField]
        private PinGUI pinTemplate;

        private void Start()
        {
            LoadPin(PersistentActiveDayDatabase.Instance.GetActiveDayData);
        }

        private void LoadPin(RuntimeDayData dayData)
        {
            List<LocationSO> locationList = dayData.GetAvailableLocation();
            LocationName location = locationList[0].LocationName;
            //Instantiate PinTemplates
            pinObject = new List<PinGUI>();
            foreach (Transform pinTransform in pinTransformList)
            {
                PinGUI newPin = Instantiate(pinTemplate, pinTransform);
                newPin.Init();
                pinObject.Add(newPin);
            }

            
            List<BaseLocationSelectionCommand> commandList = dayData.FactorizeCommandActionWithinLocation(locationList[0]);

            foreach (BaseLocationSelectionCommand command in commandList)
            {
                Sprite news = command.GetActionIconSprite;
                command.ExecuteCommand();
            }
        }
    }
}
