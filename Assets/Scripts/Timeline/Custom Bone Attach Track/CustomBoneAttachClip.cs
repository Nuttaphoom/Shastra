using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Vanaring
{
    [Serializable]
    public class CustomBoneAttachClip : PlayableAsset, ITimelineClipAsset
    {

        [SerializeField]
        private CustomBoneAttachControlBehavior template = new CustomBoneAttachControlBehavior();
        public ClipCaps clipCaps {
            get { return ClipCaps.None; } }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<CustomBoneAttachControlBehavior>.Create(graph, template);
        }
    }
}
