using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Vanaring_DepaDemo
{
    public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject windowObject; // Reference to your window UI object
        [SerializeField]
        private GameObject _button;

        private bool isHovering = false;

        private void Update()
        {
            // If hovering, update window position to follow the mouse
            if (isHovering)
            {

                //Vector2 mousePosition = Input.mousePosition;
                //Vector2 windowPos = new Vector3(_button.transform.position.x+1, _button.transform.position.y+1);
                //windowObject.transform.position = mousePosition;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;
            windowObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;
            windowObject.SetActive(false);
        }
    }
}
