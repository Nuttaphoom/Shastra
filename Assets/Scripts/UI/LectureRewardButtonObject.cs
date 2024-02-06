using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class LectureRewardButtonObject : MonoBehaviour
    {
        [SerializeField]
        private Button lectureRewardButton;
        [SerializeField]
        private Image rewardImage;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private ButtonHover _buttonHover;

        public Button GetButtonComponent => lectureRewardButton;
        public Image GetIconImage => rewardImage;
        public Animator GetAnimator => anim;
        public void SetRewardDetail(string newText, Sprite newIcon)
        {
            _buttonHover.InitWindowDetail(newText, newIcon);
            rewardImage.sprite = newIcon;
        }
        public void TriggerAnimation()
        {
            anim.enabled = true;
        }

    }
}
