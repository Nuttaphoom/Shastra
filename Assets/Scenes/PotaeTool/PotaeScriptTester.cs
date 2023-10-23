using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class PotaeScriptTester : MonoBehaviour
    {
        public Transform testUI;

        public Transform target;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Vector3 screen = UISpaceSingletonHandler.ObjectToUISpace(target);
                testUI.position = screen;
            }
        }
    }
}
