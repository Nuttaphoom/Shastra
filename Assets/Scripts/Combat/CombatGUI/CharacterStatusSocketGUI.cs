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

namespace Vanaring_DepaDemo
{
    public class CharacterStatusSocketGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private CharacterStatusWindowManager _windowManager;
        private StatusRuntimeEffect _effect;
        public void Init(CharacterStatusWindowManager manager, StatusRuntimeEffect effect)
        {
            _windowManager = manager;
            _effect = effect;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _windowManager.ShowStatusEffectInfoUI(_effect);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _windowManager.HideStatusEffectInfoUI();
        }
    }
}
