using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class InputActionReader : MonoBehaviour
    {
        public void InputCall(string str)
        {
            CentralInputReceiver.Instance().TransmitInput(str);
        }


    }
}