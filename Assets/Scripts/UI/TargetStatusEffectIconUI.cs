using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vanaring 
{
    public class TargetStatusIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TargetInfoWindowManager _windowManager;
        private StatusRuntimeEffect _effect;
        public void Init(TargetInfoWindowManager manager, StatusRuntimeEffect effect)
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
