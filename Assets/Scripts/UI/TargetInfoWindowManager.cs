using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring_DepaDemo
{
    public class TargetInfoWindowManager : MonoBehaviour
    {
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
        private TargetStatusIconUI _IconSample;
        [SerializeField]
        private Transform _IconLayout;
        [SerializeField]
        private Transform[] _IconPos;
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
            TTLNum.text = "    TTL :   -1";
            StackNum.text = "   Stack : -1";
        }

        public void ShowCombatEntityInfoUI(CombatEntity entities)
        {
            InfoUISetup(entities);
            _infoWindow.SetActive(true);
        }
        public void HideCombatEntityInfoUI()
        {
            _infoWindow.SetActive(false);
        }

        private void InfoUISetup(CombatEntity entities)
        {
            _combatEntity = entities;

            hpNumText.text = _combatEntity.StatsAccumulator.GetHPAmount().ToString();
            lightNumText.text = _combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy).ToString();
            darkNumText.text = _combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy).ToString();

            createStatusUI();
        }

        private void createStatusUI()
        {
            foreach (KeyValuePair<string, List<StatusRuntimeEffect>> entry in _combatEntity.GetStatusEffectHandler().Effects)
            {
                if (entry.Value != null && entry.Value.Count != 0)
                {
                    TargetStatusIconUI newIcon = Instantiate(_IconSample, _IconSample.transform.position, _IconSample.transform.rotation);
                    int count = 0;
                    if (count >= _IconPos.Length)
                    {
                        count = 0;
                    }

                    newIcon.transform.parent = _IconLayout.transform;
                    newIcon.transform.localScale = _IconPos[count].transform.localScale;

                    newIcon.Init(this, entry.Value[0]);
                    newIcon.gameObject.SetActive(true);
                }
            }
        }

        public void ShowStatusEffectInfoUI(StatusRuntimeEffect effect)
        {
            StatusEffectUISetup(_combatEntity.GetStatusEffectHandler().Effects, effect);
            _statusInfoWindow.SetActive(true);
        }

        public void HideStatusEffectInfoUI()
        {
            _statusInfoWindow.SetActive(false);
        }

        private void StatusEffectUISetup(Dictionary<string, List<StatusRuntimeEffect>> effects, StatusRuntimeEffect buff)
        {
            _currentStatusEffect = buff;

            statusName.text = _currentStatusEffect.GetStatusEffectDescription().FieldName;
            statusDetail.text = _currentStatusEffect.GetStatusEffectDescription().FieldDescription;
            if (!_currentStatusEffect.IsInfiniteTTL)
            {
                TTLNum.text = "    TTL :   " + _currentStatusEffect.TimeToLive.ToString();
            }
            else
            {
                TTLNum.text = "";
            }

            foreach (KeyValuePair<string, List<StatusRuntimeEffect>> entry in effects)
            {
                if (entry.Value != null && entry.Value.Count != 0 && entry.Key == _currentStatusEffect.GetStatusEffectDescription().FieldName)
                {
                    if (!entry.Value[0].StackInfo.Stackable)
                    {
                        StackNum.text = "";
                    }
                    else
                    {
                        StackNum.text = "   Stack : " + entry.Value.Count;
                    }
                }
            }

        }

    }
}
