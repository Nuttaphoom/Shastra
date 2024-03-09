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
        private Animator animator;
        [SerializeField] private string inAnimationName;
        [SerializeField] private string outAnimationName;

        private bool isHovering = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(animator != null)
            {
                animator.Play(inAnimationName);
            }
            isHovering = true;

            windowObject.SetActive(true);
            windowObject.GetComponent<Animator>().enabled = true;
            windowObject.GetComponent<Animator>().Play(inAnimationName);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (animator != null)
            {
                animator.Play(outAnimationName);
            }
            isHovering = false;
            windowObject.GetComponent<Animator>().enabled = true;
            windowObject.GetComponent<Animator>().Play(outAnimationName);
        }

        public void InitWindowDetail(string newText, Sprite newIcon)
        {
            desText.text = newText;
            iconImageDetail.sprite = newIcon;
        }

        private void OpenDescriptionBox()
        {
            windowObject.gameObject.SetActive(true);
        }
    }
}
