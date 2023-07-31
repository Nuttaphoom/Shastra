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
        private TextMeshProUGUI lightNum;
        [SerializeField]
        private TextMeshProUGUI darkNum;

        private float hpVal = 100;
        private float maxHpVal = 100;
        private float maxManaVal = 100;
        private float lightVal = 50;
        private float darkVal = 50;

        private CombatEntity _caster;

        public void Init(int hpVal, float manaVal, string name, CombatEntity combatEntity)
        {
            _caster = combatEntity;
            characterName.text = name;
            this.hpVal = _caster.StatsAccumulator.GetHPAmount();
            //Debug.Log(_caster.StatsAccumulator.GetHPAmount());
            lightVal = manaVal;
            darkVal = maxManaVal - manaVal;
            UpdateHPScaleBar();
            UpdateManaScaleBar();
        }
        private void Update()
        {
            if (Input.GetKeyDown("e"))
            {
                Debug.Log("E");
                OnHPModify(10);
            }
            if (Input.GetKeyDown("q"))
            {
                Debug.Log("Q");
                OnHPModify(100);
            }
            //_caster.StatsAccumulator.GetHPAmount();
        }
        private void Awake()
        {
            Debug.Log(hpVal);
            //lightVal = 50;
            //darkVal = 50;
            //hpVal = 100;
            //maxHpVal = 100;
        }
        private void UpdateHPScaleBar()
        {
            hpBar.fillAmount = hpVal / maxHpVal;
        }

        private void UpdateManaScaleBar()
        {
            manaBar.fillAmount = lightVal / maxManaVal;
            lightNum.text = lightVal.ToString();
            darkNum.text = darkVal.ToString();
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
                UpdateHPScaleBar();
                yield return new WaitForSeconds(0.01f);
            }
            while (hpVal > currentHP)
            {
                hpVal -= 1;
                UpdateHPScaleBar();
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }

        
    }
}
