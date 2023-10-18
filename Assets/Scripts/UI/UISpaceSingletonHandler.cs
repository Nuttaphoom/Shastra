using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vanaring 
{
    public class UISpaceSingletonHandler : MonoBehaviour
    {
        public static UISpaceSingletonHandler instance = null;

        private void Awake()
        {
            instance = this;
        }

        public Vector3 ObjectToUISpace(Transform objtransform)
        {
            return Camera.main.WorldToScreenPoint(objtransform.position); 
        }
    }
}
