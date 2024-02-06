using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Vanaring 
{
    public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject windowObject; // Reference to your window UI object
        [SerializeField]
        private TextMeshProUGUI desText;
        [SerializeField]
        private Image iconImageDetail;
        [SerializeField]
        private Button _button;
        [SerializeField]
        private Animator animator;

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
            animator.Play("LectureRewardDetailSlideUp");
            isHovering = true;
            windowObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            animator.Play("LectureRewardDetailSlideDown");
            isHovering = false;
        }

        public void InitWindowDetail(string newText, Sprite newIcon)
        {
            desText.text = newText;
            iconImageDetail.sprite = newIcon;
        }
    }
}
