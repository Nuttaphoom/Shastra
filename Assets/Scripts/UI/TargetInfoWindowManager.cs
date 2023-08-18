using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring_DepaDemo
{
    public class TargetInfoWindowManager : MonoBehaviour
    {
        [SerializeField]
        CombatEntity _combatEntity;

        StatusRuntimeEffect _currentStatusEffect;

        [Header("Target Infomation UI")]
        [SerializeField]
        private TextMeshProUGUI characterName;
        [SerializeField]
        private TextMeshProUGUI hpNumText;
        [SerializeField]
        private TextMeshProUGUI lightNumText;
        [SerializeField]
        private TextMeshProUGUI darkNumText;
        [SerializeField]
        private GameObject _infoWindow;

        [Header("Status Infomation UI")]
        [SerializeField]
        private TextMeshProUGUI statusName;
        [SerializeField]
        private TextMeshProUGUI statusDetail;
        [SerializeField]
        private TextMeshProUGUI TTLNum;
        [SerializeField]
        private TextMeshProUGUI StackNum;
        [SerializeField]
        private GameObject _statusInfoWindow;

        public static TargetInfoWindowManager instance = null;

        public static TargetInfoWindowManager GetInstance => instance;

        private void Awake()
        {
            instance = this;
            _infoWindow.SetActive(false);
            _statusInfoWindow.SetActive(false);
        }
        private void Start()
        {
            characterName.text = "Target Info";
            hpNumText.text = "0 / 0";
            lightNumText.text = "50";
            darkNumText.text = "50";

            statusName.text = "Status Name";
            statusDetail.text = "Status Description";
            TTLNum.text = "0";
            StackNum.text = "0";
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                ShowCombatEntityInfoUI(_combatEntity);
            }
        }

        public void ShowCombatEntityInfoUI(CombatEntity entities)
        {
            InfoUISetup(entities);
            _infoWindow.SetActive(true);
        }

        private void InfoUISetup(CombatEntity entities)
        {
            _combatEntity = entities;

            hpNumText.text = _combatEntity.StatsAccumulator.GetHPAmount().ToString();
            lightNumText.text = _combatEntity.StatsAccumulator.GetHPAmount().ToString();
            darkNumText.text = _combatEntity.StatsAccumulator.GetHPAmount().ToString();
        }

        public void ShowStatusEffectInfoUI(StatusRuntimeEffect effect)
        {
            StatusEffectUISetup(effect);
            _infoWindow.SetActive(true);
        }

        private void StatusEffectUISetup(StatusRuntimeEffect effect)
        {
            _currentStatusEffect = effect;

            statusName.text = _currentStatusEffect.GetStatusEffectDescription().FieldName;
            statusDetail.text = _currentStatusEffect.GetStatusEffectDescription().FieldDescription;
            TTLNum.text = _currentStatusEffect.TimeToLive.ToString();
            StackNum.text = _combatEntity.StatsAccumulator.GetHPAmount().ToString();
        }
    }
}
