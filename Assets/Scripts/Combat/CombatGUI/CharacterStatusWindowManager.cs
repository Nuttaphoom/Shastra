using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vanaring_DepaDemo
{
    public class CharacterStatusWindowManager : MonoBehaviour
    {
        Dictionary<string, List<StatusRuntimeEffect>> _effects;

        StatusRuntimeEffect _currentStatusEffect;

        [SerializeField]
        private CharacterStatusSocketGUI _IconSample;
        [SerializeField]
        private Transform _IconLayout;
        [SerializeField]
        private Transform[] _IconPos;

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

        private void Awake()
        {
            _statusInfoWindow.SetActive(false);
            _ClonedGameObject = new List<GameObject>(3);
        }
        private void Start()
        { 
            statusName.text = "Status Name";
            statusDetail.text = "Status Description";
            TTLNum.text = "    TTL :   -1";
            StackNum.text = "   Stack : -1";
        }

        public void ShowStatusIconUI(Dictionary<string, List<StatusRuntimeEffect>> effects)
        {
            ClearClonedUI();
            StatusIconUISetup(effects);
        }
        private void ClearClonedUI()
        {
            foreach (GameObject eIconObj in _ClonedGameObject)
            {
                Destroy(eIconObj);
            }
        }

        private void StatusIconUISetup(Dictionary<string, List<StatusRuntimeEffect>> effects)
        {
            _effects = effects;

            HideStatusEffectInfoUI();

            createStatusUI();
        }

        private void createStatusUI()
        {
            int count = 0;
            foreach (KeyValuePair<string, List<StatusRuntimeEffect>> entry in _effects)
            {
                if (entry.Value != null && entry.Value.Count != 0)
                {
                    //Debug.Log("Created" + entry.Value[0].GetStatusEffectDescription().FieldName);
                    CharacterStatusSocketGUI newIcon = Instantiate(_IconSample, _IconSample.transform.position, _IconSample.transform.rotation);
                    if (count >= _IconPos.Length)
                    {
                        count = 0;
                    }
                    newIcon.transform.SetParent(_IconLayout.transform);
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
            StatusEffectUISetup(_effects, effect);
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
                if (entry.Value != null && entry.Value.Count != 0 && entry.Key == _currentStatusEffect.StackInfo.StackID().ToString())
                {
                    if (!entry.Value[0].StackInfo.Stackable)
                    {
                        StackNum.text = "";
                    }
                    else
                    {
                        StackNum.text = " Stack : " + entry.Value.Count;
                    }
                }
            }
        }
    }
}
