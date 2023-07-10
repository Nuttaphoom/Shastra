using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Vanaring_DepaDemo
{
    //[RequireComponent(typeof(CinemachineVirtualCamera))]
    public class DynamicCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform focusObjectTransform;

        private CinemachineVirtualCamera _cmv;
        private void Awake()
        {
            Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out var brain);
            if (brain == null)
            {
                brain = Camera.main.gameObject.AddComponent<CinemachineBrain>();
            }
            //brain.m_DefaultBlend.m_Time = 1;
            //brain.m_ShowDebugText = true;

            _cmv = gameObject.AddComponent<CinemachineVirtualCamera>();
            _cmv.Follow = focusObjectTransform;
            _cmv.LookAt = focusObjectTransform;
            _cmv.Priority = 1;

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        

    }
}
