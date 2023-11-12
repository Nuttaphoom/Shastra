using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Vanaring
{
    [TrackColor(0.0f, 0.0f, 1)]
    [TrackBindingType(typeof(GameObject))]
    [TrackClipType(typeof(CustomAnimatorCallerClip))]
    public class CustomAnimatorCallerTrack : TrackAsset
    {
        public CustomAnimatorCallerClip GetAnimatorCallerClip()
        {
            return GetClips() as CustomAnimatorCallerClip ; 
        }
    }

   
}
