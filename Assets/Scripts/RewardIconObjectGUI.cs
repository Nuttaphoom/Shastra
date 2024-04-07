using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Vanaring
{
    public class RewardIconObjectGUI : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI amountText;
        private EventReward reward;
        private int amount = 1;

        public void Init(EventReward reward)
        {
            this.reward = reward;
            amountText.text = "x" + amount;
            itemIcon.sprite = reward.GetRewardData().RewardIcon;
        }

        public void AddAmount()
        {
            amount++;
            Init(reward);
        }
    }
}
