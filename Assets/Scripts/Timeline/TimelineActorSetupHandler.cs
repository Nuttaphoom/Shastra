using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;
using System;
using NaughtyAttributes;

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

        //[Header("Direct Vector Interpolation. The system will place these transform starting from CasterTransform (index 0) up to TargetTransform (last index)")]
        //[SerializeField]
        //Still Unavailable
        private List<Transform> _directVectorTransform = new List<Transform>() ;


        [SerializeField]
        private bool ChangeLookAt = false;

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("ChangeLookAt") ]
        [Header("Dynamically binding look at position with center betwen target transform")]
        private CinemachineVirtualCamera _virtualCameraToChangeLookAt ;

        private PlayableDirector _director;

        public void SetUpActor(PlayableDirector director, ActionTimelineSettingStruct actionTimelineSetting, SignalReceiver unitySignalReciver   )
        {
            _director = director;

            Transform objectWithTrackName;
            Animator animatorWithTrackName; 

            //For every tracks 
            foreach (var track in (director.playableAsset as TimelineAsset).GetOutputTracks())
            {
                //for (int i = 0; i < actionTimelineSetting.TrackNames.Count; i++)
                //{
                //    if (track.name == actionTimelineSetting.TrackNames[i])
                //    {
                //        director.SetGenericBinding(track, actionTimelineSetting.GetObjectWithTrackName(track.name));
                //    } else if ( track.name == "SignalTrack") {
                //        director.SetGenericBinding(track, unitySignalReciver) ; 
                //    } else if (track is CinemachineTrack)
                //    {

                //    } 
                //}
                if (objectWithTrackName = actionTimelineSetting.GetObjectWithTrackName(track.name))
                {
                    director.SetGenericBinding(track, objectWithTrackName);
                }
                else if (animatorWithTrackName = actionTimelineSetting.GetAnimatorWithTrackName(track.name))
                {
                    director.SetGenericBinding(track, animatorWithTrackName); 
                }
                else if (track.name == "SignalTrack")
                {
                    director.SetGenericBinding(track, unitySignalReciver);
                }
                else if (track is CinemachineTrack)
                {

                }
            
            }

            if (_casterTransform != null)
                SetUpCasterTargetTransform(_casterTransform, actionTimelineSetting.GetObjectWithIndex(0));

            int targetSelectedAmount = 0; 
            for (int i = 0; i < _targetTransform.Count; i++)
            {
                if (actionTimelineSetting.GetObjectWithIndex(i + 1) == null)
                {
                    _targetTransform[i].gameObject.SetActive(false);
                    continue;
                }
                targetSelectedAmount += 1; 

                SetUpCasterTargetTransform(_targetTransform[i].transform, actionTimelineSetting.GetObjectWithIndex(i + 1));
            }

            SetUpDirectVector();

            SetUpDynamicLookAtBinding(targetSelectedAmount);


        }

        private void SetUpDynamicLookAtBinding(int targetSelectedAmount)
        {
            if (_virtualCameraToChangeLookAt == null)
                return;

            if (targetSelectedAmount == 0)
                return; 

            Transform newLookAt = Instantiate(new GameObject().transform) ;

            Vector3 averagePos = _targetTransform[0].transform.position ;


            for (int i = 1; i < targetSelectedAmount ; i++)
            {
                averagePos += _targetTransform[i].transform.position;
            }

            averagePos.x /= targetSelectedAmount;
            averagePos.y /= targetSelectedAmount ;
            averagePos.z /= targetSelectedAmount ;

            newLookAt.position = averagePos; 

            _virtualCameraToChangeLookAt.LookAt = newLookAt; // [index];
            newLookAt.gameObject.SetActive(true);

            _destroyedWithTimeline.Add(newLookAt.gameObject) ;
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
            Destroy(_casterTransform.gameObject); 
            for (int i = _targetTransform.Count - 1; i >= 0 ; i--)
            {
                Destroy(_targetTransform[i].gameObject);
            }

            for (int i = _destroyedWithTimeline.Count - 1; i >= 0; i--)
            {
                if (_destroyedWithTimeline[i] != null) 
                    Destroy(_destroyedWithTimeline[i]);
            }
            
            _destroyedWithTimeline.Clear();

            Destroy(gameObject);
        }

    }



    


}
