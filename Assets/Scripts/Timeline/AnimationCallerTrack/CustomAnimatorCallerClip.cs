using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Vanaring
{
    [Serializable]
    public class CustomAnimatorCallerClip : PlayableAsset, ITimelineClipAsset
    {

        [SerializeField]
        private CustomAnimatorCallerControlBehavior template = new CustomAnimatorCallerControlBehavior();
        public ClipCaps clipCaps {
            get { return ClipCaps.None; } }


        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<CustomAnimatorCallerControlBehavior>.Create(graph, template);
        }
    }
}
