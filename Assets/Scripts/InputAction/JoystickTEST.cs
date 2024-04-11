using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vanaring
{
    public class JoystickTEST : MonoBehaviour
    {
        private JoystickControl joystickControl;
        private InputAction stick;

        public JoystickTEST()
        {

        }

        private void Awake()
        {
            joystickControl = new JoystickControl();
        }

        private void OnEnable()
        {
            stick = joystickControl.TurnBasedCombat.ScrollOrSelect;
            stick.Enable();

            joystickControl.TurnBasedCombat.ScrollOrSelect.performed += DoScroll;
            joystickControl.TurnBasedCombat.ScrollOrSelect.Enable();
        }

        private void DoScroll(InputAction.CallbackContext obj)
        {
            if (obj.performed)
            {
                Debug.Log(obj);
            }
        }

        private void OnDisable()
        {
            stick.Disable();
            joystickControl.TurnBasedCombat.ScrollOrSelect.Disable();
        }
    }
}
