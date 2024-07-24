using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;
using System;
using NaughtyAttributes;
using Vanaring.Assets.Scripts.Utilities;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace Vanaring
{
    public class ActionTimelinePrefab : MonoBehaviour
    {
        #region Const 

        private const string CasterTransformTag = "Combat/Animation/Action/CasterTransform";
        private const string TargetTransformTag = "Combat/Animation/Action/TargetTransform";

        private const string AttachToImpactTransformTag = "Combat/Animation/Action/AttachToImpactTransform";

        #endregion

        #region Caster/Target Transform  
        private Transform _casterTransform;

        private List<Transform> _targetTransform;

        #endregion

        /// Look At Variables //
        [Header("Dynamicall Chnage look at")]
        [SerializeField]
        private bool _changeLookAt = false;

        private CinemachineVirtualCamera _virtualCameraToChangeLookAt;

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("_changeLookAt")]
        private ActionTimelineLookAtBinder _lookAtBinder;

        ////////////////////////////
        [Header("Use ActionTimelinePrefab location")]
        [SerializeField]
        private bool _useActionTimelinePrefabLocation = false;

        [Header("Use  automatically rotate targets forward direction to caster")]
        [SerializeField]
        private bool _rotateTargetsToCaster;

        public bool IsThisTimelineUseInitialCamera => _rotateTargetsToCaster; 

        [SerializeField, AllowNesting, NaughtyAttributes.ShowIf("_useActionTimelinePrefabLocation")]
        private ActionAnimationLocationBinder _actionAnimationLocationBinder;

        private ActionTimelineSettingStruct _actionTimelineSetting;

        private List<GameObject> _relocatedVFX;
        //private List<GameObject> _destroyedWithTimeline = new List<GameObject>();

        public void SetUpActor(PlayableDirector director, ActionTimelineSettingStruct actionTimelineSetting, SignalReceiver unitySignalReciver   )
        {
            _actionTimelineSetting = actionTimelineSetting;

            _actionTimelineSetting.PrepareEntityAnimationForAction(); 

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

                    if (objectWithTrackName = _actionTimelineSetting.GetObjectWithTrackName(track.name))
                    {
                        //Debug.Log( track.name + " bind with " + objectWithTrackName.name);

                        director.SetGenericBinding(track, objectWithTrackName);
                    }
                }else if (track is CustomEntityMeshHiddenTrack)
                {
                    string trackName = (track as CustomEntityMeshHiddenTrack).TrackName;

                    if (objectWithTrackName = _actionTimelineSetting.GetObjectWithTrackName(track.name))
                    {
                        //Debug.Log( track.name + " bind with " + objectWithTrackName.name);

                        director.SetGenericBinding(track, objectWithTrackName);
                    }
                }
               
                else if (track.name == "SignalTrack")
                {
                    //Debug.Log("Signal Track bind with " + unitySignalReciver.gameObject.name);
                    director.SetGenericBinding(track, unitySignalReciver.gameObject);
                }
                else if (track is CinemachineTrack)
                {

                }
            
            }

            HandleTargetsLocation();

        }

       

        private void HandleTargetsLocation()
        {
            AssignCasterTargetTransform(_actionTimelineSetting);

            //Set up Target and Caster transform, place them into correct location 
            var targetActors = _actionTimelineSetting.GetAllTimelineActors();
            var casterActor = _actionTimelineSetting.GetAllTimelineActors()[0];
            targetActors.RemoveAt(0);

            //Uise ActionTimelinePrefab 
            if (_useActionTimelinePrefabLocation)
            {
                List<Transform> casterTransforms = new List<Transform>() { _casterTransform.transform };
                if (!_actionAnimationLocationBinder.MoveTargets)
                {
                    AssignTargetTransformsToNewParent(targetActors);
                }

                if (!_actionAnimationLocationBinder.MoveCaster)
                {
                    AssignCasterTransformToNewParent(casterActor);
                }

                _actionAnimationLocationBinder.SetUpBinder(casterTransforms, _targetTransform, casterActor, targetActors);



            }
            else
            {
                AssignCasterTransformToNewParent(casterActor);
                AssignTargetTransformsToNewParent(targetActors);

            }

            if (_changeLookAt)
                //Set up look at of the camera 
                _lookAtBinder.BindLookAtTargetsToEnemies(_targetTransform);


            RelocateImpactVFXtoImpactTransform(casterActor, targetActors);
        }
         
        private void RelocateImpactVFXtoImpactTransform(GameObject casters, List<GameObject> targets)
        {
            _relocatedVFX = new List<GameObject>(); 

            
            //Assign Target's first 
            //Target now is inside the TargetTransform

            if (_actionAnimationLocationBinder.MoveTargets)
            {
                var allTargetTransform = ObjectFindingTool.QueryObjectInChildren(transform, TargetTransformTag);
 

                for (int i = 0; i < targets.Count; i++)
                {
                    //Debug.Log("targets for relocation :  " + targets[i].gameObject.name);

                    var target = targets[i];

                    List<GameObject> allVFXs = ObjectFindingTool.QueryObjectInChildren(allTargetTransform[i].transform, AttachToImpactTransformTag);

                    foreach (var allVFX in allVFXs)
                    {
                        _relocatedVFX.Add(allVFX);
                    }

                    Transform impactTransform = target.GetComponent<CombatEntityAnimationHandler>().GetImpactTransform();

                    for (int j = 0; j < allVFXs.Count; j++)
                    {
                        //Debug.Log("vfx for relocation :  " + allVFXs[j].gameObject.name);

                        allVFXs[j].transform.parent = impactTransform;
                        allVFXs[j].transform.position = impactTransform.position;
                        allVFXs[j].transform.rotation = impactTransform.rotation;
                    }
                }
            }
            //TargetTransform is now inside Targets 
            else
            {

                //Debug.Log("try to relocate vfxs is ");

                for(int i = 0  ; i < targets.Count;i++) {
                    //Debug.Log("targets for relocation :  " + targets[i].gameObject.name) ;
                    var allTargetTransform = ObjectFindingTool.QueryObjectInChildren(targets[i].transform, TargetTransformTag);


                    var target = targets[i];  

                    List<GameObject> allVFXs = ObjectFindingTool.QueryObjectInChildren(allTargetTransform[i].transform, AttachToImpactTransformTag);

                    foreach (var allVFX in allVFXs)
                    {
                        _relocatedVFX.Add(allVFX);
                    }

                    Transform  impactTransform = target.GetComponent<CombatEntityAnimationHandler>().GetImpactTransform(); 

                    for (int j =0; j <allVFXs.Count; j++)
                    {
                        //Debug.Log("vfx for relocation :  " + allVFXs[i].gameObject.name);

                        allVFXs[j].transform.parent = impactTransform               ;
                        allVFXs[j].transform.position = impactTransform.position    ;
                        allVFXs[j].transform.rotation = impactTransform.rotation    ;
                    }
                }
            }

            //Assign Caster's
        }

        #region Caster Target Transform Set up
        private void AssignCasterTargetTransform(ActionTimelineSettingStruct actionTimelineSetting)
        {
            //Find Caster Transform 
            List<GameObject> casterTransforms = ObjectFindingTool.QueryObjectInChildren(transform, CasterTransformTag);
            List<GameObject> targetTransforms = ObjectFindingTool.QueryObjectInChildren(transform, TargetTransformTag);

            
            if (casterTransforms.Count != 1)
                throw new Exception("CasterTransform count is not right, foundObject.count is " + casterTransforms.Count);

            if (targetTransforms.Count == 0)
                throw new Exception("Target transform can not be found on ");


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
        private void AssignCasterTransformToNewParent(GameObject casterParent)
        {
            
            _casterTransform.transform.parent = casterParent.transform;
            _casterTransform.transform.position = casterParent.GetComponent<CombatEntityAnimationHandler>().GetEntityTimelineAnimationLocation(); ;
            _casterTransform.transform.rotation = casterParent.transform.rotation;

            
        }

        private void AssignTargetTransformsToNewParent(List<GameObject> targetEntities)
        {

            if (targetEntities.Count > _targetTransform.Count)
                throw new Exception("Targeted Entities exceed Target Transforms ==> " + targetEntities.Count + " > " + _targetTransform.Count);

            int i = 0;

            foreach (Transform targetTransfrom in _targetTransform)
            {
                GameObject targetObj = targetEntities[i];
                targetTransfrom.parent = targetObj.transform;
                targetTransfrom.transform.position = targetObj.GetComponent<CombatEntityAnimationHandler>().GetEntityTimelineAnimationLocation(); ;
                targetTransfrom.transform.rotation = targetObj.transform.rotation;

                i++;
            }
        }
        #endregion 


        public void DestroyTimelineElement()
        {
            if (_useActionTimelinePrefabLocation)
                _actionAnimationLocationBinder.ResetPositionBack();

            for (int i = 0; i < _relocatedVFX.Count; i++)
            {
                if (_relocatedVFX[i] != null)
                {
                    Destroy(_relocatedVFX[i]);
                }

                _relocatedVFX.RemoveAt(i);
                i--; 
            }



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
