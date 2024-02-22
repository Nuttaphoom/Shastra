using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Vanaring
{
    public class IAwakeableEntity : MonoBehaviour
    {
        public void IAwake()
        {
            foreach (var awakeable in GetComponents<IAwakeable>())
            {
                awakeable.IAwake();
            }
        }
    }
}
