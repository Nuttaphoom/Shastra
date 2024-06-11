using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "UIInputSetSO", menuName = "ScriptableObject/UI/UIInputSetSO")]
    public class UIInputSetSO : ScriptableObject
    {
        [Serializable]
        struct ButtonUI 
        {
            [SerializeField]
            public InputCode _inputcode;

            [SerializeField]
            public Sprite _imageIcon;
        }

        [SerializeField]
        public ControlScheme _scheme;

        [SerializeField]
        private List<ButtonUI> buttonUIs;

        public Sprite GetSprite(InputCode code)
        {
            for (int i = 0; i < buttonUIs.Count; i++)
            {
                if (buttonUIs[i]._inputcode == code)
                {
                    return buttonUIs[i]._imageIcon;
                }
            }

            Debug.LogError("NULL inside");
            return null;
        }
    }
}
