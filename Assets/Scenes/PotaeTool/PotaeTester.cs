using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class PotaeTester : MonoBehaviour
    {
        private static Stack<IInputReceiver> _receiverStack = new Stack<IInputReceiver>();
        public void RunInputUp(string str)
        {

            //CentralInputReceiver.Instance().TransmitInput(str);
        }

        public void RunInputDown(string str)
        {
            //CentralInputReceiver.Instance().TransmitInput(str);
        }
    }
}
