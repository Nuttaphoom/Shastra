using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Vanaring_DepaDemo
{
    public class CameraShake : MonoBehaviour
    {
        public float shakeDuration = 0.3f;
        public float shakeAmplitude = 1.2f;
        public float shakeFrequency = 2.0f;

        private CinemachineVirtualCamera virtualCamera;
        private float shakeTimer = 0.0f;
        private bool isShaking = false;

        private void Start()
        {
            //virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public void SetShakedCamera(CinemachineVirtualCamera virtualCamera)
        {
            this.virtualCamera = virtualCamera;
        }

        public void ShakeCamera()
        {
            if (!isShaking)
            {
                isShaking = true;
                shakeTimer = shakeDuration;
                var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                noise.m_AmplitudeGain = shakeAmplitude;
                noise.m_FrequencyGain = shakeFrequency;
            }
        }

        private void Update()
        {
            if (isShaking)
            {
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0.0f)
                {
                    var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    noise.m_AmplitudeGain = 0.0f;
                    noise.m_FrequencyGain = 0.0f;
                    isShaking = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CameraShake cameraShake = virtualCamera.GetComponent<CameraShake>();
                cameraShake.ShakeCamera();
            }
        }
    }
}
