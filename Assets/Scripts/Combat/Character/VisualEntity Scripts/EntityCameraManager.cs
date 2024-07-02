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
            Debug.Log("enable face camera in " + gameObject.name); 
            CameraSetUPManager.Instance.EnableCamera(_faceCamera); 
        }

        public void EnableShoulderCamera()
        {
            CameraSetUPManager.Instance.EnableCamera(_shoulderCam);
        }
       
        public void DisableAllAttachedCamera()
        {
            if (_shoulderCam != null && _shoulderCam.gameObject.activeSelf)
                _shoulderCam.gameObject.SetActive(false);

            if (_faceCamera != null && _faceCamera.gameObject.activeSelf)
                _faceCamera.gameObject.SetActive(false);
        }
     
        public Vector3 GetRightVectorShoulderCam()
        {
            return _shoulderCam.transform.right; 
        }
    }
}
