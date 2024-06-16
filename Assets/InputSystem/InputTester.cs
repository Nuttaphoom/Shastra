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
        public GameObject currentSelected;
        private void Update()
        {
            //Debug.Log(EventSystem.current.currentSelectedGameObject + " is selected");
            currentSelected = EventSystem.current.currentSelectedGameObject;
        }
    }
}
