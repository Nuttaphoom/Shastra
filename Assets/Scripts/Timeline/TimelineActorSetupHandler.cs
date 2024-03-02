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


        [SerializeField]
        private bool ModifyMainCameraPosition = false;
        
        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("ModifyMainCameraPosition")]
        [Header("Transform to put main VM in and apply translation")]
        private Transform _VMTranslationTransform;

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("ModifyMainCameraPosition")]
        [Header("(Optional) Rotation of the parent (bind dynamically ) will be assigned to this object, result in rotating _VMTranslationTransform translation direction")]
        private Transform _VMFaceDirectionParentTransform ;


        private Transform _mainVMTransform;
        private Transform _formerParent; 

        public void SetUpActor(PlayableDirector director, ActionTimelineSettingStruct actionTimelineSetting, SignalReceiver unitySignalReciver   )
        {
            _director = director;

            Transform objectWithTrackName;

            //Tranform main camera to translation object
            if (ModifyMainCameraPosition)
            {
                var animatorWithTrackName = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject ;
                if (_VMFaceDirectionParentTransform != null)
                {
                    _VMFaceDirectionParentTransform.transform.rotation = animatorWithTrackName.transform.parent.transform.rotation;
                    _VMFaceDirectionParentTransform.transform.position = animatorWithTrackName.transform.position;
                }
                _mainVMTransform = animatorWithTrackName.transform;
                _formerParent = _mainVMTransform.parent;
                _mainVMTransform.parent = _VMTranslationTransform;
            }

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
            //owner.transform.position = objectWithIndex.transform.position;
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
            if (ModifyMainCameraPosition)
            {
                _mainVMTransform.parent = _formerParent;
                Destroy(_VMTranslationTransform.gameObject);
                if (_VMFaceDirectionParentTransform)
                    Destroy(_VMFaceDirectionParentTransform.gameObject);
            }

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
