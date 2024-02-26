using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Vanaring
{
    public class BonusTraitObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI bonusTraitName;
        [SerializeField] private TextMeshProUGUI bonusTraitLevel;
        [SerializeField] private Image lockIcon;
        [SerializeField] private TextMeshProUGUI curTraitLevel;
        [SerializeField] private Animator textAnim;
        private string traitName;
        private bool IsPlaying = false;
        private bool isLevelReach;
        public bool IsLvReach => isLevelReach;

        public void Init(string name, string level, bool isReach, string curLevel)
        {
            bonusTraitName.text = name;
            isLevelReach = isReach;
            traitName = name;
            bonusTraitLevel.text = level;
            curTraitLevel.text = "Current: " + curLevel;
            if (isReach)
            {
                lockIcon.gameObject.SetActive(false);
            }
        }
        public void StartAnimation()
        {
            if (!IsPlaying)
            {
                Debug.Log("Play anim " + traitName);
                IsPlaying = true;
            }
            
            //textAnim.Play();
        }

        public void StopAnimation()
        {
            if (IsPlaying)
            {
                Debug.Log("Stop anim " + traitName);
                IsPlaying = false;
            }
            
            //textAnim.gameObject.active = false;
        }
    }
}
