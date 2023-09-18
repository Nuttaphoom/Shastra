using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Vanaring 
{
    public class CameraShake : MonoBehaviour
    {
        private float shakeDuration = 0.2f;
        private float shakeAmplitude = 1.2f;
        private float shakeFrequency = 2.0f;

        private CinemachineVirtualCamera virtualCamera;
        private float shakeTimer = 0.0f;
        private bool isShaking = false;

        private void Start()
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public void SetShakedCamera(CinemachineVirtualCamera virtualCamera)
        {
            this.virtualCamera = virtualCamera;
        }

        public void Shake()
        {
            CameraShake cameraShake = virtualCamera.GetComponent<CameraShake>();
            cameraShake.ShakeCamera();
        }

        public void ShakeCamera()
        {
            Debug.Log("----------------------Skaerrrrrrrrrr");
            var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = shakeAmplitude;
            noise.m_FrequencyGain = shakeFrequency;
            StartCoroutine(ShakeDelay());
            noise.m_AmplitudeGain = 0.0f;
            noise.m_FrequencyGain = 0.0f;
        }
        private IEnumerator ShakeDelay()
        {
            
            yield return new WaitForSeconds(0.3f);
            if (!isShaking)
            {
                
                isShaking = true;
                shakeTimer = shakeDuration;

                //var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                //noise.m_AmplitudeGain = shakeAmplitude;
                //noise.m_FrequencyGain = shakeFrequency;
            }
            yield return null;
        }

        private void Update()
        {
            //if (isShaking)
            //{
            //    shakeTimer -= Time.deltaTime;
            //    if (shakeTimer <= 0.0f)
            //    {
            //        var noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //        noise.m_AmplitudeGain = 0.0f;
            //        noise.m_FrequencyGain = 0.0f;
            //        isShaking = false;
            //    }
            //}
            
        }
    }
}
