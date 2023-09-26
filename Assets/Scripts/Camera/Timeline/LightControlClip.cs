using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Vanaring
{
    [Serializable]
    public class LightControlClip : PlayableAsset, ITimelineClipAsset
    {

        [SerializeField]
        private LightControlBehavior template = new LightControlBehavior();
        public ClipCaps clipCaps {
            get { return ClipCaps.None; } }


        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<LightControlBehavior>.Create(graph, template);
        }
    }
}
