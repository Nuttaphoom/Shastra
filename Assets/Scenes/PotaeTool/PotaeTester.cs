using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class PotaeTester : MonoBehaviour, IInputReceiver
    {
        public void RunInputUp()
        {
            //Debug.Log("RunInputUp");
            ReceiveKeys(KeyCode.W);
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
        }

        public void RunInputDown()
        {
            //Debug.Log("RunInputDown");
            ReceiveKeys(KeyCode.S);
            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);
        }

        public void ReceiveKeys(KeyCode key)
        {
            if (key == KeyCode.W)
            {
                //Debug.Log("KeyCode.W");
            }
            else if (key == KeyCode.S)
            {
                //Debug.Log("KeyCode.S");
            }
            else if (key == KeyCode.Space)
            {
                //Debug.Log("KeyCode.Space");
            }
            else if (key == KeyCode.Q)
            {
                //Debug.Log("KeyCode.Q");
            }
        }

    }
}
