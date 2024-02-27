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
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color lockColor;
        [SerializeField] private Color goldColor;
        [SerializeField] private Gradient gradient;
        private string traitName;
        private bool IsPlaying = false;
        private bool isLevelReach;
        private Trait.Trait_Type bonusTraitType;
        public Trait.Trait_Type GetBonusTraitType => bonusTraitType;
        public bool IsLvReach => isLevelReach;

        public void Init(Trait.Trait_Type trait, string level, bool isReach, string curLevel)
        {
            bonusTraitType = trait;
            bonusTraitName.text = trait.ToString();
            isLevelReach = isReach;
            traitName = trait.ToString();
            bonusTraitLevel.text = level;
            curTraitLevel.text = "Current: " + curLevel;
            if (isReach)
            {
                gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                lockIcon.gameObject.SetActive(false);
                bonusTraitName.text = trait.ToString();
                bonusTraitName.color = defaultColor;

                bonusTraitLevel.color = defaultColor;
            }
        }
        public void StartAnimation()
        {
            if (!IsPlaying)
            {
                //bonusTraitName.rectTransform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                StartCoroutine(ScaleOverTime(1.5f, 0.05f));
                Debug.Log("Play anim " + traitName);
                IsPlaying = true;
            }
        }

        public void StopAnimation()
        {
            if (IsPlaying)
            {
                StartCoroutine(ScaleOverTime(1.0f, 1.5f));
                IsPlaying = false;
            }
        }
        private IEnumerator ScaleOverTime(float targetSize, float duration)
        {
            Vector3 initialScale = bonusTraitName.rectTransform.localScale;
            float currentTime = 0f;

            while (currentTime <= duration)
            {
                float t = currentTime / duration;
                bonusTraitName.rectTransform.localScale = Vector3.Lerp(initialScale, new Vector3(targetSize, targetSize, 1f), t);
                currentTime += Time.deltaTime;
                yield return null;
            }
            bonusTraitName.rectTransform.localScale = new Vector3(targetSize, targetSize, 1f);
        }

      
    }
}
