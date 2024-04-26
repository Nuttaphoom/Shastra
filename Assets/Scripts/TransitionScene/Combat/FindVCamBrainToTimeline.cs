using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

namespace Vanaring
{
    public class FindVCamBrainToTimeline : MonoBehaviour
    {
        [SerializeField] private PlayableDirector director;
        private CinemachineVirtualCameraBase vcam;
        private CinemachineBrain cmBrain;


        void Start()
        {
            cmBrain = FindAnyObjectByType<CinemachineBrain>();
            //cmBrain = CinemachineCore.Instance.FindPotentialTargetBrain(vcam);
            var timelineAsset = director.playableAsset as TimelineAsset;
            var trackList = timelineAsset.GetOutputTracks();
            foreach (var track in trackList)
            {
                if (track.name == "Cinemachine Track")
                {
                    director.SetGenericBinding(track, cmBrain);
                }
            }
        }
    }
}
