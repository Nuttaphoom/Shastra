using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Vanaring
{
    public class InputTester : MonoBehaviour
    {
        private Vector2 moveInputValue;
        private void OnNavigate(InputValue value) 
        {
            moveInputValue = value.Get<Vector2>();
            Debug.Log(moveInputValue);
        }

        private void OnSelect()
        {
            Debug.Log("Select");
        }
    }
}
