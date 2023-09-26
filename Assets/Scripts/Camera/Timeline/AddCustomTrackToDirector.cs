using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;

namespace Vanaring
{
    public class AddCustomTrackToDirector : MonoBehaviour
    {
        public PlayableDirector director;
        public TrackAsset customTrackPrefab; // The custom track you want to add

        private void Start()
        {
            // Make sure director and customTrackPrefab are assigned in the Inspector
            if (director == null || customTrackPrefab == null)
            {
                Debug.LogError("Director or custom track is not assigned.");
                return;
            }

            // Create a new Timeline asset
            TimelineAsset timelineAsset = ScriptableObject.CreateInstance<TimelineAsset>();

            // Add the custom track to the timeline
            TrackAsset newTrack = timelineAsset.CreateTrack(customTrackPrefab.GetType(), null, "CustomTrack");

            // You can customize the track or add clips to it here if needed

            // Assign the timeline asset to the director
            director.playableAsset = timelineAsset;
        }
        private void Update()
        {
            if (director != null)
            {
                double currentTime = director.time;
                
                int roundedNumber = (int)Math.Round(currentTime);

                //TestA();
                //Debug.Log("Director Time: " + roundedNumber);
            }
        }

        public void TestA()
        {
            Debug.Log("TestA");
        }
    }
}
