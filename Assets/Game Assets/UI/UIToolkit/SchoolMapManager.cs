using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class SchoolMapManager : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> pinTransformList = new List<Transform>();
        private List<PinLocation> pinObject = new List<PinLocation>();
        [SerializeField]
        private PinLocation pinTemplate;

        private void Start()
        {
            foreach (Transform pinTransform in pinTransformList)
            {
                PinLocation newPin = Instantiate(pinTemplate, pinTransform);
                newPin.Init();
                pinObject.Add(newPin);
            }
        }
    }
}
