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

        [SerializeField]
        private List<GameObject> _destroyedWithTimeline = new List<GameObject>() ;  

        [Header("Direct Vector Interpolation. The system will place these transform starting from CasterTransform (index 0) up to TargetTransform (last index)")]
        [SerializeField]
        private List<Transform> _directVectorTransform = new List<Transform>() ;

        private PlayableDirector _director;

        public void SetUpActor(PlayableDirector director, ActionTimelineSettingStruct actionTimelineSetting, SignalReceiver unitySignalReciver)
        {
            _director = director;

            foreach (var track in (director.playableAsset as TimelineAsset).GetOutputTracks())
            {
                for (int i = 0; i < actionTimelineSetting.TrackNames.Count; i++)
                {
                    if (track.name == actionTimelineSetting.TrackNames[i])
                    {
                        director.SetGenericBinding(track, actionTimelineSetting.GetObjectWithTrackName(track.name));
                    } else if (track.name == "SignalTrack")
                    {
                        director.SetGenericBinding(track, unitySignalReciver);
                    } else if (track is CinemachineTrack)
                    {

                    }
                }
            }

            if (_casterTransform != null)
                SetUpCasterTargetTransform(_casterTransform, actionTimelineSetting.GetObjectWithIndex(0)); 

            for (int i = 0; i < _targetTransform.Count; i++)
            {
                if (actionTimelineSetting.GetObjectWithIndex(i + 1) == null)
                {
                    _targetTransform[i].gameObject.SetActive(false);
                    continue;
                }

                SetUpCasterTargetTransform(_targetTransform[i].transform, actionTimelineSetting.GetObjectWithIndex(i + 1));
            }

            SetUpDirectVector();

        }

        private void SetUpCasterTargetTransform(Transform owner, GameObject objectWithIndex)
        {
            owner.transform.parent = objectWithIndex.transform;
            owner.transform.position = objectWithIndex.GetComponent<CombatEntityAnimationHandler>().GetEntityTimelineAnimationLocation(); ;
            owner.transform.rotation = objectWithIndex.transform.rotation;
        }

        private void SetUpDirectVector()
        {
            if (_directVectorTransform.Count > 0)
            {
                throw new NotImplementedException(); 
            }
        }
    
    
        public void DestroyTimelineElement()
        {
            for (int i = _destroyedWithTimeline.Count - 1 ;i >= 0; i--)
               Destroy( _destroyedWithTimeline[i] ) ;
            
            _destroyedWithTimeline.Clear();

            Destroy(gameObject);
        }

    }



    


}
