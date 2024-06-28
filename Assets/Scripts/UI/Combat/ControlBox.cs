using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class ControlBox : MonoBehaviour
    {
        [SerializeField] private Image fillImg;
        public void SetActiveImage(bool isShow)
        {
            fillImg.gameObject.SetActive(isShow);
        }
    }
}
