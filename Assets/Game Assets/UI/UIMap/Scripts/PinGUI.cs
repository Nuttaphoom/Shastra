using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Vanaring
{
    public class PinGUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject EventWindow;

        [SerializeField]
        private Button _eventButton;

        [SerializeField]
        private Image pinImage;
        [SerializeField]
        private Sprite loadCutsceneIcon;
        [SerializeField]
        private Sprite loadLocationIcon;

        private Transform horizontalLayout;
        private List<BaseLocationSelectionCommand> commandList;

        [SerializeField]
        private List<LocationSelectionCommandRegister> baseCommand;

        public void Init(Sprite image) {

            //foreach (LocationSelectionCommandRegister command in baseCommand)
            //{
            //    commandList.Add(command.FactorizeLocationSelectionCommand());
            //}
            EventWindow.SetActive(false);
            InitEventButton();
            pinImage.sprite = image;
        }

        private void InitEventButton()
        {
            if(commandList == null)
            {
                Debug.LogWarning("No Command can be execute!");
                return;
            }
            _eventButton.gameObject.SetActive(true);
            foreach (BaseLocationSelectionCommand command in commandList)
            {
                Button newEventButton = Instantiate(_eventButton, horizontalLayout);
                newEventButton.onClick.AddListener(() => command.ExecuteCommand());
                if(command is LoadCutsceneCommand)
                {
                    newEventButton.GetComponent<Image>().sprite = loadCutsceneIcon;
                }
                else if(command is LoadLocationCommand)
                {
                    newEventButton.GetComponent<Image>().sprite = loadLocationIcon;
                }
                
            }
        }

        public void OnHoverEnterUIEventWindow(BaseEventData eventData)
        {
            EventWindow.SetActive(true);
        }

        public void OnHoverExitUIEventWindow(BaseEventData eventData)
        {
            EventWindow.SetActive(false);
        }
    }
}
