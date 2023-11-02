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
    public class StatGUIScaler : MonoBehaviour
    {
        public enum ModifiedEnergy
        {
            NONE,
            LIGHT,
            DARK
        }

        [SerializeField]
        private CombatEntity _owner  ;

        public TextMeshProUGUI lightNumText;
        public TextMeshProUGUI darkNumText;

        [Header("Energy bar value")]
        
        private float lightScale = 50 ;
        
        private float darkScale = 50;
        
        private float maxEnergyVal = 100.0f;
        [SerializeField] private Image lightImage;

        [Header("HP bar value")]
        private float hpVal;
        private float maxHP;
        [SerializeField] private Image hpImage;
        [SerializeField] private Image secondhpImage;
        [SerializeField] private GameObject gui;

        [Header("Modified Sprite Feedback")]
        [SerializeField] private Image modifiedEnergyImg;
        [SerializeField] private Sprite modLight;
        [SerializeField] private Sprite modDark;
        [SerializeField] private Sprite modDefault;

        private void Awake()
        {
            lightScale = 50;
            darkScale = 50;
            if(lightNumText!=null && darkNumText != null)
            {
                lightNumText.text = lightScale.ToString();
                darkNumText.text = darkScale.ToString();
            }
        }
        private void Start()
        {
            if (_owner != null)
            {
                hpVal = _owner.StatsAccumulator.GetHPAmount();
                maxHP = _owner.StatsAccumulator.GetHPAmount();
            }
            //UpdateHPBarScaleGUI();
            //SetEnergyModified(ModifiedEnergy.NONE);
        }

        private void OnEnable()
        {
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
            if(modifiedEnergyImg == null)
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
        private void OnEnergyModified(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side , int val)
        { 
            if (side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                lightScaleIncrease(val);
            }
            else
            {
                lightScaleDecrease(val);
            }
        }
        private void UpdateEnergyBarScaleGUI()
        {
            lightImage.fillAmount = (int.Parse(lightNumText.text) / maxEnergyVal);
        }
        public void lightScaleIncrease(int val)
        {
            if(lightScale + val > 100)
            {
                val = (int) (100 - lightScale); 
            }
            lightScale += val;
            StartCoroutine(IEAnimateEnergyBarScale());
        }
        public void lightScaleDecrease(int val)
        {
            lightScale -= val;

            if (lightScale < 0)
            {
                lightScale = 0;
            }

            StartCoroutine(IEAnimateEnergyBarScale());
        }
        #endregion
        #region HP
        private void OnHPModified(int damage)
        {
            hpVal = _owner.StatsAccumulator.GetHPAmount();

            float hptemp = maxHP == 0 ? (hpVal == 0 ? 1 : hpVal) : maxHP;

            hpImage.fillAmount = hpVal / hptemp;
            StartCoroutine(IEAnimateHPBarScale(hptemp));
        }
        private void UpdateHPBarScaleGUI()
        {
            //secondhpImage.fillAmount -= 0.01f;
            if (secondhpImage.fillAmount <= 0 && gui != null)
            {
                //gui.SetActive(false);
            }
        }
        #endregion
        #region IEnumerator
        private IEnumerator IEAnimateEnergyBarScale()
        {
            while (int.Parse(lightNumText.text) < lightScale)
            {
                lightNumText.text = (int.Parse(lightNumText.text) + 1).ToString();
                darkNumText.text = (int.Parse(darkNumText.text) - 1).ToString();
                UpdateEnergyBarScaleGUI();
                yield return new WaitForSeconds(0.01f);
            }
            while (int.Parse(lightNumText.text) > lightScale)
            {
                lightNumText.text = (int.Parse(lightNumText.text) - 1).ToString();
                darkNumText.text = (int.Parse(darkNumText.text) + 1).ToString();
                UpdateEnergyBarScaleGUI();
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }

        private IEnumerator IEAnimateHPBarScale(float maxHP)
        {
            float tickRate = 0.5f / ((Mathf.Abs((hpVal / maxHP) - secondhpImage.fillAmount))*100);

            yield return new WaitForSeconds(0.5f);
            while (secondhpImage.fillAmount < hpVal/maxHP)
            {
                secondhpImage.fillAmount += 0.01f;
                yield return new WaitForSeconds(tickRate);
            }
            while (secondhpImage.fillAmount > hpVal/maxHP)
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
