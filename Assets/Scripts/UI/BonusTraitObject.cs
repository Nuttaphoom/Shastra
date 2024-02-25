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

        public void Init(string name, string level, bool isReach, string curLevel)
        {
            bonusTraitName.text = name;
            bonusTraitLevel.text = level;
            curTraitLevel.text = "Current: " + curLevel;
            if (isReach)
            {
                lockIcon.gameObject.SetActive(false);
            }
        }
    }
}
