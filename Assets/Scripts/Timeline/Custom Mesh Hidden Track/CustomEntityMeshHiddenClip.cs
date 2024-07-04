using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Vanaring
{
    [Serializable]
    public class CustomEntityMeshHiddenClip : PlayableAsset, ITimelineClipAsset
    {

        [SerializeField]
        private CustomEntityMeshHiddenControlBehavior template = new CustomEntityMeshHiddenControlBehavior();
        public ClipCaps clipCaps {
            get { return ClipCaps.None; } }


        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<CustomEntityMeshHiddenControlBehavior>.Create(graph, template);
        }
    }
}
