using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class ButtonMinimumAlpha : MonoBehaviour
    {
        void Start()
        {
            gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        }
    }
}
