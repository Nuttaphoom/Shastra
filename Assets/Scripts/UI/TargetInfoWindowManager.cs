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

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private GameObject _uiRotator;

        [Header("Target Infomation UI")]
        [SerializeField]
        private TextMeshProUGUI characterName;
        [SerializeField]
        private Image targetIcon;
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
        private Image statusIcon;
        [SerializeField]
        private TextMeshProUGUI statusDetail;
        [SerializeField]
        private TextMeshProUGUI TTLNum;
        [SerializeField]
        private TextMeshProUGUI StackNum;
        [SerializeField]
        private GameObject _statusInfoWindow;


        private List<GameObject> _ClonedGameObject;

        public static TargetInfoWindowManager instance = null;

        public static TargetInfoWindowManager GetInstance => instance;

        private void Awake()
        {
            instance = this;
            _infoWindow.SetActive(false);
            _statusInfoWindow.SetActive(false);
            _ClonedGameObject = new List<GameObject>(4);
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
            ClearClonedUI();
            InfoUISetup(entities);
            _infoWindow.SetActive(true);

            _animator.SetTrigger("OnPlay");

        }
        public void HideCombatEntityInfoUI()
        {
            _infoWindow.SetActive(false);
        }
        private void ClearClonedUI()
        {
            foreach (GameObject eIconObj in _ClonedGameObject)
            {
                Destroy(eIconObj);
            }
        }

        private void InfoUISetup(CombatEntity entities)
        {
            _combatEntity = entities;

            characterName.text = _combatEntity.CharacterSheet.CharacterName;
            targetIcon.sprite = _combatEntity.CharacterSheet.GetCharacterIcon;
            hpNumText.text = _combatEntity.StatsAccumulator.GetHPAmount().ToString();
            lightNumText.text = _combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy).ToString();
            darkNumText.text = _combatEntity.SpellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy).ToString();

            HideStatusEffectInfoUI();

            createStatusUI();
        }

        private void createStatusUI()
        {
            int count = 0;
            foreach (KeyValuePair<string, List<StatusRuntimeEffect>> entry in _combatEntity.GetStatusEffectHandler().Effects)
            {
                if (entry.Value != null && entry.Value.Count != 0)
                {
                    TargetStatusIconUI newIcon = Instantiate(_IconSample, _IconSample.transform.position, _IconSample.transform.rotation);
                    if (count >= _IconPos.Length)
                    {
                        count = 0;
                    }
                    newIcon.transform.SetParent( _IconLayout.transform ) ;
                    newIcon.transform.localPosition = _IconPos[count].localPosition;
                    newIcon.transform.localScale = _IconPos[count].localScale;
                    Image iconImage = newIcon.GetComponent<Image>();
                    iconImage.sprite = entry.Value[0].GetStatusEffectDescription().FieldImage;

                    newIcon.Init(this, entry.Value[0]);
                    newIcon.gameObject.SetActive(true);

                    _ClonedGameObject.Add(newIcon.gameObject);

                    count++;
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

            statusIcon.sprite = _currentStatusEffect.GetStatusEffectDescription().FieldImage;
            statusName.text = _currentStatusEffect.GetStatusEffectDescription().FieldName;
            statusDetail.text = _currentStatusEffect.GetStatusEffectDescription().FieldDescription;
            if (!_currentStatusEffect.IsInfiniteTTL)
            {
                TTLNum.text = " TTL :   " + _currentStatusEffect.TimeToLive.ToString();
            }
            else
            {
                TTLNum.text = "";
            }

            foreach (KeyValuePair<string, List<StatusRuntimeEffect>> entry in effects)
            {
                if (entry.Value != null && entry.Value.Count != 0 && entry.Key == _currentStatusEffect.StackInfo.StackID().ToString() )
                {
                    if (!entry.Value[0].StackInfo.Stackable)
                    {
                        StackNum.text = "";
                    }
                    else
                    {
                        StackNum.text = " Stack : " + entry.Value.Count  ; 
                    }
                }
            }
        }

        //private IEnumerator UIRotateIn(float maxtime, int frame, float angle)
        //{
        //    float currRotation = angle;
        //    _uiRotator.transform.localRotation = Quaternion.Euler(0, 0, currRotation);
        //    for (int i = 0; i< frame; i++)
        //    {
        //        currRotation -= (angle) / frame;
        //        _uiRotator.transform.localRotation = Quaternion.Euler(0, 0, currRotation);
        //        yield return new WaitForSeconds(maxtime / frame);
        //    }
        //}

    }
}
