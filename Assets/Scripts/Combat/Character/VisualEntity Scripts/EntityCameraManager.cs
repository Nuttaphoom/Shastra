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

        [SerializeField]
        private CinemachineVirtualCamera _shoulderCam ;
        public void EnableFaceCamera()
        {
            CameraSetUPManager.Instance.EnableCamera(_faceCamera); 
        }

        public void EnableShoulderCamera()
        {
            CameraSetUPManager.Instance.EnableCamera(_shoulderCam);
        }
    }
}
