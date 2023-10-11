using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Vanaring
{
    public class TimelineBehavior : MonoBehaviour
    {
        public PlayableDirector director;
        public GameObject newObjectToAnimate;

        public Transform emptyTranform;

        public List<string> trackNameList = new List<string>();
        public List<Object> objectList = new List<Object>();
        //SignalTrack

        private void Start()
        {
            if (director != null && newObjectToAnimate != null)
            {
                AnimationTrack animationTrack = FindAnimationTrackInTimeline(director.playableAsset as TimelineAsset);

                //SignalTrack signalTrack = FindAnimationTrackInTimeline(director.playableAsset)

                if (animationTrack != null)
                {
                    director.SetGenericBinding(animationTrack, newObjectToAnimate);
                    director.RebuildGraph();
                }
                else
                {
                    Debug.LogWarning("No Animation Track found in the timeline.");
                }
            }

            //TestFunc(trackNameList, objectList, director.playableAsset as TimelineAsset);
        }

        private AnimationTrack FindAnimationTrackInTimeline(TimelineAsset timeline)
        {
            foreach (var track in timeline.GetOutputTracks())
            {
                if (track is AnimationTrack animationTrack)
                {
                    Debug.Log("track is " + track.name);
                    if (track.name == "CharacterA")
                    {
                    }
                }
                if (track is CustomAnimatorCallerTrack animationCallerTrack)
                {
                    Debug.Log((animationCallerTrack as CustomAnimatorCallerTrack).GetAnimatorCallerClip());
                }
            }
            return null;
        }
    }
}
