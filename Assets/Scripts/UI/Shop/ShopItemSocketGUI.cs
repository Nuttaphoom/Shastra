using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Vanaring
{
    public class ShopItemSocketGUI : MonoBehaviour
    {
        [Header("GFX")]
        [SerializeField] private GameObject socketGFX;
        [Header("Item Detail")]
        [SerializeField] private Button itemButton;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemPriceText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private Image itemIcon;

        public Button ItemButton
        {
            get
            {
                return itemButton;
            }
        }

        public void Init(DungeonShopManager.ProductData item)
        {
            itemNameText.text = item.DisplayedProductName;
            itemPriceText.text = item.Cost.ToString();
            itemDescriptionText.text = item.Reward.ItemReward.GetRewardData().RewardDescription;
            itemIcon.sprite = item.Reward.ItemReward.GetRewardData().RewardIcon;
        }
        public void OnSelectSocket()
        {
            socketGFX.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        public void OnDeSelectSocket()
        {
            socketGFX.GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }
}
