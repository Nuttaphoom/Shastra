using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Vanaring
{
    public class InputTester : MonoBehaviour
    {
        public GameObject previousButton;
        public GameObject currentButton;
        private void Update()
        {
            if (previousButton != EventSystem.current.currentSelectedGameObject)
            {
                if (!EventSystem.current.currentSelectedGameObject.CompareTag("UI/NonInteractable"))
                {
                    previousButton = EventSystem.current.currentSelectedGameObject;
                }
                else
                {
                    if (previousButton != null)
                    {
                        Button prev = previousButton.GetComponent<Button>();
                        prev.Select();
                    }
                }
                
            }

            currentButton = EventSystem.current.currentSelectedGameObject;
        }
    }
}
