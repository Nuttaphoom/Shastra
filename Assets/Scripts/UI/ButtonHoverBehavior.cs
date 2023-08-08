using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class ButtonHoverBehavior : MonoBehaviour
    {
        public ButtonHover buttonHoverDetector;

        private void Update()
        {
            if (buttonHoverDetector.IsHovering)
            {
                // Button is being hovered over
                Debug.Log("Button is being hovered over.");
            }
            else
            {
                // Button is not being hovered over
                Debug.Log("Button is not being hovered over.");
            }
        }
    }
}
