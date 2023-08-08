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


        private int hpVal;
        private int maxHpVal;
        private float maxEnergyVal = 100;
        private float lightVal = 50;
        private float darkVal = 50;

        private CombatEntity _combatEntity;

        private void OnEnable()
        {
            if(_combatEntity != null)
            {
                _combatEntity.SubOnDamageVisualEvent(OnHPModified);
            }
        }

        private void OnDisable()
        {
            _combatEntity.UnSubOnDamageVisualEvent(OnHPModified);
        }

        public void Init(float manaVal, string name, CombatEntity combatEntity)
        {
            _combatEntity = combatEntity;
            _combatEntity.SubOnDamageVisualEvent(OnHPModified);

            characterName.text = name;
            lightVal = manaVal;
            darkVal = maxEnergyVal - manaVal;

            hpVal = _combatEntity.StatsAccumulator.GetHPAmount();
            maxHpVal = _combatEntity.StatsAccumulator.GetHPAmount();
            UpdateHPScaleGUI();
        }
        private void UpdateHPScaleGUI()
        {
            hpBar.fillAmount = (float)hpVal / maxHpVal;
            hpNumText.text = hpVal + "/" + maxHpVal.ToString();
        }

        private void UpdateEnergyScaleGUI(int modifyMana)
        {
            manaBar.fillAmount = lightVal / maxEnergyVal;
            lightNumText.text = lightVal.ToString();
            darkNumText.text = darkVal.ToString();
        }

        public void OnHPModified(int damage)
        {
            hpVal = _combatEntity.StatsAccumulator.GetHPAmount();
            StopAllCoroutines();
            StartCoroutine(IEAnimateHPBarScale(hpVal));
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
