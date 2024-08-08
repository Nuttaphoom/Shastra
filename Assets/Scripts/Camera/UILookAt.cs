using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring 
{
    public class UILookAt : MonoBehaviour
    {
        private Transform mainCameraTransform;
        public bool isTargetGUI = false;

        private void Start()
        {
            mainCameraTransform = Camera.main.transform;
            if (isTargetGUI)
            {
                transform.position = Vector3.MoveTowards(transform.position, mainCameraTransform.position, 0.3f);
            }
            
        }
        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}
