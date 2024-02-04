using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

namespace Vanaring
{
    public class LocationUISetup : MonoBehaviour
    {
        [SerializeField] private GameObject actionButton;
        private List<BaseLocationActionCommand> actionCommandList = new List<BaseLocationActionCommand>();
        private List<GameObject> actionButtonList = new List<GameObject>();
        [SerializeField] private GameObject confirmPanel;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button mapButton;
        [SerializeField] private TextMeshProUGUI confirmText;
        [SerializeField]
        private SceneDataSO _mapSceneToLoad;
        [SerializeField]
        private GameObject _lectureSelectPanel;
        [SerializeField]
        private Button tmpLectureButton;
        //[SerializeField] private Button testButton;
        [SerializeField] private List<Transform> buttonTransform = new List<Transform>();

        private void Start()
        {
            Init();
            //_lectureSelectPanel = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<GameObject>("LectureSelectPanel");
        }

        private void Init()
        {
            LoadLocationMenuCommandData currentLocationSO = PersistentSceneLoader.Instance.ExtractLastSavedData<LoadLocationMenuCommandData>().GetData(); //(loaderDataContainer.UseDataInContainer() as LoaderDataUser<LoadLocationMenuCommandData>).GetData() ; //.action_on_this_location;

            foreach (var location in PersistentActiveDayDatabase.Instance.GetActiveDayData.GetAvailableLocation()) {
                if (! location.IsSameLocation(currentLocationSO.LocationName))
                    continue;
                
                actionCommandList = location.ActionOnLocation ;
                break; 
            }
            int i = 0;
            foreach (BaseLocationActionCommand action in actionCommandList)
            {
                
                GameObject newActionButton = Instantiate(actionButton, buttonTransform[i]);
                newActionButton.SetActive(true);

                Image[] images = newActionButton.GetComponentsInChildren<Image>(true);
                foreach (Image img in images)
                {
                    img.sprite = action.GetActionIconSprite;
                }
                Button actualButton = newActionButton.GetComponentInChildren<Button>(true);
                if (action is LectureParticipationActionCommand)
                {
                    tmpLectureButton.onClick.AddListener(() => PerformAction(action));
                    actualButton.onClick.AddListener(OpenLecturePanel);
                }
                if (action is ActivityActionCommand)
                {
                    actualButton.onClick.AddListener(() => PerformAction(action));
                }
                
                actualButton.GetComponent<Image>().sprite = action.GetActionIconSprite;  
                
                //Set Button Description
                TextMeshProUGUI[] textComponents = newActionButton.GetComponentsInChildren<TextMeshProUGUI>(true);
                textComponents[0].text = action.GetActionName;
                textComponents[1].text = action.GetActionDescription;

                actionButtonList.Add(newActionButton);
                i++;
            }
            actionButton.SetActive(false);
            mapButton.onClick.AddListener(() => PersistentSceneLoader.Instance.LoadLocation<int>(_mapSceneToLoad, 0));
        }

        private void PerformAction(BaseLocationActionCommand action)
        {
            OpenPanel();
            string ct = string.Format("{0} at the library?\nThis action will consume <color=#FF0000FF>1 time slot</color>, are you sure to perform this action ? ", action.GetActionDescription);
            confirmText.text = ct;
            confirmButton.onClick.AddListener(() => action.ExecuteCommand());
            
        }

        public void OpenPanel()
        {
            confirmPanel.SetActive(true);
        }

        private void OpenLecturePanel()
        {
            Debug.Log("Opn");
            _lectureSelectPanel.SetActive(true);
        }

        public void ClosePanel()
        {
            confirmPanel.SetActive(false);
            confirmButton.onClick.RemoveAllListeners();
        }
    }
}
