using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

namespace Vanaring
{
    public class SchoolMapSetup : MonoBehaviour
    {
        [SerializeField]
        private Image _mapImage;

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
        private List<RuntimeLocation> availableLocationList;




        private void Awake()
        {
            if (_mapImage == null)
                throw new Exception("_mapImage hasn't been assigned");
            availableLocationList = PersistentActiveDayDatabase.Instance.GetActiveDayData.GetAvailableLocationAccordingToDayTime();
            //LoadPin(PersistentActiveDayDatabase.Instance.GetActiveDayData);
            LoadAllPin();

            //StartCoroutine(PersistentTutorialManager.Instance.CheckTuitorialNotifier("MapExplain")) ; 
        }
        private void LoadAllPin()
        {
            LoadMapBackground();

            //Instantiate PinTemplates
            pinObject = new List<PinGUI>();
            int locationIndex = 0;
            foreach (RuntimeLocation location in availableLocationList)
            {
                switch (location.LocationName)
                {
                    case LocationName.Dorm:
                        locationIndex = 0;
                        break;
                    case LocationName.Library:
                        locationIndex = 1;
                        break;
                    //case LocationName.Stadium:
                    //    locationIndex = 2;
                    //    break;
                    //case LocationName.Cottage:
                    //    locationIndex = 3;
                    //    break;
                    default:
                        Debug.LogError(location.LocationName + " hasn't been set in locationIndex");
                        break;
                }
                PinGUI newPin = Instantiate(pinTemplate, pinTransformList[locationIndex]);
                newPin.Init(location);
                pinObject.Add(newPin);
            }
        }

        #region Map Image Method
        [SerializeField]
        private AssetReferenceT<Sprite> _morningMapAddress;
        [SerializeField]
        private AssetReferenceT<Sprite> _noonMapAddress;
        [SerializeField]
        private AssetReferenceT<Sprite> _nightMapAddress;

        private Sprite _morningMapSprite;
        private Sprite _noonMapSprite;
        private Sprite _nightMapSprite;

        private void LoadMapBackground()
        {
            LoadMapSpriteFromAddress(); 

            EDayTime currentDayTime = PersistentActiveDayDatabase.Instance.GetActiveDayData.GetCurrentDayTime;
            Debug.Log("load map with current day time :  " + currentDayTime);
            switch (currentDayTime) {
                case EDayTime.Morning:
                    _mapImage.sprite = _morningMapSprite;
                    break;
                case EDayTime.Noon:
                    _mapImage.sprite = _noonMapSprite;
                    break;
                case EDayTime.Night:
                    _mapImage.sprite = _nightMapSprite;
                    break;
                default:
                    break;
            }
        }

        private void LoadMapSpriteFromAddress()
        {
            if (_morningMapAddress == null || _noonMapAddress == null || _nightMapAddress == null)
            {
                throw new NullReferenceException("Address of maps image hasn't been assigned"); 
            }

            if (_morningMapSprite == null)
            {
                _morningMapSprite = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<Sprite>(_morningMapAddress);
            }
            if (_noonMapSprite == null)
            {
                _noonMapSprite = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<Sprite>(_noonMapAddress);

            }
            if (_nightMapSprite == null)
            {
                _nightMapSprite = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<Sprite>(_nightMapAddress);

            }
        }
        #endregion
    }
}
