using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Vanaring 
{
    public class CharacterSocketGUI : MonoBehaviour
    {
        public enum CharacterTurnStatus
        {
            READY,
            STUN,
            DEAD
        }

        [Header("Image")]
        [SerializeField]
        private Image characterImg;

        [Header("BarScaler")]
        [SerializeField]
        private Image hpBar;
        [SerializeField]
        private Image secondHpBar;
        private List<Image> energySlotList = new List<Image>();
        [SerializeField]
        private Image curveMask;
        [SerializeField]
        private Image mpBar;
        [SerializeField]
        private Image secondMpBar;

        [Header("TextMeshPro")]
        [SerializeField]
        private TextMeshProUGUI characterName;
        [SerializeField]
        private TextMeshProUGUI hpNumText;
        [SerializeField]
        private TextMeshProUGUI mpNumText;

        [Header("Components")]
        [SerializeField]
        private GameObject statusBarLayout;
        [SerializeField]
        private Image effectIcon;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private List<float> slotBarRatios;
        [SerializeField]
        private List<Image> fadeBlackImageList = new List<Image>();
        [SerializeField]
        private GameObject characterArrow;

        private int hpVal;
        private int maxHpVal;
        private int mpVal;
        private int maxMpVal;

        private CombatCharacterSheetSO _characterSheetSO;

        private CombatEntity _combatEntity;

        private void Awake()
        {
            
        }
        private void OnEnable()
        {
            if(_combatEntity != null)
            {
                _combatEntity.SubOnDamageVisualEvent(OnHPModified);
                _combatEntity.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
                _combatEntity.SubOnHealVisualEvent(OnHPModified);
                _combatEntity.SpellCaster.SubOnMPModified(OnMPModified);
            }
        }

        private void OnDisable()
        {
            _combatEntity.UnSubOnDamageVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.UnSubOnModifyEnergy(OnEnergyModified);
            _combatEntity.UnSubOnHealVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.UnSubOnMPModified(OnMPModified);
        }

        private void Update()
        {
            Color imageColor = characterImg.color;
            if (_combatEntity.IsExhausted) { imageColor.a = 0.7f; }
            else { imageColor.a = 1.0f; }
            characterImg.color = imageColor;
        }

        public void Init(CombatEntity combatEntity)
        {
            _combatEntity = combatEntity;
            _combatEntity.SubOnDamageVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
            _combatEntity.SubOnHealVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.SubOnMPModified(OnMPModified);
            _combatEntity.SubOnStatusEffectApplied(AddEffectIcon);

            _characterSheetSO = _combatEntity.CombatCharacterSheet;
            characterImg.sprite = _characterSheetSO.GetCharacterIcon;

            characterArrow.SetActive(false);

            characterName.text = _characterSheetSO.CharacterName;

            hpVal = (int) _combatEntity.StatsAccumulator.GetHPAmount();
            maxHpVal = (int)_combatEntity.StatsAccumulator.GetHPAmount();
            mpVal = (int)_combatEntity.SpellCaster.GetMP;
            maxMpVal = (int)_combatEntity.SpellCaster.GetPeakMP;

            secondHpBar.fillAmount = (float)hpVal / maxHpVal;

            UpdateHPScaleGUI();
            UpdateMPScaleGUI();

            animator = GetComponent<Animator>();

            InitEnergySlot();
        }
        private void AddEffectIcon(EntityStatusEffectPair effect)
        {
            Image newEffectIcon = Instantiate(effectIcon, statusBarLayout.transform);
            newEffectIcon.gameObject.SetActive(true);
            effectIcon.sprite = effect.StatusEffectFactory.StatusImage;
        }

        private void InitEnergySlot()
        {
            curveMask.fillAmount = slotBarRatios[_combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy)];
        }

        #region TurnStatus
        //public void ToggleTurnStatusDisplay(bool b)
        //{
        //    _turnStatusImage.gameObject.SetActive(b);
        //}
        #endregion
        public void ToggleOnTurnHighlightDisplay(bool b)
        {     
        }

        public void ToggleExpandSizeUI()
        {
            gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        public void ToggleShrinkSizeGUI()
        {
            gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }

        public void DisplayArrowOnTargetCharacter()
        {
            if (! characterArrow.activeSelf)
                characterArrow.SetActive(true);
        }

        public void HideArrowOnTargetCharacter()
        {
            if (characterArrow.activeSelf) 
                characterArrow.SetActive(false);
        }

        #region STAT
        private void UpdateHPScaleGUI()
        {
            hpBar.fillAmount = (float)hpVal / maxHpVal;
            hpNumText.text = hpVal.ToString();
        }
        private void UpdateMPScaleGUI()
        {
            mpBar.fillAmount = (float)mpVal / maxMpVal;
            mpNumText.text = mpVal.ToString();
        }

        private void OnHPModified(int damage)
        {
            //animator.SetTrigger("Hit");
            hpVal = (int) _combatEntity.StatsAccumulator.GetHPAmount();
            if(hpVal <= 0)
            {
                foreach (Image image in fadeBlackImageList)
                {
                    Color grayColor = image.color;
                    grayColor = new Color(80.0f / 255.0f, 80.0f / 255.0f, 80.0f / 255.0f);
                    image.color = grayColor;
                }
                foreach (GameObject item in statusBarLayout.gameObject.transform)
                {
                    Destroy(item);
                }
            }
            float hptemp = maxHpVal == 0 ? (hpVal == 0 ? 1 : hpVal) : maxHpVal;
            UpdateHPScaleGUI();
            StopAllCoroutines();
            StartCoroutine(IEAnimateBarScale(hpVal, hptemp, secondHpBar));
        }

        private void OnMPModified(float valueChange)
        {
            mpVal = (int) _combatEntity.SpellCaster.GetMP;
            if (mpVal <= 0)
            {
                mpBar.fillAmount = 0;
            }
            float mptemp = maxMpVal == 0 ? (mpVal == 0 ? 1 : mpVal) : maxMpVal;
            UpdateMPScaleGUI();
            StopAllCoroutines();
            StartCoroutine(IEAnimateBarScale(mpVal, mptemp, secondMpBar));
        }

        private void OnEnergyModified(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int val)
        {
            curveMask.fillAmount = slotBarRatios[_combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy)];
        }

        private IEnumerator IEAnimateBarScale(float currentVal, float maxVal, Image secondBar)
        {
            float tickRate = 0.5f / ((Mathf.Abs((currentVal / maxVal) - secondBar.fillAmount)) * 100);
            yield return new WaitForSeconds(0.5f);
            while (secondBar.fillAmount < currentVal / maxVal)
            {
                if (secondBar.fillAmount < 1.0f)
                {
                    secondBar.fillAmount += 0.01f;
                }
                else if (secondBar.fillAmount >= 1.0f)
                {
                    secondBar.fillAmount = 1.0f;
                }
                yield return new WaitForSeconds(tickRate);
            }
            while (secondBar.fillAmount > currentVal / maxVal)
            {
                if (secondBar.fillAmount > 0.0f)
                {
                    secondBar.fillAmount -= 0.01f;
                }
                else if (secondBar.fillAmount <= 0.0f)
                {
                    secondBar.fillAmount = 0.0f;
                }
                yield return new WaitForSeconds(tickRate);
            }
            yield return null;
        }

        #endregion
    }
}
