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
        [SerializeField] private GameObject nextArrow;
        [SerializeField] private GameObject prevArrow;

        public void Init(BackpackItemData itemData) {
            itemIcon.sprite = itemData.BackpackItem.GetDescriptionBaseField().FieldImage;
            amountText.text = itemData.Amount.ToString();
        }
    }
}
