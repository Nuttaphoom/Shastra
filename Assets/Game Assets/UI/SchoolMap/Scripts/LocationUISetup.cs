using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class LocationUISetup : MonoBehaviour
    {
        [SerializeField] private GameObject verticalLayout;
        [SerializeField] private GameObject actionButton;
        private List<BaseLocationActionCommand> actionCommandList = new List<BaseLocationActionCommand>();
        private List<GameObject> actionButtonList = new List<GameObject>();
        [SerializeField] private GameObject confirmPanel;
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button mapButton;
        [SerializeField] private TextMeshProUGUI confirmText;
        [SerializeField]
        private SceneDataSO _mapSceneToLoad;
        //[SerializeField] private Button testButton;
        [SerializeField] private List<Transform> buttonTransform = new List<Transform>();

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            //RuntimeLocation d = PersistentActiveDayDatabase.Instance.GetActiveDayData.GetAvailableLocation()[1];
            //var loaderDataContainer = (FindObjectOfType<LoaderDataContainer>() ) ;

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
                Button button = newActionButton.GetComponentInChildren<Button>();
                if (button != null)
                {
                    Image buttonImage = button.GetComponent<Image>();
                    buttonImage.alphaHitTestMinimumThreshold = 0.1f;

                    // Check if the Image component is found
                    if (buttonImage != null)
                    {
                        // Now you can work with the Image component (e.g., change sprite, color, etc.)
                        // Example: buttonImage.color = Color.red;
                    }
                    else
                    {
                        Debug.LogError("Image component not found in the Button game object.");
                    }
                }
                else
                {
                    Debug.LogError("Button game object not found within child GameObjects.");
                }

                Image[] images = newActionButton.GetComponentsInChildren<Image>(true);
                foreach (Image img in images)
                {
                    img.sprite = action.GetActionIconSprite;
                }
                Button actualButton = newActionButton.GetComponentInChildren<Button>(true);
                actualButton.onClick.AddListener(() => PerformAction(action));
                actualButton.GetComponent<Image>().sprite = action.GetActionIconSprite;  
                
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

        public void ClosePanel()
        {
            confirmPanel.SetActive(false);
            confirmButton.onClick.RemoveAllListeners();
        }
    }
}
