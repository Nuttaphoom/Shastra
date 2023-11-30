using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Vanaring
{
    public class EntityCameraManager : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera _faceCamera;

        public void EnableFaceCamera()
        {
            Debug.Log("enable face cam");
            CameraSetUPManager.Instance.EnableCamera(_faceCamera); 
        }
    }
}
