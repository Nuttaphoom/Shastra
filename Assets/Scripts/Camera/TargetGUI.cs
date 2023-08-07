using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class TargetGUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject targetGUI;

        public GameObject Init(Vector3 pos, Transform parent)
        {
            GameObject newTargetObj = Instantiate(targetGUI, pos, Quaternion.identity, parent);
            newTargetObj.SetActive(false);
            return newTargetObj;
        }

        //public void En
    }
}
