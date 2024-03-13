using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring
{
    public class LectureRewardObject : MonoBehaviour
    {
        [SerializeField]
        private Image rewardImage;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private ButtonHover _buttonHover;
        [SerializeField]
        private TextMeshProUGUI rewardName;
        [SerializeField]
        private TextMeshProUGUI rewardNameDes;
        [SerializeField]
        private TextMeshProUGUI rewardDes;
        [SerializeField]
        private Image faderImg;
        [SerializeField]
        private Image obtainedImg;
        private Sprite rewardIconSprite;
        private string rewardNameString;
        private string rewardDesString;

        public Image GetIconImage => rewardImage;
        public Animator GetAnimator => anim;
        public Sprite GetRewardIconSprite => rewardIconSprite;
        public string GetRewardNameString => rewardNameString;
        public string GetRewardDesString => rewardDesString;
        public void SetRewardDetail(string newText, Sprite newIcon, string newDesText)
        {
            rewardIconSprite = newIcon;
            rewardNameString = newText;
            rewardDesString = newDesText;
            _buttonHover.InitWindowDetail(newText, newIcon);
            rewardImage.sprite = newIcon;
            rewardName.text = newText;
            rewardNameDes.text = newText;
            rewardDes.text = newDesText;
        }
        public void SetIsReachedState(bool isReach)
        {
            faderImg.gameObject.SetActive(!isReach);
        }
        public void SetIsObtainedState(bool isObtain)
        {
            obtainedImg.gameObject.SetActive(isObtain);
        }
        public void TriggerAnimation()
        {
            anim.enabled = true;
        }

    }
}
