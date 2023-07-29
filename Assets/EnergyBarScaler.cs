using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring_DepaDemo
{
    public class EnergyBarScaler : MonoBehaviour
    {
        [SerializeField]
        private CombatEntity _owner  ;

        public TextMeshProUGUI lightNumText;
        public TextMeshProUGUI darkNumText;

        [Header("EnergyBarValue")]
        [SerializeField] private float lightScale;
        [SerializeField] private float darkScale;
        private float maxBarVal = 100.0f;
        [SerializeField] private Image lightImage;
        [SerializeField] private Image lightImageIcon;

        [Header("HPBarValue")]
        [SerializeField] private float hpScale;
        private float maxHP = 100.0f;
        [SerializeField] private Image hpImage;
        [SerializeField] private Image hpImageIcon;

        private void Awake()
        {
            _owner.SpellCaster.SubOnModifyEnergy(OnEnergyModified); 
        }

        private void OnEnergyModified(RuntimeMangicalEnergy.EnergySide side , int val)
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
            lightImage.fillAmount = (int.Parse(lightNumText.text) / maxBarVal);
            lightImageIcon.fillAmount = (int.Parse(lightNumText.text) / maxBarVal);
        }

        private void UpdateHPBarScaleGUI()
        {
            hpImage.fillAmount = (hpScale / maxHP);
            hpImageIcon.fillAmount = (hpScale / maxHP);
        }

        private void Start()
        {
            lightScale = 50;
            darkScale = 50;
            UpdateEnergyBarScaleGUI();
            UpdateHPBarScaleGUI();
            lightNumText.text = lightScale.ToString();
            darkNumText.text = darkScale.ToString();
            
        }
        private void Update()
        {
            //test
            if (Input.GetKeyDown("e"))
            {
                lightScaleIncrease(10);
                Debug.Log(lightScale);
            }
            if (Input.GetKeyDown("q"))
            {
                lightScaleDecrease(10);
                Debug.Log(lightScale);
            }
        }
        public void lightScaleIncrease(int val)
        {
            if(lightScale + val > 100)
            {
                Debug.Log("Value can't be exceed 100!");
                return;
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
