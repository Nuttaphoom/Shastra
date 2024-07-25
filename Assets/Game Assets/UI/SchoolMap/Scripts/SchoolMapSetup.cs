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

        [SerializeField] private SceneDataSO _shortcutSceneSO;

        [SerializeField] private Button _dungeonButton;

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
        [SerializeField]
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

            StartCoroutine(PersistentTutorialManager.Instance.CheckTuitorialNotifier("MapExplain"));
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
                    case LocationName.SchoolField:
                        locationIndex = 2;
                        break;
                    case LocationName.Classroom:
                        locationIndex = 3;
                        break;
                    case LocationName.Dungeon:
                        locationIndex = 4;
                        break;
                    case LocationName.AomeTest:
                        locationIndex = 5;
                        break;
                    default:
                        Debug.LogError(location.LocationName + " hasn't been set in locationIndex");
                        break;
                }
                PinGUI newPin = Instantiate(pinTemplate, pinTransformList[locationIndex]);
                newPin.Init(location);
                newPin.EventButton.onClick.AddListener(delegate { PersistentButtonSelector.Instance.AddPreviousButton(newPin.EventButton); } );
                pinObject.Add(newPin);
            }

            for (int i = 0; i < pinObject.Count; i++)
            {
                Navigation NewNav = new Navigation();
                NewNav.mode = Navigation.Mode.Explicit;
                NewNav.selectOnDown = _dungeonButton;
                if ((i - 1) < 0)
                {
                    NewNav.selectOnLeft = _dungeonButton;
                }
                else
                {
                    NewNav.selectOnLeft = pinObject[((i - 1) < 0) ? (pinObject.Count - 1) : (i - 1)].TemplateButton;
                }
                if ((i + 1) >= pinObject.Count)
                {
                    NewNav.selectOnRight = _dungeonButton;
                }
                else
                {
                    NewNav.selectOnRight = pinObject[(i + 1) % pinObject.Count].TemplateButton;
                }
                //NewNav.selectOnRight = pinObject[(i + 1) % pinObject.Count].TemplateButton;
                pinObject[i].TemplateButton.navigation = NewNav;
            }

            if (pinObject.Count > 0)
            {
                Navigation DunNav = new Navigation();

                DunNav.mode = Navigation.Mode.Explicit;
                DunNav.selectOnRight = pinObject[0].TemplateButton;
                DunNav.selectOnUp = pinObject[pinObject.Count - 1].TemplateButton;
                DunNav.selectOnLeft = pinObject[pinObject.Count - 1].TemplateButton;
                _dungeonButton.navigation = DunNav;
            }
            // Setting Button Navigation with input control
            if (pinObject.Count > 0)
            {
                pinObject[0].OnHoverButton();
            }else
            {
                PersistentButtonSelector.Instance.AssignInitialButtons(_dungeonButton) ;

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
