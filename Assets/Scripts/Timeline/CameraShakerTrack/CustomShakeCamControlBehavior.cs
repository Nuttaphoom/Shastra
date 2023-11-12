using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Vanaring
{
    [Serializable]
    public class CustomShakeCamControlBehavior : PlayableBehaviour
    {
        [SerializeField]
        private float _amplitude = 0.0f ;

        [NoiseSettingsProperty]
        [SerializeField]
        private NoiseSettings _noiseSetting;

        private NoiseSettings _oldNoiseSetting; 
        private float _oldAmp = 0.0f ;
 
        private bool firstTime = false;

        CinemachineBasicMultiChannelPerlin _multiChannelPerlin; 
        public override void ProcessFrame(Playable playable, FrameData ifo, object playerData)
        {
            if (!firstTime)
            {
                firstTime = true;

                CinemachineVirtualCamera vm = playerData as CinemachineVirtualCamera;

                _multiChannelPerlin = vm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                _oldNoiseSetting =  _multiChannelPerlin.m_NoiseProfile ;
                _oldAmp = _multiChannelPerlin.m_AmplitudeGain;

                _multiChannelPerlin.m_AmplitudeGain = _amplitude;
                _multiChannelPerlin.m_NoiseProfile = _noiseSetting;
            }

            
 
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable,info); 

            var duration = playable.GetDuration();
            //var delay = playable.GetDelay (); // probably used in some cases, but for now, just let it be
            var time = playable.GetTime();
            var delta = info.deltaTime;

            if (info.evaluationType == FrameData.EvaluationType.Playback)
            {
                var count = time + delta;

                if (count >= duration)
                {
                    StopCamShake(); 
                }
            }
        }

        private void StopCamShake()
        {
            Debug.Log("stop cam shake");
            _multiChannelPerlin.m_AmplitudeGain = _oldAmp ;
            _multiChannelPerlin.m_NoiseProfile = _oldNoiseSetting ;
        }

         
    }
}
