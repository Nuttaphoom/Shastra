using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace Vanaring
{
    public class PersonalityTraitRewardUIObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI traitName;
        [SerializeField] private TextMeshProUGUI traitLevel;
        [SerializeField] private Image fillBar;
        [SerializeField] private Image traitIcon;
        private Animator anim;
        private Trait.Trait_Type traitType;
        private void Awake()
        {
            anim = gameObject.GetComponent<Animator>();
        }

        public void TriggerLevelUPAnimation()
        {
            transform.localScale = Vector3.zero;

            // Scale up over 1 second
            transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.01f)
                .SetEase(Ease.OutBack) // You can choose a different ease function if needed
                .OnComplete(() =>
                {
                // Scale down over 0.5 seconds after scaling up
                transform.DOScale(Vector3.one, 0.2f)
                        .SetEase(Ease.InBack); // You can choose a different ease function here as well
            });
            Debug.Log("Anim Play");
        }

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
