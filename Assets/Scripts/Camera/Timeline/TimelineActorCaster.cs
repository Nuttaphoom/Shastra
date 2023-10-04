using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Vanaring
{
    public class TimelineActorCaster : MonoBehaviour
    {
        public PlayableDirector director;
        public GameObject newObjectToAnimate;

        public Transform emptyTranform;

        public List<string> trackNameList = new List<string>();
        public List<Object> objectList = new List<Object>();
        //SignalTrack

        private void Start()
        {
            //if (director != null && newObjectToAnimate != null)
            //{
            //    AnimationTrack animationTrack = FindAnimationTrackInTimeline(director.playableAsset as TimelineAsset);
                
            //    //SignalTrack signalTrack = FindAnimationTrackInTimeline(director.playableAsset)

            //    if (animationTrack != null)
            //    {
            //        director.SetGenericBinding(animationTrack, newObjectToAnimate);
            //        director.RebuildGraph();
            //    }
            //    else
            //    {
            //        Debug.LogWarning("No Animation Track found in the timeline.");
            //    }
            //}

            TestFunc(trackNameList, objectList,director.playableAsset as TimelineAsset);
        }

        private void TestFunc(List<string> trackNameList, List<Object> objectList, TimelineAsset timeline)
        {
            //Debug.Log("Start");
            //int current = 0;
            //PlayableAsset playable;
            //foreach (string trackName in trackNameList)
            //{
            //    if(trackName == objectList[current].name)
            //    {
            //        if(objectList[current] is PlayableAsset)
            //        {
            //            Debug.Log("Founded:" + objectList[current] + " at index" + current);
            //            playable = objectList[current] as PlayableAsset;
            //        }
            //    }
            //    current++;
            //}

            foreach (var track in timeline.GetOutputTracks())
            {
                for (int i = 0; i < trackNameList.Count; i++)
                {
                    if (track.name == trackNameList[i])
                    {
                        Debug.Log("found");
                        director.SetGenericBinding(track, objectList[i]) ;
                        break; 
                    }
                }
            }
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
                if (track is LightControlTrack lightControlTrack)
                {
                    Debug.Log((lightControlTrack as LightControlTrack).GetLightClip()); 
                }
            }
            return null;
        }
    }
}
