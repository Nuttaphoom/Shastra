using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class VfxTelegraphySingletonHandler : MonoBehaviour
    {
        public static VfxTelegraphySingletonHandler instance = null;
        [SerializeField]
        private GameObject[] _vfxTelegraphPrefab;

        private void Awake()
        {
            instance = this;
        }
        public GameObject GetVfxTelegraphPrefab(int index)
        {
            if (index > _vfxTelegraphPrefab.Length || index < 0)
            {
                Debug.Log("illegal index for vfx prfab");
                return null;
            }
            return _vfxTelegraphPrefab[index];
        }
    }
}
