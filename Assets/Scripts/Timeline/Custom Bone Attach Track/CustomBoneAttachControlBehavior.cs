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
    public class CustomBoneAttachControlBehavior : PlayableBehaviour
    {
  
        private bool firstTime = false;

        CinemachineBasicMultiChannelPerlin _multiChannelPerlin; 
        public override void ProcessFrame(Playable playable, FrameData ifo, object playerData)
        {
            if (!firstTime)
            {
                firstTime = true;

                MonoBehaviour.FindObjectOfType<ActionTimelinePrefab>().RelocateTargetsToBone(true); 

            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable,info); 

            var duration = playable.GetDuration();
            //var delay = playable.GetDelay (); // probably used in some cases, but for now, just let it be
            var time = playable.GetTime();
            var delta = info.deltaTime;

            //if (info.evaluationType == FrameData.EvaluationType.Playback)
            //{
            //    var count = time + delta;

            //    if (count >= duration)
            //    {
            //        StopCamShake(); 
            //    }
            //}
        }

        

         
    }
}
