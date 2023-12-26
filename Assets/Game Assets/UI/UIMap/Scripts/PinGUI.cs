using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class PinGUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject EventWindow;
        public void Init()
        {
            EventWindow.SetActive(false);
        }
    }
}
