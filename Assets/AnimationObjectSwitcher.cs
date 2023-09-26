using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class AnimationObjectSwitcher : MonoBehaviour
    {
        public GameObject box1; // Reference to the first object (box1)
        public GameObject box2; // Reference to the second object (box2)
        public Transform emptyObject; // Reference to the empty object (EmptyObject)

        private void Start()
        {
            // Disable both objects at the start
            box1.SetActive(false);
            box2.SetActive(false);
        }

        public void SwitchToBox1()
        {
            // Ensure the proper object (box1) is active and positioned at the EmptyObject
            box2.SetActive(false);
            box1.SetActive(true);
            box1.transform.position = emptyObject.position;
            box1.transform.rotation = emptyObject.rotation;
        }

        public void SwitchToBox2()
        {
            // Ensure the proper object (box2) is active and positioned at the EmptyObject
            box1.SetActive(false);
            box2.SetActive(true);
            box2.transform.position = emptyObject.position;
            box2.transform.rotation = emptyObject.rotation;
        }
    }
}
