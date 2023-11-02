using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring 
{
    public class TargetGUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject targetGUI;
        [SerializeField]
        private GameObject _breakTargetGUI;
        [SerializeField]
        private GameObject _weakTargetGUI;

        private void Awake()
        {
            if (_breakTargetGUI == null)
                throw new System.Exception("BreakTargetGUI can not be null");

            if (targetGUI == null)
                throw new System.Exception("targetGUI can not be null");

            return; 

            //Dont check for weak for now as I complicates the system 
            if (_weakTargetGUI == null)
                throw new System.Exception("_weakTargetGUI can not be null");
        }

        public GameObject InstantiateTargetGUI(Vector3 pos, Transform parent)
        {
            GameObject newTargetObj = Instantiate(targetGUI, pos, Quaternion.identity, parent);
            newTargetObj.SetActive(false);
            return newTargetObj;
        }
        public GameObject InstantiateBreakGUI(Vector3 pos, Transform parent)
        {
            GameObject newTargetObj = Instantiate(_breakTargetGUI, pos, Quaternion.identity, parent);
            newTargetObj.SetActive(false);
            return newTargetObj;
        }
        public GameObject InstantiateWeakGUI(Vector3 pos, Transform parent)
        {
            GameObject newTargetObj = Instantiate(_weakTargetGUI, pos, Quaternion.identity, parent);
            newTargetObj.SetActive(false);
            return newTargetObj;
        }
        //public void En
    }
}
