using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class MissionItemRewardSocketGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI amount;
        [SerializeField] private Animator anim;

        private void Start()
        {
            anim = gameObject.GetComponent<Animator>();
        }

        public void InitSocket(EventRewardData reward)
        {
            itemNameText.text = reward.GetReward().GetRewardData().RewardName;
            itemIcon.sprite = reward.GetReward().GetRewardData().RewardIcon;
            
        }

        public void PlayAnimationMoveIn()
        {
            anim.Play("MoveIn");
        }
    }
}
