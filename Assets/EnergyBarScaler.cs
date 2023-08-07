using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using UnityEditor;
using Unity.Collections;
using Unity.Jobs;

namespace Vanaring_DepaDemo
{
    
    public class EnergyBarScaler : MonoBehaviour
    {
        [SerializeField]
        private CombatEntity _owner  ;

        public TextMeshProUGUI lightNumText;
        public TextMeshProUGUI darkNumText;

        [Header("EnergyBarValue")]
        
        private float lightScale = 50 ;
        
        private float darkScale = 50;
        
        private float maxEnergyVal = 100.0f;
        [SerializeField] private Image lightImage;

        [Header("HPBarValue")]
        private int hpVal;
        private int maxHP;
        [SerializeField] private Image hpImage;
   

        private void Awake()
        {
            lightScale = 50;
            darkScale = 50;
            UpdateEnergyBarScaleGUI();
            UpdateHPBarScaleGUI();
            lightNumText.text = lightScale.ToString();
            darkNumText.text = darkScale.ToString();
        }

        private void OnEnable()
        {
            _owner.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
            //_owner.SpellCaster.SubOnModifyEnergy(OnEnergyModified);

        }

        private void OnDisable()
        {
            _owner.SpellCaster.UnSubOnModifyEnergy(OnEnergyModified);
            //_owner.SpellCaster.SubOnModifyEnergy(OnEnergyModified);
        }
        #region Energy
        private void OnEnergyModified(RuntimeMangicalEnergy.EnergySide side , int val)
        {
            Debug.Log("visual modify energy in " + gameObject.name); 
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
            if (lightScale - val < 0)
            {
                Debug.Log("Value can't be lower than 0!");
                return;
            }
            lightScale -= val;
            StartCoroutine(IEAnimateEnergyBarScale());
        }
        #endregion
        #region HP
        private void OnHPModified(int damage)
        {
            Debug.Log("visual modify HP in " + gameObject.name);
            if (hpVal - damage < 0)
            {
                hpVal = 0;
            }
            else
            {
                hpVal -= damage;
            }
            IEAnimateHPBarScale();
        }
        private void UpdateHPBarScaleGUI()
        {
            int hptemp = maxHP == 0 ? (hpVal == 0?1:hpVal) : maxHP;
            hpImage.fillAmount = (hpVal / hptemp);
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

        private IEnumerator IEAnimateHPBarScale()
        {
            while (hpImage.fillAmount < hpVal/maxHP)
            {
                UpdateHPBarScaleGUI();
                yield return new WaitForSeconds(0.01f);
            }
            while (hpImage.fillAmount > hpVal / maxHP)
            {
                UpdateHPBarScaleGUI();
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }
        #endregion
    }
}
