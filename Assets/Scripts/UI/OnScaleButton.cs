using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class OnScaleButton : MonoBehaviour
    {
        [SerializeField] private RectTransform buttonGFX;

        public void OnExpand()
        {
            //Debug.Log("Expand");
            buttonGFX.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        public void OnShrink()
        {
            //Debug.Log("Shrink");
            buttonGFX.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
