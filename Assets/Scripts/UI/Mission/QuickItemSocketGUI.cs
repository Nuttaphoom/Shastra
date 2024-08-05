using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Vanaring
{
    public class QuickItemSocketGUI : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI amountText;
        private int amount;
        [SerializeField] private GameObject nextArrow;
        [SerializeField] private GameObject prevArrow;
        [SerializeField] private GameObject buttonIndicator;
        [SerializeField] private GameObject highlightFrame;

        public void Init(BackpackItemData itemData) {
            itemIcon.sprite = itemData.BackpackItem.GetDescriptionBaseField().FieldImage;
            amount = itemData.Amount;
            amountText.text = "x" + itemData.Amount.ToString();
            nextArrow.SetActive(false);
            prevArrow.SetActive(false);
            buttonIndicator.SetActive(false);
        }

        public void OnSelect()
        {
            nextArrow.SetActive(true);
            prevArrow.SetActive(true);
            buttonIndicator.SetActive(true);
            highlightFrame.SetActive(true);
        }

        public void OnSelectIndex(int cur, int max)
        {
            prevArrow.SetActive(true);
            nextArrow.SetActive(true);
            if (cur == 0)
            {
                prevArrow.SetActive(false);
            }
            if(cur == max - 1)
            {
                nextArrow.SetActive(false);
            }
            
            
            buttonIndicator.SetActive(true);
            highlightFrame.SetActive(true);
        }

        public void OnDeSelect()
        {
            nextArrow.SetActive(false);
            prevArrow.SetActive(false);
            buttonIndicator.SetActive(false);
            highlightFrame.SetActive(false);
        }
        public void UseItemUpdate()
        {
            amount = amount - 1;
            amountText.text = "x" + amount.ToString();
        }
    }
}
