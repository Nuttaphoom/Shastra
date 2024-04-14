using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using Cinemachine;
using DG.Tweening;
using System.Runtime.InteropServices;

namespace Vanaring 
{
    [RequireComponent(typeof(EntityCameraManager))]
    [Serializable]
    public class CombatEntityAnimationHandler : MonoBehaviour
    {
        private GameObject _visualMesh ;

        //Pivot Position for Visualization 

        #region Pivot Params
        
        private Transform _impactTransform;
        private Transform _groundTransform;
        private Transform _aboveHeadTransform;
        private Transform _hudTransform;
        private const string CharacterVisualMeshTag = "Character/VisualPivot/CharacterVisualMesh";
        private const string CharacterImpactPivotTag = "Character/VisualPivot/CharacterImpactPivot";
        private const string CharacterWorldHUDPivotTag = "Character/VisualPivot/CharacterWorldHUDPivot";
        private const string CharacterAboveHeadPivotTag = "Character/VisualPivot/CharacterAboveHeadPivot";
        private const string CharacterGroundPivotTag = "Character/VisualPivot/CharacterGroundPivot";

        private void RecursiveSetUpPivot(Transform child)
        {
            if (child.CompareTag(CharacterVisualMeshTag))
                _visualMesh = child.gameObject;

            if (child.CompareTag(CharacterImpactPivotTag))
                _impactTransform = child;

            if (child.CompareTag(CharacterWorldHUDPivotTag))
                _hudTransform = child;

            if (child.CompareTag(CharacterGroundPivotTag))
                _groundTransform = child;

            if (child.CompareTag(CharacterAboveHeadPivotTag))
                _aboveHeadTransform = child;

            for (int i = 0; i < child.transform.childCount; i++)
            {
                RecursiveSetUpPivot(child.GetChild(i));
            }
        }
        private void SetUpVisualPivotTransform()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                RecursiveSetUpPivot(child);

                if (child.CompareTag(CharacterVisualMeshTag))
                    _visualMesh = child.gameObject;

                if (child.CompareTag(CharacterImpactPivotTag))
                    _impactTransform = child;

                if (child.CompareTag(CharacterWorldHUDPivotTag))
                    _hudTransform = child;

                if (child.CompareTag(CharacterGroundPivotTag))
                    _groundTransform = child;

                if (child.CompareTag(CharacterAboveHeadPivotTag))
                    _aboveHeadTransform = child;
            }

            if (_visualMesh == null)
                throw new Exception("Object with tag " + CharacterVisualMeshTag + " can't be FOUND in " + gameObject.name);


            if (_impactTransform == null)
                throw new Exception("Object with tag " + CharacterImpactPivotTag + " can't be FOUND in " + gameObject.name);

            if (_hudTransform == null)
                throw new Exception("Object with tag " + CharacterWorldHUDPivotTag + " can't be FOUND in " + gameObject.name);

            if (_groundTransform == null)
                throw new Exception("Object with tag " + CharacterGroundPivotTag + " can't be FOUND in " + gameObject.name);

            if (_aboveHeadTransform == null)
                throw new Exception("Object with tag " + CharacterAboveHeadPivotTag + " can't be FOUND in " + gameObject.name);
        }
    
        private Transform ImpactTransform
        {
            get
            {
                if (_impactTransform == null)
                    SetUpVisualPivotTransform();

                if (_impactTransform == null)
                    throw new Exception("Impact transform of " + gameObject.name + " can't be found"); 

                return _impactTransform; 
            }
        }
        private Transform HudTransform
        {
            get
            {
                if (_hudTransform == null)
                    SetUpVisualPivotTransform();

                if (_hudTransform == null)
                    throw new Exception("HudTransform of " + gameObject.name + " can't be found");

                return _hudTransform;
            }
        }

        private Transform AboveHeadTransform
        {
            get
            {
                if (_aboveHeadTransform == null)
                    SetUpVisualPivotTransform();

                if (_aboveHeadTransform == null)
                    throw new Exception("AboveHeadTransform of " + gameObject.name + " can't be found");

                return _aboveHeadTransform;
            }
        }
        private Transform GroundTransform
        {
            get
            {
                if (_groundTransform == null)
                    SetUpVisualPivotTransform();

                if (_groundTransform == null)
                    throw new Exception("GroundTransform of " + gameObject.name + " can't be found");

                return _groundTransform;
            }
        }

        #endregion
        //////

        [SerializeField]
        private ParticleSystem _spawnVisualEffect;

        [SerializeField]
        public VisualEffect _deadVisualEffect;

        [SerializeField]
        private string _deadAnimationTrigger = "NONE";

        //[Header("Use for specially set where (CastTransform, TarTransform) position will be set to #Can leave blank")]
        //[SerializeField]
        //public Transform _timelineAnimationRootLocation ; 

        private Animator _animator;


        #region GETTER
        
        public GameObject GetVisualMesh()
        {
            if (_visualMesh == null)
            {
                throw new Exception("VisualMesh is null"); 
            }
            return _visualMesh;
        }
     

        //Get Pivot 
        public Transform GetGUISpawnTransform()
        {
            //if (_guiPos == null || _guiPos.position == null)
            //    throw new Exception("GUI Spawn Position of " + gameObject.name + "hasn't never been assigned");
            
            return HudTransform ;
        }

        public Transform GetTargetIconSpawnPos
        { 
            get {
                return ImpactTransform ; 
            } 
        }

        /// <summary>
        /// return a transform in which Caster or Target position will be set to 
        /// </summary>
        /// <returns></returns>
        public Vector3 GetEntityTimelineAnimationLocation()
        {
            //if (_timelineAnimationRootLocation == null)
            //{
            //    return GetGUISpawnTransform().position;
            //}
            //return _timelineAnimationRootLocation.position;

            return ImpactTransform.position;
        }

        //////////////




        #endregion

       
        private void Awake()
        {
            SetUpVisualPivotTransform(); 

            _animator = GetVisualMesh().GetComponent<Animator>();
        }

        public IEnumerator PlayTriggerAnimation(string triggerName)
        {
            _animator.SetTrigger(triggerName);

            // Get the hash of the animation state
            int animationHash = Animator.StringToHash(triggerName);
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;

            //yield return new WaitForEndOfFrame();

            //yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length );

            yield return new WaitForSeconds(3.0f);
        }

        public IEnumerator DestroyVisualMesh()
        {
            if (_deadVisualEffect)
            {
                _deadVisualEffect.gameObject.SetActive(true);
                _deadVisualEffect.Play();
            }

            yield return new WaitForSeconds(0.6f);

            if (_deadAnimationTrigger == "NONE")
            {
                _visualMesh.transform.Translate(new Vector2(10000000, 1000000));
                yield return new WaitForSeconds(2.5f);
                if (_deadVisualEffect)
                {
                    Destroy(_deadVisualEffect.gameObject);
                }

            }
            else
            {
                yield return PlayTriggerAnimation(_deadAnimationTrigger);
            }
        }

        public void InstantlyHideVisualMesh()
        {
            _visualMesh.SetActive(false);
        }

        public IEnumerator PlaySpawnVisualEffectCoroutine()
        {
            float overallTime = 0.0f ;
            if (_spawnVisualEffect != null)
            {
                _spawnVisualEffect.gameObject.SetActive(true);
                _spawnVisualEffect.Play();
                overallTime = _spawnVisualEffect.main.duration; 
                yield return new WaitForSeconds(overallTime / 2);
            }

            _visualMesh.SetActive(true);

            yield return new WaitForSeconds(overallTime / 2);

            if (_spawnVisualEffect != null)
            {
                _spawnVisualEffect.gameObject.SetActive(false); 
            }
        }


        /// <summary>
        /// Attahment position include 
        /// "HEAD" , "CENTERMESH" 
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="whereToAttach"></param>
        public void AttachVFXToMeshComponent(GameObject vfxPrefab, string whereToAttach, string vfxName)
        {

            Transform parent = GetAttachmentFromName(whereToAttach) ;
     
            var newVFX = Instantiate(vfxPrefab, parent);

            newVFX.name = vfxName;
            newVFX.transform.position = parent.position;
            newVFX.transform.rotation = parent.rotation; 
        }

        public void DeAttachVFXFromMeshComponent(string vfxName, string whereToAttach)
        {
            Transform parent = GetAttachmentFromName(whereToAttach);

            Destroy(parent.Find(vfxName).gameObject) ;
        }

        private Transform GetAttachmentFromName(string whereToAttach)
        {
            if (whereToAttach == "HEAD")
            {
                return AboveHeadTransform ;
            }

            else if (whereToAttach == "CENTERMESH")
            {
                return _visualMesh.transform;
            }
            else if (whereToAttach == "VFXPOS")
            {
                return ImpactTransform ; 
            }
            else
            {
                throw new Exception("whereToAttach is not match");
            }
        }




    }
}