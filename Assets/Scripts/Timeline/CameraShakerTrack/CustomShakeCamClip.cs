using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Vanaring
{
    [Serializable]
    public class CustomShakeCamClip : PlayableAsset, ITimelineClipAsset
    {

        [SerializeField]
        private CustomShakeCamControlBehavior template = new CustomShakeCamControlBehavior();
        public ClipCaps clipCaps {
            get { return ClipCaps.None; } }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<CustomShakeCamControlBehavior>.Create(graph, template);
        }
    }
}
