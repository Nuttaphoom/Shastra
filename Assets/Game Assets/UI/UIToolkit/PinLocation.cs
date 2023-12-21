using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class PinLocation : MonoBehaviour
    {
        [SerializeField]
        private GameObject EventWindow;
        public void Init()
        {
            EventWindow.SetActive(false);
        }
    }
}
