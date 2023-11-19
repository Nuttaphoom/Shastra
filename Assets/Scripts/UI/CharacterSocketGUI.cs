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
        [SerializeField]
        private Image _highlightTurn;

        [Header("BarScaler")]
        [SerializeField]
        private Image hpBar;
        [SerializeField]
        private Image secondHpBar;
        [SerializeField]
        private GameObject verticalLayout;
        [SerializeField]
        private Image lightSlot;
        [SerializeField]
        private Image darkSlot;
        [SerializeField]
        private Image energySlot;
        private List<Image> energySlotList = new List<Image>();
        [SerializeField] List<float> energyBarRatio = new List<float>();
        [SerializeField] Image darkCurveBar;

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

        private bool isCanTurn;
        private bool isSelected;

        private int hpVal;
        private int maxHpVal;

        private float maxEnergyVal = 100;
        private float lightVal = 50;
        private float darkVal = 50;
        private CharacterSheetSO _characterSheetSO;

        

        private CombatEntity _combatEntity;

        private void Awake()
        {
            lightVal = 50;
            darkVal = 50;

            //if (lightNumText != null && darkNumText != null)
            //{
            //    lightNumText.text = lightVal.ToString();
            //    darkNumText.text = darkVal.ToString();
            //}
        }
        private void OnEnable()
        {
            if(_combatEntity != null)
            {
                _combatEntity.SubOnDamageVisualEvent(OnHPModified);
                _combatEntity.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
            }
        }

        private void OnDisable()
        {
            _combatEntity.UnSubOnDamageVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.UnSubOnModifyEnergy(OnEnergyModified);
        }

        public void Init(CombatEntity combatEntity)
        {
            _combatEntity = combatEntity;
            _combatEntity.SubOnDamageVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.SubOnModifyEnergy(OnEnergyModified);

            _characterSheetSO = _combatEntity.CharacterSheet;
            characterImg.sprite = _characterSheetSO.GetCharacterIcon;
            isCanTurn = false;
            isSelected = false;
            _highlightTurn.gameObject.SetActive(false);
            characterName.text = _characterSheetSO.CharacterName;
            hpVal = _combatEntity.StatsAccumulator.GetHPAmount();
            maxHpVal = _combatEntity.StatsAccumulator.GetHPAmount();
            secondHpBar.fillAmount = (float)hpVal / maxHpVal;
            UpdateHPScaleGUI();
            animator = GetComponent<Animator>();

            //_combatEntity.GetStatusEffectHandler().SubOnStatusVisualEvent(_statusWindow.ShowStatusIconUI);
            InitEnergySlot();

        }

        private void InitEnergySlot()
        {
            darkCurveBar.fillAmount = energyBarRatio[_combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy)];
//            for (int i = 0; i < _combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy); i++)
//            {
//                Image slot = Instantiate(energySlot, verticalLayout.transform);
//                Color lightColor = lightSlot.color;
//                slot.color = lightColor;
//                slot.gameObject.SetActive(true);
//                energySlotList.Add(slot);
//;           }
//            for (int i = 0; i < _combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy); i++)
//            {
//                Image slot = Instantiate(energySlot, verticalLayout.transform);
//                Color lightColor = darkSlot.color;
//                slot.color = lightColor;
//                slot.gameObject.SetActive(true);
//                energySlotList.Add(slot);
//            }

        }

        #region TurnStatus
        public void ToggleTurnStatusDisplay(bool b)
        {
            //_turnStatusImage.gameObject.SetActive(b);
        }
        #endregion
        public void ToggleOnTurnHighlightDisplay(bool b)
        {
            _highlightTurn.gameObject.SetActive(b);            
        }

        public void ToggleExpandSizeUI()
        {
            gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        public void ToggleShrinkSizeGUI()
        {
            gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }

        #region STAT
        private void UpdateHPScaleGUI()
        {
            hpBar.fillAmount = (float)hpVal / maxHpVal;
            //hpNumText.text = "HP " + hpVal.ToString() + "/" + maxHpVal.ToString();
            hpNumText.text = hpVal.ToString();
            //animator.Play("CharacterIconGotHit");
        }

        private void UpdateEnergyScaleGUI()
        {
            //lightBar.fillAmount = (int.Parse(lightNumText.text) / maxEnergyVal);
            //darkBar.fillAmount = (maxEnergyVal/100) - lightBar.fillAmount;
        }

        private void OnHPModified(int damage)
        {
            
            hpVal = _combatEntity.StatsAccumulator.GetHPAmount();
            float hptemp = maxHpVal == 0 ? (hpVal == 0 ? 1 : hpVal) : maxHpVal;
            UpdateHPScaleGUI();

            StopAllCoroutines();
            StartCoroutine(IEAnimateHPBarScale(hptemp));
        }

        private void OnEnergyModified(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int val)
        {

            if (side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                ////Debug.Log("Player spell:" + caster.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy));
                //for (int i = 0; i < caster.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy); i++)
                //{
                //    Color slotColor = lightSlot.color;
                //    energySlotList[i].color = slotColor;
                //}
                ////Increase Light
                ////LightScaleIncrease(val);


            }
            else
            {
                //for (int i = caster.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy) + caster.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy)-1; i >= caster.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy); i--)
                //{
                //    Color slotColor = darkSlot.color;
                //    energySlotList[i].color = slotColor;
                //}
                

                //Increase Dark
                //DarkScaleIncrease(val);
            }
            darkCurveBar.fillAmount = energyBarRatio[caster.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy)];
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
