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

        public void SetUpActor(PlayableDirector director, ActionTimelineSettingStruct actionTimelineSetting , SignalReceiver unitySignalReciver)
        {
            foreach (var track in (director.playableAsset as TimelineAsset).GetOutputTracks())
            {
                for (int i = 0; i < actionTimelineSetting.TrackNames.Count; i++)
                {
                    if (track.name == actionTimelineSetting.TrackNames[i])
                    {
                        GameObject bindedObject = actionTimelineSetting.GetObjectWithTrackName(track.name).GetComponent<CombatEntityAnimationHandler>().GetVisualMesh() ;
                        director.SetGenericBinding(track, bindedObject );
                    }else if (track.name == "SignalTrack")
                    {
                        director.SetGenericBinding(track, unitySignalReciver) ;
                    }
                }
            }
        }
    }
}
