using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vanaring 
{
    public class UISpaceSingletonHandler    
    {
        public static Vector3 ObjectToUISpace(Transform objtransform)
        {
            return Camera.main.WorldToScreenPoint(objtransform.position); 
        }
    }
}
