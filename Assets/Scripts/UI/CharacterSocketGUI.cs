using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Vanaring_DepaDemo
{
    public class CharacterSocketGUI : MonoBehaviour
    {
        [SerializeField]
        private Image characterImg;
        [SerializeField]
        private Image hpBar;
        [SerializeField]
        private Image manaBar;
        [SerializeField]
        private TextMeshProUGUI characterName;
        [SerializeField]
        private TextMeshProUGUI lightNumText;
        [SerializeField]
        private TextMeshProUGUI darkNumText;
        [SerializeField]
        private TextMeshProUGUI hpNumText;

        [SerializeField]
        private StatusWindowManager _statusWindow;

        private int hpVal;
        private int maxHpVal;
        private float maxEnergyVal = 100;
        private float lightVal = 50;
        private float darkVal = 50;

        private CombatEntity _combatEntity;

        private void Awake()
        {
            lightVal = 50;
            darkVal = 50;
            if (lightNumText != null && darkNumText != null)
            {
                lightNumText.text = lightVal.ToString();
                darkNumText.text = darkVal.ToString();
            }
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

        public void Init(float manaVal, string name, CombatEntity combatEntity)
        {
            _combatEntity = combatEntity;
            _combatEntity.SubOnDamageVisualEvent(OnHPModified);
            _combatEntity.SpellCaster.SubOnModifyEnergy(OnEnergyModified);

            characterName.text = name;
            hpVal = _combatEntity.StatsAccumulator.GetHPAmount();
            maxHpVal = _combatEntity.StatsAccumulator.GetHPAmount();
            UpdateHPScaleGUI();

            _statusWindow.SetCombatEntity(combatEntity);
        }
        private void UpdateHPScaleGUI()
        {
            hpBar.fillAmount = (float)hpVal / maxHpVal;
            hpNumText.text = hpVal + "/" + maxHpVal.ToString();
        }

        private void UpdateEnergyScaleGUI()
        {
            manaBar.fillAmount = (int.Parse(lightNumText.text) / maxEnergyVal);
        }

        private void OnHPModified(int damage)
        {
            //Debug.Log("hammmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm");
            hpVal = _combatEntity.StatsAccumulator.GetHPAmount();
            StopAllCoroutines();
            StartCoroutine(IEAnimateHPBarScale(hpVal));
        }

        private void OnEnergyModified(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int val)
        {
            //Debug.Log("mammmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm");
            if (side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                Debug.Log("INCREASEEEEEEEEEEEEEEE");
                lightScaleIncrease(val);
            }
            else
            {
                Debug.Log("DECREASEEEEEEEEEEEEEEE");
                lightScaleDecrease(val);
            }
        }

        public void lightScaleIncrease(int val)
        {
            if (lightVal + val > 100)
            {
                val = (int)(100 - lightVal);
            }
            lightVal += val;
            StartCoroutine(IEAnimateEnergyBarScale());
        }
        public void lightScaleDecrease(int val)
        {
            if (lightVal - val < 0)
            {
                Debug.Log("Value can't be lower than 0!");
                return;
            }
            lightVal -= val;
            StartCoroutine(IEAnimateEnergyBarScale());
        }

        private IEnumerator IEAnimateEnergyBarScale()
        {
            while (int.Parse(lightNumText.text) < lightVal)
            {
                lightNumText.text = (int.Parse(lightNumText.text) + 1).ToString();
                darkNumText.text = (int.Parse(darkNumText.text) - 1).ToString();
                UpdateEnergyScaleGUI();
                yield return new WaitForSeconds(0.01f);
            }
            while (int.Parse(lightNumText.text) > lightVal)
            {
                lightNumText.text = (int.Parse(lightNumText.text) - 1).ToString();
                darkNumText.text = (int.Parse(darkNumText.text) + 1).ToString();
                UpdateEnergyScaleGUI();
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }

        private IEnumerator IEAnimateHPBarScale(int hpVal)
        {
            while (manaBar.fillAmount < (float)hpVal/maxHpVal)
            {
                hpVal += 1;
                UpdateHPScaleGUI();
                yield return new WaitForSeconds(0.01f);
            }
            while ((float)hpVal / maxHpVal > hpVal)
            {
                hpVal -= 1;
                UpdateHPScaleGUI();
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }
    }
}
