using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;
using System;

namespace Vanaring
{
    public class TimelineActorSetupHandler : MonoBehaviour
    {
        [Header("Caster and Target transform should be assigned in Editor, but it should be later on assigned to be child of real caster/targets on runtime ")]
        [SerializeField]
        private Transform _casterTransform ;

        [SerializeField]
        private List<Transform> _targetTransform = new List<Transform>() ;

        [Header("Direct Vector Interpolation. The system will place these transform starting from CasterTransform (index 0) up to TargetTransform (last index)")]
        [SerializeField]
        private List<Transform> _directVectorTransform = new List<Transform>() ; 

        public void SetUpActor(PlayableDirector director, ActionTimelineSettingStruct actionTimelineSetting , SignalReceiver unitySignalReciver)
        { 
            foreach (var track in (director.playableAsset as TimelineAsset).GetOutputTracks())
            {
                for (int i = 0; i < actionTimelineSetting.TrackNames.Count; i++)
                {
                    if (track.name == actionTimelineSetting.TrackNames[i])
                    {
                        GameObject bindedObject = actionTimelineSetting.GetObjectWithTrackName(track.name);    
                        director.SetGenericBinding(track, bindedObject);
                    }else if (track.name == "SignalTrack")
                    {
                        director.SetGenericBinding(track, unitySignalReciver) ;
                    }else if (track is CinemachineTrack)
                    {
                        
                    }
                }
            }

            if (_casterTransform != null)
            { 
                Transform par = actionTimelineSetting.GetObjectWithIndex(0).transform;
                _casterTransform.transform.parent = par;
                _casterTransform.transform.position = par.position ;
                _casterTransform.transform.rotation = par.rotation;

            }

            for (int i = 0; i < _targetTransform.Count; i++)
            {
                Transform par = actionTimelineSetting.GetObjectWithIndex(i + 1).transform;
                _targetTransform[i].transform.parent = par; 
                _targetTransform[i].transform.position = par.position ;
                _targetTransform[i].transform.rotation = par.rotation ;
            }

            SetUpDirectVector(); 
            
        }
        private void SetUpDirectVector()
        {
            if (_directVectorTransform.Count > 0)
            {
                throw new NotImplementedException(); 
            }
        }
    }

    


}
