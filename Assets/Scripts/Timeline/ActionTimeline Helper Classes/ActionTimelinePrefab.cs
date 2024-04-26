using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;
using System;
using NaughtyAttributes;
using Vanaring.Assets.Scripts.Utilities;

namespace Vanaring
{
    public class ActionTimelinePrefab : MonoBehaviour
    {
        #region Const 
        
        private const string CasterTransformTag = "Combat/Animation/Action/CasterTransform";
        private const string TargetTransformTag = "Combat/Animation/Action/TargetTransform";
        
        #endregion

        #region Caster/Target Transform  
        private Transform _casterTransform ;

        private List<Transform> _targetTransform;

        #endregion

        /// Look At Variables //
        [Header("Dynamicall Chnage look at")]
        [SerializeField]
        private bool _changeLookAt = false;

        private CinemachineVirtualCamera _virtualCameraToChangeLookAt ;

   
        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("_changeLookAt") ]
        private ActionTimelineLookAtBinder _lookAtBinder; 

        ////////////////////////////


        //private List<GameObject> _destroyedWithTimeline = new List<GameObject>();

        public void SetUpActor(PlayableDirector director, ActionTimelineSettingStruct actionTimelineSetting, SignalReceiver unitySignalReciver   )
        {

            if (_lookAtBinder == null)
                _lookAtBinder = new ActionTimelineLookAtBinder(); 

            Transform objectWithTrackName;

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
                if (track is CustomAnimatorCallerTrack)
                {
                    string trackName = (track as CustomAnimatorCallerTrack).TrackName;

                    if (objectWithTrackName = actionTimelineSetting.GetObjectWithTrackName(track.name))
                    {
                        director.SetGenericBinding(track, objectWithTrackName);
                    }
                }
               
                else if (track.name == "SignalTrack")
                {
                    director.SetGenericBinding(track, unitySignalReciver);
                }
                else if (track is CinemachineTrack)
                {

                }
            
            }

            AssignCasterTargetTransform(actionTimelineSetting);

            //Set up Target and Caster transform, place them into correct location 
            var targetActors = actionTimelineSetting.GetAllTimelineActors() ;
            var casterActor = actionTimelineSetting.GetAllTimelineActors()[0];
            targetActors.RemoveAt(0);
            AssignCasterAndTargetsTransformToNewParent(casterActor, targetActors);

            //Set up look at of the camera 
            _lookAtBinder.BindLookAtTargetsToEnemies(_targetTransform);

            int targetSelectedAmount = _targetTransform.Count;


        }

       

        #region Caster Target Transform Set up
        private void AssignCasterTargetTransform(ActionTimelineSettingStruct actionTimelineSetting)
        {
            //Find Caster Transform 
            List<GameObject> casterTransforms = ObjectFindingTool.QueryObjectInChildren(transform, CasterTransformTag);
            List<GameObject> targetTransforms = ObjectFindingTool.QueryObjectInChildren(transform, TargetTransformTag);

            foreach (var c in casterTransforms)
                Debug.Log(c.gameObject.name);

            if (casterTransforms.Count != 1)
                throw new Exception("CasterTransform count is not right, foundObject.count is " + casterTransforms.Count);

            if (targetTransforms.Count == 0)
                throw new Exception("Target transform can not be found");

            //Assign Caster and Target Transforms
            _casterTransform = casterTransforms[0].transform ;

            _targetTransform = new List<Transform>();

            foreach (var obj in targetTransforms)
            {
                _targetTransform.Add(obj.transform);
            }

            //Remove unused target transform (for potentially multiple target selection)
            for (int i = 0; i < _targetTransform.Count; i++)
            {
                if (actionTimelineSetting.GetTimelineActorWithIndex(i + 1) == null)
                {
                    _targetTransform[i].gameObject.SetActive(false);
                    _targetTransform.RemoveAt(i);
                    i--;
                    continue;
                }

            }

        }
        private void AssignCasterAndTargetsTransformToNewParent(GameObject casterParent, List<GameObject> targetEntities)
        {
            if (targetEntities.Count > _targetTransform.Count)
                throw new Exception("Targeted Entities exceed Target Transforms ==> " + targetEntities.Count + " > " + _targetTransform.Count);

            //owner.transform.position = objectWithIndex.transform.position;
            
            _casterTransform.transform.parent = casterParent.transform;
            _casterTransform.transform.position = casterParent.GetComponent<CombatEntityAnimationHandler>().GetEntityTimelineAnimationLocation(); ;
            _casterTransform.transform.rotation = casterParent.transform.rotation;

            int i = 0;

            foreach (Transform targetTransfrom in _targetTransform)
            {
                GameObject targetObj = targetEntities[i]; 
                targetTransfrom.parent = targetObj.transform ;
                targetTransfrom.transform.position = targetObj.GetComponent<CombatEntityAnimationHandler>().GetEntityTimelineAnimationLocation(); ;
                targetTransfrom.transform.rotation = targetObj.transform.rotation;

                i++; 
            }
        }
        #endregion 


        public void DestroyTimelineElement()
        {
            Destroy(_casterTransform.gameObject); 
            for (int i = _targetTransform.Count - 1; i >= 0 ; i--)
            {
                Destroy(_targetTransform[i].gameObject);
            }

            _lookAtBinder.DestroySpawnedObj(); 
           

            Destroy(gameObject);
        }

    }



    


}
