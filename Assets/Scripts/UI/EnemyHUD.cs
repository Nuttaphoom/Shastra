using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using UnityEditor;
using Unity.Collections;
using Unity.Jobs;
using DG.Tweening;

namespace Vanaring
{
    public class EnemyHUD : MonoBehaviour
    {
        public enum ModifiedEnergy
        {
            NONE,
            LIGHT,
            DARK
        }
        private CombatEntity _owner;

        [SerializeField]
        private TextMeshProUGUI enemyName;

        [Header("Energy bar value")]

        private float lightScale = 1;
        private float darkScale = 1;
        private float maxLightScale = 1;
        private float maxDarkScale = 1;

        private float maxEnergyVal = 100.0f;
        [SerializeField] private Image lightImage;

        [Header("HP bar value")]
        private float hpVal;
        private float maxHP;
        [SerializeField] private Image hpImage;
        [SerializeField] private Image secondhpImage;

        [Header("Modified Sprite Feedback")]
        [SerializeField] private Image modifiedEnergyImg;
        [SerializeField] private Sprite modLight;
        [SerializeField] private Sprite modDark;
        [SerializeField] private Sprite modDefault;

        [Header("EnergySlot")]
        [SerializeField] private Image lightSlotImg;
        [SerializeField] private Image darkSlotImg;
        private Color defaultSlotColor;
        private List<Image> energySlotList = new List<Image>();
        [SerializeField] private GameObject horizontalLayout;
        [SerializeField] private TextMeshProUGUI lightTmpText;
        [SerializeField] private TextMeshProUGUI darkTmpText;


        private void Awake()
        {
        }
        private void Start()
        {
            if (_owner != null)
            {
                hpVal = _owner.StatsAccumulator.GetHPAmount();
                maxHP = _owner.StatsAccumulator.GetHPAmount();
                
                lightScale = _owner.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy);
                darkScale = _owner.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy);
                maxLightScale = lightScale;
                maxDarkScale = darkScale;

                lightTmpText.text = lightScale.ToString();
                darkTmpText.text = darkScale.ToString();

                InitEnergySlot();

                enemyName.text = _owner.CharacterSheet.CharacterName.ToString();
            }
        }

        public CombatEntity GetOwner()
        {
            return _owner;
        }

        public void Init(CombatEntity owner)
        {
            _owner = owner;
            _owner.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
            _owner.SubOnDamageVisualEvent(OnHPModified);
        }

        private void OnEnable()
        {
            if (_owner == null)
                return;  

            _owner.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
            _owner.SubOnDamageVisualEvent(OnHPModified);
        }

        private void OnDisable()
        {
            _owner.SpellCaster.UnSubOnModifyEnergy(OnEnergyModified);
            _owner.UnSubOnDamageVisualEvent(OnHPModified);
        }

        public void SetEnergyModified(ModifiedEnergy modifiedEnergy)
        {
            if (modifiedEnergyImg == null)
            {
                return;
            }
            switch (modifiedEnergy)
            {
                case ModifiedEnergy.NONE:
                    modifiedEnergyImg.sprite = modDefault;
                    break;
                case ModifiedEnergy.LIGHT:
                    modifiedEnergyImg.sprite = modLight;
                    break;
                case ModifiedEnergy.DARK:
                    modifiedEnergyImg.sprite = modDark;
                    break;
            }
        }

        #region Energy
        /// <summary>
        /// val = increased value
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="side"></param>
        /// <param name="val" ></param>
        private void InitEnergySlot()
        {
            if (lightScale > 0)
            {
                for (int i = 0; i < lightScale; i++)
                {
                    Image slot = Instantiate(lightSlotImg, horizontalLayout.transform);
                    slot.gameObject.SetActive(true);
                    energySlotList.Add(slot);
                    defaultSlotColor = slot.color;
                }
            }
            if (darkScale > 0)
            {
                for (int i = 0; i < darkScale; i++)
                {
                    Image slot = Instantiate(darkSlotImg, horizontalLayout.transform);
                    slot.gameObject.SetActive(true);
                    energySlotList.Add(slot);
                    defaultSlotColor = slot.color;
                }
            }
        }

        private void OnEnergyModified(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int val)
        {
            Debug.Log(_owner.CharacterSheet.CharacterName + " mod energy " + side + " " + ": " + val);
            if(val == 0)
            {
                return;
            }
            if (side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                if (lightScale + val >= 0)
                {
                    lightScale += val;
                    lightTmpText.text = lightScale.ToString();
                    if (val < 0)
                    {
                        StartCoroutine(SlotBreak((int)maxLightScale, (int)lightScale));
                    }
                    else
                    {
                        StartCoroutine(SlotRecovery());
                    }
                }
            }
            else
            {
                if (darkScale + val >= 0)
                {
                    darkScale += val;
                    darkTmpText.text = darkScale.ToString();
                    if (val < 0)
                    {
                        StartCoroutine(SlotBreak((int)maxDarkScale, (int)darkScale));
                    }
                    else
                    {
                        StartCoroutine(SlotRecovery());
                    }
                }
            }
        }

        private IEnumerator SlotBreak(int maxSlot, int curScale)
        {
            for (int i = maxSlot-1; i >= 0; i--)
            {
                if (i+1 > curScale)
                {
                    //Break
                    Color curColor = energySlotList[i].color;
                    curColor.a = 0.3f;
                    energySlotList[i].color = curColor;
                }
                else
                {
                    //Stay
                    Color curColor = energySlotList[i].color;
                    curColor.a = 1.0f;
                    energySlotList[i].color = curColor;
                }
                yield return new WaitForSeconds(0.01f);
            }
            Debug.Log("Slot break to: " + curScale);
            yield return null;
        }

        private IEnumerator SlotRecovery()
        {
            int i = 0;
            foreach (Image slot in energySlotList)
            {
                Color curColor = energySlotList[i].color;
                curColor.a = 1.0f;
                curColor = Color.white;
                energySlotList[i].color = curColor;
                yield return new WaitForSeconds(0.1f);
                defaultSlotColor.a = 1.0f;
                energySlotList[i].color = defaultSlotColor;
                i++;
            }
            Debug.Log("Slot recovery to: " + i);
            yield return null;
        }

        private void UpdateEnergySlotState()
        {
            
        }
        //public void lightScaleIncrease(int val)
        //{
        //    if (lightScale + val > 100)
        //    {
        //        val = (int)(100 - lightScale);
        //    }
        //    lightScale += val;
        //    //StartCoroutine(IEAnimateEnergyBarScale());
        //}
        //public void lightScaleDecrease(int val)
        //{
        //    lightScale -= val;

        //    if (lightScale < 0)
        //    {
        //        lightScale = 0;
        //    }

        //    //StartCoroutine(IEAnimateEnergyBarScale());
        //}
        #endregion
        #region HP
        private void OnHPModified(int damage)
        {
            //throw new Exception("dd");
            hpVal = _owner.StatsAccumulator.GetHPAmount();

            float hptemp = maxHP == 0 ? (hpVal == 0 ? 1 : hpVal) : maxHP;

            hpImage.fillAmount = hpVal / hptemp;
            StartCoroutine(IEAnimateHPBarScale(hptemp));
        }
        private void UpdateHPBarScaleGUI()
        {
            //secondhpImage.fillAmount -= 0.01f;
            //if (secondhpImage.fillAmount <= 0 && gui != null)
            //{
            //    //gui.SetActive(false);
            //}
        }
        #endregion
        #region IEnumerator
        //private IEnumerator IEAnimateEnergyBarScale()
        //{
        //    while (int.Parse(lightNumText.text) < lightScale)
        //    {
        //        lightNumText.text = (int.Parse(lightNumText.text) + 1).ToString();
        //        darkNumText.text = (int.Parse(darkNumText.text) - 1).ToString();
        //        UpdateEnergyBarScaleGUI();
        //        yield return new WaitForSeconds(0.01f);
        //    }
        //    while (int.Parse(lightNumText.text) > lightScale)
        //    {
        //        lightNumText.text = (int.Parse(lightNumText.text) - 1).ToString();
        //        darkNumText.text = (int.Parse(darkNumText.text) + 1).ToString();
        //        UpdateEnergyBarScaleGUI();
        //        yield return new WaitForSeconds(0.01f);
        //    }
        //    yield return null;
        //}

        private IEnumerator IEAnimateHPBarScale(float maxHP)
        {
            float tickRate = 0.5f / ((Mathf.Abs((hpVal / maxHP) - secondhpImage.fillAmount)) * 100);

            yield return new WaitForSeconds(0.5f);
            while (secondhpImage.fillAmount < hpVal / maxHP)
            {
                secondhpImage.fillAmount += 0.01f;
                yield return new WaitForSeconds(tickRate);
            }
            while (secondhpImage.fillAmount > hpVal / maxHP)
            {
                secondhpImage.fillAmount -= 0.01f;
                yield return new WaitForSeconds(tickRate);
            }

            if (hpVal <= 0)
            {
                Destroy(gameObject);
            }


            yield return null;
        }
        #endregion
    }
}
