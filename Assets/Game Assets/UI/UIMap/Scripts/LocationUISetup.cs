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

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            //RuntimeLocation d = PersistentActiveDayDatabase.Instance.GetActiveDayData.GetAvailableLocation()[1];
            actionCommandList = (FindObjectOfType<LoaderDataContainer>().UseDataInContainer() as LoaderDataUser<LoadLocationMenuCommandData>).GetData().action_on_this_location;
            foreach (BaseLocationActionCommand action in actionCommandList)
            {
                GameObject newActionButton = Instantiate(actionButton, verticalLayout.transform);
                newActionButton.SetActive(true);
                newActionButton.GetComponent<Button>().onClick.AddListener(() => PerformAction(action));
                //Debug.Log(action.GetTestString);
                newActionButton.GetComponent<Image>().sprite = action.GetActionIconSprite;
                actionButtonList.Add(newActionButton);
            }
        }

        private void PerformAction(BaseLocationActionCommand action)
        {
            OpenPanel();
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
