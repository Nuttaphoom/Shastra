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

        [Header("TextMeshPro")]
        [SerializeField]
        private TextMeshProUGUI characterName;
        [SerializeField]
        private TextMeshProUGUI hpNumText;

        [Header("Components")]
        [SerializeField]
        private CharacterStatusWindowManager _statusWindow;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private List<float> slotBarRatios;
        [SerializeField]
        private List<Image> fadeBlackImageList = new List<Image>();
        [SerializeField]
        private GameObject characterArrow;

        private bool isCanTurn;
        private bool isSelected;

        private int hpVal;
        private int maxHpVal;

        private float maxEnergyVal = 100;
        private float lightVal = 50;
        private float darkVal = 50;
        private CombatCharacterSheetSO _characterSheetSO;

        

        private CombatEntity _combatEntity;

        private void Awake()
        {
            lightVal = 50;
            darkVal = 50;
        }
        private void OnEnable()
        {
            if(_combatEntity != null)
            {
                _combatEntity.SubOnDamageVisualEvent(OnHPModified);
                _combatEntity.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
                _combatEntity.SubOnHealVisualEvent(OnHPModified);
            }
        }

        private void OnDisable()
        {
            _combatEntity.UnSubOnDamageVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.UnSubOnModifyEnergy(OnEnergyModified);
            _combatEntity.UnSubOnHealVisualEvent(OnHPModified);
        }

        public void Init(CombatEntity combatEntity)
        {
            _combatEntity = combatEntity;
            _combatEntity.SubOnDamageVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
            _combatEntity.SubOnHealVisualEvent(OnHPModified);
            characterArrow.SetActive(false);
            _characterSheetSO = _combatEntity.CombatCharacterSheet;
            characterImg.sprite = _characterSheetSO.GetCharacterIcon;
            isCanTurn = false;
            isSelected = false;
            characterName.text = _characterSheetSO.CharacterName;
            hpVal = (int) _combatEntity.StatsAccumulator.GetHPAmount();
            maxHpVal = (int)_combatEntity.StatsAccumulator.GetHPAmount();
            secondHpBar.fillAmount = (float)hpVal / maxHpVal;
            UpdateHPScaleGUI();
            animator = GetComponent<Animator>();
            InitEnergySlot();

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
            //hpNumText.text = "HP " + hpVal.ToString() + "/" + maxHpVal.ToString();
            hpNumText.text = hpVal.ToString();
            //animator.Play("CharacterIconGotHit");
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
            }
            float hptemp = maxHpVal == 0 ? (hpVal == 0 ? 1 : hpVal) : maxHpVal;
            UpdateHPScaleGUI();

            StopAllCoroutines();
            StartCoroutine(IEAnimateHPBarScale(hptemp));
        }

        private void OnEnergyModified(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int val)
        {
            curveMask.fillAmount = slotBarRatios[_combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy)];
            //if (side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            //{
            //    for (int i = 0; i < caster.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy); i++)
            //    {
            //        Color slotColor = lightSlot.color;
            //        energySlotList[i].color = slotColor;
            //    }
            //    //LightScaleIncrease(val);
            //}
            //else
            //{
            //    for (int i = caster.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy) + caster.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy)-1; i >= caster.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy); i--)
            //    {
            //        Color slotColor = darkSlot.color;
            //        energySlotList[i].color = slotColor;
            //    }
            //    //DarkScaleIncrease(val);
            //}
        }

        //public void LightScaleIncrease(int val)
        //{
        //    lightGroup.transform.SetAsLastSibling();
        //    if (lightVal + val > 100)
        //    {
        //        val = (int)(100 - lightVal);
        //    }
        //    lightVal += val;
        //    secondLightBar.fillAmount = lightVal / maxEnergyVal;
        //    StartCoroutine(IEAnimateEnergyBarScale());
        //}
        //public void DarkScaleIncrease(int val)
        //{
        //    darkGroup.transform.SetAsLastSibling();
        //    if (lightVal - val < 0)
        //    {
        //        Debug.Log("Value can't be lower than 0!");
        //        return;
        //    }
        //    lightVal -= val;
        //    secondDarkBar.fillAmount = (float)(100-lightVal) / maxEnergyVal;
        //    StartCoroutine(IEAnimateEnergyBarScale());
        //}

        //private IEnumerator IEAnimateEnergyBarScale()
        //{
        //    yield return new WaitForSeconds(0.5f);
        //    while (int.Parse(lightNumText.text) < lightVal)
        //    {
        //        lightNumText.text = (int.Parse(lightNumText.text) + 1).ToString();
        //        darkNumText.text = (int.Parse(darkNumText.text) - 1).ToString();
        //        UpdateEnergyScaleGUI();
        //        yield return new WaitForSeconds(0.01f);
        //    }
        //    while (int.Parse(lightNumText.text) > lightVal)
        //    {
        //        lightNumText.text = (int.Parse(lightNumText.text) - 1).ToString();
        //        darkNumText.text = (int.Parse(darkNumText.text) + 1).ToString();
        //        UpdateEnergyScaleGUI();
        //        yield return new WaitForSeconds(0.01f);
        //    }
        //    yield return null;
        //}

        private IEnumerator IEAnimateHPBarScale(float maxHP)
        {
            //float tickRate = 0.5f / ((Mathf.Abs((hpVal / maxHP) - secondhpImage.fillAmount)) * 100);
            //while (hpBar.fillAmount < (float)hpVal/maxHpVal)
            //{
            //    hpVal += 1;
            //    UpdateHPScaleGUI();
            //    yield return new WaitForSeconds(0.01f);
            //}
            //while ((float)hpVal / maxHpVal > hpBar.fillAmount)
            //{
            //    hpVal -= 1;
            //    UpdateHPScaleGUI();
            //    yield return new WaitForSeconds(0.01f);
            //}
            float tickRate = 0.5f / ((Mathf.Abs((hpVal / maxHP) - secondHpBar.fillAmount)) * 100);
            //Debug.Log((hpVal / maxHP) + "-" + hpImage.fillAmount + "=" + tickRate);
            yield return new WaitForSeconds(0.5f);
            while (secondHpBar.fillAmount < hpVal / maxHP)
            {
                if (secondHpBar.fillAmount < 1.0f)
                {
                    secondHpBar.fillAmount += 0.01f;
                }
                else if(secondHpBar.fillAmount >= 1.0f)
                {
                    secondHpBar.fillAmount = 1.0f;
                }
                
                yield return new WaitForSeconds(tickRate);
            }
            while (secondHpBar.fillAmount > hpVal / maxHP)
            {
                if (secondHpBar.fillAmount > 0.0f)
                {
                    secondHpBar.fillAmount -= 0.01f;
                }
                else if (secondHpBar.fillAmount <= 0.0f)
                {
                    secondHpBar.fillAmount = 0.0f;
                }
                
                yield return new WaitForSeconds(tickRate);
            }
            yield return null;
        }
        #endregion
    }
}
