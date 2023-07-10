using System.Collections;
using UnityEngine;
using Cinemachine;

namespace Vanaring_DepaDemo
{
    public class CameraTBManager : MonoBehaviour
    {
        public CinemachineVirtualCameraBase[] cameras;
        public GameObject[] targetCamPoints;
        public GameObject[] CharacterCamPoints;
        public GameObject Dolly;
        private int currentCamIndex;
        private int currentTargetIndex;
        private Coroutine pathCoroutine;

        void Start()
        {
            if (cameras.Length == 0 || targetCamPoints.Length == 0)
            {
                Debug.LogError("No cameras or target can be found");
                return;
            }

            currentCamIndex = 0;
            currentTargetIndex = 0;
            ActivateCamera(currentCamIndex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                currentCamIndex++;
                if (currentCamIndex >= cameras.Length)
                {
                    currentCamIndex = 0;
                }

                ActivateCamera(currentCamIndex);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentTargetIndex--;
                if (currentTargetIndex < 0)
                {
                    currentTargetIndex = targetCamPoints.Length - 1;
                }
                SwitchFollowTarget(currentTargetIndex);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                currentTargetIndex++;
                if (currentTargetIndex >= targetCamPoints.Length)
                {
                    currentTargetIndex = 0;
                }
                SwitchFollowTarget(currentTargetIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (pathCoroutine == null)
                {
                    pathCoroutine = StartCoroutine(AnimatePathPosition());
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchDolPosition(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchDolPosition(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchDolPosition(2);
            }
        }

        private void SwitchDolPosition(int i)
        {
            Dolly.transform.position = new Vector3(CharacterCamPoints[i].transform.position.x, Dolly.transform.position.y, Dolly.transform.position.z) ;
        }

        private void ActivateCamera(int index)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].gameObject.SetActive(i == index);
            }
        }

        private void SwitchFollowTarget(int index)
        {
            // Set the follow target of all cameras to the specified target
            for (int i = 0; i < cameras.Length; i++)
            {
                CinemachineVirtualCameraBase virtualCamera = cameras[i];
                if (virtualCamera != null)
                {
                    virtualCamera.LookAt = targetCamPoints[index].transform;
                }
            }
        }

        private IEnumerator AnimatePathPosition()
        {
            CinemachineVirtualCamera virtualCamera = cameras[currentCamIndex] as CinemachineVirtualCamera;
            CinemachineTrackedDolly dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
            if (dolly != null)
            {
                float pathPosition = 0f;
                float desiredPathPosition = 1f;

                while (pathPosition < desiredPathPosition)
                {
                    pathPosition += 0.02f;
                    dolly.m_PathPosition = pathPosition;
                    yield return null;
                }
                yield return new WaitForSeconds(2.0f);
                dolly.m_PathPosition = 0f;
            }

            pathCoroutine = null;
        }
    }
}
