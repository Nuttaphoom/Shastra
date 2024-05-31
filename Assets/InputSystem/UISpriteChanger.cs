using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class UISpriteChanger : MonoBehaviour
    {
        [SerializeField]
        private InputCode inputType;

        private Image _image;

        private void Awake()
        {
            CentralInputReceiver.Instance.SubOnControllerSchemeChange(ChangeSprite);
            _image = GetComponent<Image>();
        }

        private void ChangeSprite(ControlScheme scheme)
        {
            _image.sprite = CentralInputReceiver.Instance.GetUISprite(inputType);
        }
    }
}
