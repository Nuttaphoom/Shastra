using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class ButtonSelector : MonoBehaviour
    {
        [SerializeField]
        private Button primaryButton;
        void Start()
        {
            primaryButton.Select();
        }
    }
}
