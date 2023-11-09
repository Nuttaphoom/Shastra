using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class TestSlotMan : MonoBehaviour
    {
        [SerializeField]
        List<TestMeshMod> _meshmods;

        private void Awake()
        {
            for (int  i = 0; i < _meshmods.Count; i++)
            {
                _meshmods[i].SetSLOTNO(i); // = = i; 
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
