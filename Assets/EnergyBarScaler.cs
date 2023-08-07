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
        
        private float maxBarVal = 100.0f;
        [SerializeField] private Image lightImage;

        [Header("HPBarValue")]
        private float hpScale;
        private float maxHP = 100.0f;
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

        }

        private void OnDisable()
        {
            _owner.SpellCaster.UnSubOnModifyEnergy(OnEnergyModified);
        }

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
            lightImage.fillAmount = (int.Parse(lightNumText.text) / maxBarVal);
        }

        private void UpdateHPBarScaleGUI()
        {
            hpImage.fillAmount = (hpScale / maxHP);
        }

         
        private void Update()
        {
            
        }
        public void lightScaleIncrease(int val)
        {
            
            if(lightScale + val > 100)
            {
                //What ??? -- Arm
                //Debug.Log("Value can't be exceed 100!");
                //return;

                val = (int) (100 - lightScale); 
            }
            lightScale += val;
            StartCoroutine(IEAnimateBarScale());
            //UpdateEnergyBarScaleGUI();
        }
        public void lightScaleDecrease(int val)
        {
            if (lightScale - val < 0)
            {
                Debug.Log("Value can't be lower than 0!");
                return;
            }
            lightScale -= val;
            StartCoroutine(IEAnimateBarScale());
            //UpdateEnergyBarScaleGUI();
        }

        private IEnumerator IEAnimateBarScale()
        {
            while (int.Parse(lightNumText.text) < lightScale)
            {
                lightNumText.text = (int.Parse(lightNumText.text) + 1).ToString();
                darkNumText.text = (int.Parse(darkNumText.text) - 1).ToString();
                UpdateEnergyBarScaleGUI();
                yield return new WaitForSeconds(0.1f);
            }
            while (int.Parse(lightNumText.text) > lightScale)
            {
                lightNumText.text = (int.Parse(lightNumText.text) - 1).ToString();
                darkNumText.text = (int.Parse(darkNumText.text) + 1).ToString();
                UpdateEnergyBarScaleGUI();
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }
}
