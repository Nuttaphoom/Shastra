using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Vanaring
{
    [TrackColor(0.0f, 0.0f, 1)]
    [TrackBindingType(typeof(GameObject))]
    [TrackClipType(typeof(CustomEntityMeshHiddenClip))]
    public class CustomEntityMeshHiddenTrack : TrackAsset
    {
        
        public string TrackName = "Caster" ;
        public CustomEntityMeshHiddenClip GetAnimatorCallerClip()
        {
            return GetClips() as CustomEntityMeshHiddenClip; 
        }
    }

   
}
