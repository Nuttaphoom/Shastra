using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring_DepaDemo
{
    public class CustomAreaButton : MonoBehaviour
    {
        void Start()
        {
            this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        }
    }
}
