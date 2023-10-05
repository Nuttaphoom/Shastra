using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Vanaring
{
    public class TimelineActorSetupHandler : MonoBehaviour
    {
        List<ActionSignal> _currentSignal = new List<ActionSignal>();

        public void SetUpActor(PlayableDirector director, ActionTimelineSettingStruct actionTimelineSetting)
        {
            foreach (var track in (director.playableAsset as TimelineAsset).GetOutputTracks())
            {
                for (int i = 0; i < actionTimelineSetting.TrackNames.Count; i++)
                {
                    if (track.name == actionTimelineSetting.TrackNames[i])
                    {
                        Debug.Log("Found");
                        director.SetGenericBinding(track, actionTimelineSetting.GetObjectWithTrackName(track.name) as GameObject);
                        break;
                    }
                }
            }
        }
    }
}
