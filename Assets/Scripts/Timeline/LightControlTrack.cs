using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Vanaring
{
    [TrackColor(255f/255f, 0f, 0f)]
    [TrackBindingType(typeof(Light))]
    [TrackClipType(typeof(LightControlClip))]
    public class LightControlTrack : TrackAsset
    {
        public LightControlClip GetLightClip()
        {
            return GetClips() as LightControlClip ; 
        }
    }

   
}
