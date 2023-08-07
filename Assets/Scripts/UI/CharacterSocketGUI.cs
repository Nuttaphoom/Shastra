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


        private float hpVal;
        private float maxHpVal;
        private float maxManaVal = 100;
        private float lightVal = 50;
        private float darkVal = 50;

        private CombatEntity _caster;

        private void OnEnable()
        {
            if(_caster != null)
            {
                _caster.SubOnDamageVisualEvent(UpdateHPScaleGUI);
            }
        }

        private void OnDisable()
        {
            _caster.UnSubOnDamageVisualEvent(UpdateHPScaleGUI);
        }

        public void Init(float manaVal, string name, CombatEntity combatEntity)
        {
            _caster = combatEntity;
            _caster.SubOnDamageVisualEvent(UpdateHPScaleGUI);
            characterName.text = name;
            hpVal = _caster.StatsAccumulator.GetHPAmount();
            maxHpVal = _caster.StatsAccumulator.GetHPAmount();
            lightVal = manaVal;
            darkVal = maxManaVal - manaVal;
            UpdateHPScaleGUI(0);
            UpdateManaScaleGUI(0);
        }
        private void Update()
        {
            //if (Input.GetKeyDown("e"))
            //{
            //    Debug.Log("E");
            //    OnHPModify(Random.Range(10, 50));
            //}
            //if (Input.GetKeyDown("q"))
            //{
            //    Debug.Log("Q");
            //    OnHPModify(Random.Range(55, 120));
            //}
            //UpdateHPScaleGUI(0);
            //_caster.StatsAccumulator.GetHPAmount();
        }
        private void Awake()
        {
            //hpVal = Random.Range(1, 100);
        }
        private void UpdateHPScaleGUI(int damage)
        {
            Debug.Log("Update CharacterGUI: HP");
            if(hpVal - damage < 0)
            {
                hpVal = 0;
            }
            else
            {
                hpVal -= damage;
            }
            hpBar.fillAmount = hpVal / maxHpVal;
            hpNumText.text = hpVal + "/" + maxHpVal.ToString();
        }

        private void UpdateManaScaleGUI(int modifyMana)
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
                UpdateHPScaleGUI(0);
                yield return new WaitForSeconds(0.01f);
            }
            while (hpVal > currentHP)
            {
                hpVal -= 1;
                UpdateHPScaleGUI(0);
                yield return new WaitForSeconds(0.01f);
            }
            yield return null;
        }

        
    }
}
