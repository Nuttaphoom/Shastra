using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        private float hpVal;
        private float maxHpVal = 100;
        private float maxManaVal = 100;
        private float lightVal = 50;
        private float darkVal = 50;

        private CombatEntity _caster;

        public void Init(float manaVal, string name, CombatEntity combatEntity)
        {
            _caster = combatEntity;
            characterName.text = name;
            this.maxHpVal = _caster.StatsAccumulator.GetHPAmount();
            //Debug.Log(_caster.StatsAccumulator.GetHPAmount());
            lightVal = manaVal;
            darkVal = maxManaVal - manaVal;
            UpdateHPScaleGUI();
            UpdateManaScaleGUI();
        }
        private void Update()
        {
            if (Input.GetKeyDown("e"))
            {
                Debug.Log("E");
                OnHPModify(Random.Range(10, 50));
            }
            if (Input.GetKeyDown("q"))
            {
                Debug.Log("Q");
                OnHPModify(Random.Range(55, 120));
            }
            UpdateHPScaleGUI();
            //_caster.StatsAccumulator.GetHPAmount();
        }
        private void Awake()
        {
            hpVal = Random.Range(1, 100);
            Debug.Log(hpVal);
            //lightVal = 50;
            //darkVal = 50;
            //hpVal = 100;
            //maxHpVal = 100;
        }
        private void UpdateHPScaleGUI()
        {
            hpBar.fillAmount = _caster.StatsAccumulator.GetHPAmount() / maxHpVal;
            hpNumText.text = _caster.StatsAccumulator.GetHPAmount() + "/" + maxHpVal.ToString();
        }

        private void UpdateManaScaleGUI()
        {
            manaBar.fillAmount = lightVal / maxManaVal;
            lightNumText.text = lightVal.ToString();
            darkNumText.text = darkVal.ToString();
        }

        public void OnHPModify(int currentHP)
        {
            StopAllCoroutines();
            StartCoroutine(IEAnimateHPBarScale(currentHP));
        }

        private IEnumerator IEAnimateHPBarScale(int currentHP)
        {
            while (hpVal < currentHP)
            {
                hpVal += 1;
                UpdateHPScaleGUI();
                yield return new WaitForSeconds(0.01f);
            }
            while (hpVal > currentHP)
            {
                hpVal -= 1;
                UpdateHPScaleGUI();
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }

        
    }
}
