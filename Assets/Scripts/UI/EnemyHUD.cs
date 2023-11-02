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

        //public TextMeshProUGUI lightNumText;
        //public TextMeshProUGUI darkNumText;

        [SerializeField]
        private TextMeshProUGUI enemyName;

        [Header("Energy bar value")]

        private float lightScale = 1;

        private float darkScale = 1;

        private float maxEnergyVal = 100.0f;
        [SerializeField] private Image lightImage;

        [Header("HP bar value")]
        private float hpVal;
        private float maxHP;
        [SerializeField] private Image hpImage;
        [SerializeField] private Image secondhpImage;
        //[SerializeField] private GameObject gui;

        [Header("Modified Sprite Feedback")]
        [SerializeField] private Image modifiedEnergyImg;
        [SerializeField] private Sprite modLight;
        [SerializeField] private Sprite modDark;
        [SerializeField] private Sprite modDefault;

        [Header("EnergySlot")]
        [SerializeField] private Image lightSlotImg;
        [SerializeField] private Image darkSlotImg;
        private List<Image> energySlotList = new List<Image>();
        [SerializeField] private GameObject horizontalLayout;
        [SerializeField] private TextMeshProUGUI lightTmpText;
        [SerializeField] private TextMeshProUGUI darkTmpText;


        private void Awake()
        {
            //lightScale = 50;
            //darkScale = 50;
            //if (lightNumText != null && darkNumText != null)
            //{
            //    lightNumText.text = lightScale.ToString();
            //    darkNumText.text = darkScale.ToString();
            //}
        }
        private void Start()
        {
            if (_owner != null)
            {
                hpVal = _owner.StatsAccumulator.GetHPAmount();
                maxHP = _owner.StatsAccumulator.GetHPAmount();
                
                lightScale = _owner.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy);
                darkScale = _owner.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy);

                lightTmpText.text = lightScale.ToString();
                darkTmpText.text = darkScale.ToString();

                InitEnergySlot();

                enemyName.text = _owner.CharacterSheet.CharacterName.ToString();
            }
        }

        private void InitEnergySlot()
        {
            for (int i = energySlotList.Count - 1 ; i >= 0 ; i--)
            {
                Destroy(energySlotList[i]);
                energySlotList.RemoveAt(i);
            }
            if (lightScale > 0)
            {
                for (int i = 0; i < lightScale; i++)
                {
                    Image slot = Instantiate(lightSlotImg, horizontalLayout.transform);
                    slot.gameObject.SetActive(true);
                    energySlotList.Add(slot);
                }
            }
            if (darkScale > 0)
            {
                for (int i = 0; i < darkScale; i++)
                {
                    Image slot = Instantiate(darkSlotImg, horizontalLayout.transform);
                    slot.gameObject.SetActive(true);
                    energySlotList.Add(slot);
                }
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
        private void OnEnergyModified(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int val)
        {
            if (side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                lightScale += val;
                lightTmpText.text = lightScale.ToString();
                InitEnergySlot();
                //lightScaleIncrease(val);
            }
            else
            {
                darkScale += val;
                darkTmpText.text = darkScale.ToString();
                InitEnergySlot();
                //lightScaleDecrease(val);
            }
        }
        private void UpdateEnergyBarScaleGUI()
        {
            //lightImage.fillAmount = (int.Parse(lightNumText.text) / maxEnergyVal);
        }
        public void lightScaleIncrease(int val)
        {
            if (lightScale + val > 100)
            {
                val = (int)(100 - lightScale);
            }
            lightScale += val;
            //StartCoroutine(IEAnimateEnergyBarScale());
        }
        public void lightScaleDecrease(int val)
        {
            lightScale -= val;

            if (lightScale < 0)
            {
                lightScale = 0;
            }

            //StartCoroutine(IEAnimateEnergyBarScale());
        }
        #endregion
        #region HP
        private void OnHPModified(int damage)
        {
            //Debug.LogError("HP check");
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
