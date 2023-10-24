using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

namespace Vanaring 
{
    public class StatusSocketGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private TextMeshProUGUI _statusName;

        [SerializeField]
        private TextMeshProUGUI _statusDescription;

        [SerializeField]
        private TextMeshProUGUI _statusDurationUI;

        [SerializeField]
        private TextMeshProUGUI _statusStackUI;

        [SerializeField]
        private Image _statusImageIcon;

        [SerializeField]
        private Image _statusImage;

        [SerializeField]
        private GameObject _descriptionWindow;

        private StatusRuntimeEffect _statusRuntime;
        private CombatEntity _caster;

        public void Init(StatusRuntimeEffect description, CombatEntity combatEntity)
        {
            DescriptionBaseField temp = description.GetStatusEffectDescription();
            _statusName.text = temp.FieldName;
            _statusDescription.text = temp.FieldDescription;
            _statusImageIcon.sprite = temp.FieldImage;
            if (!description.Property.Stackable)
            {
                _statusStackUI.text = "";
            }
            else
            {
                _statusStackUI.text = "x" + 1;
            }
            if (!description.IsInfiniteTTL)
            {
                _statusDurationUI.text = description.TimeToLive.ToString();
            }
            else
            {
                _statusDurationUI.text = "";
            }

            _statusRuntime = description;
            this._caster = combatEntity;
        }
        public void ChangeBuffStack(StatusRuntimeEffect status, int stackcount)
        {
            if (!status.Property.Stackable)
            {
                _statusStackUI.text = "";
            }
            else
            {
                _statusStackUI.text = "x" + stackcount;
            }
        }
        public void AddGameObjectWindow(GameObject window)
        {
            _descriptionWindow = window;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _descriptionWindow.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _descriptionWindow.SetActive(false);
        }
    }
}
