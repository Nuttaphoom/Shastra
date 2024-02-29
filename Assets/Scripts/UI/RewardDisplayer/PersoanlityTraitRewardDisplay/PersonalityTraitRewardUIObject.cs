using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Vanaring
{
    public class PersonalityTraitRewardUIObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI traitName;
        [SerializeField] private TextMeshProUGUI traitLevel;
        [SerializeField] private Image fillBar;
        [SerializeField] private Image traitIcon;
        private Trait.Trait_Type traitType;
        public bool IsTraitTypeEqual(Trait.Trait_Type inputType)
        {
            return inputType == traitType;
        }
        public Trait.Trait_Type TraitType
        {
            get { return traitType; }
            set { traitType = value; }
        }
        public Image TraitIcon
        {
            get { return traitIcon; }
            set { traitIcon = value; }
        }
        public Image FillBar
        {
            get { return fillBar; }
            set { fillBar = value; }
        }
        public TextMeshProUGUI TraitName
        {
            get { return traitName; }
            set { traitName = value; }
        }
        public TextMeshProUGUI TraitLevel
        {
            get { return traitLevel; }
            set { traitLevel = value; }
        }

        public void Init(Trait.Trait_Type type, Sprite img, int curLv, float curExp)
        {
            traitType = type;
            traitLevel.text = curLv.ToString();
            traitName.text = type.ToString();
            traitIcon.sprite = img;
            fillBar.fillAmount = curExp;
            Debug.Log("Trait " + type + "start at " + curExp.ToString());
        }
    }
}
