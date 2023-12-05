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

namespace Vanaring 
{
    [RequireComponent(typeof(EntityCameraManager))]
    [Serializable]
    public class CombatEntityAnimationHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _visualMesh ;



        [SerializeField]
        public Transform _guiPos;

        [SerializeField]
        public VisualEffect _deadVisualEffect;

        [SerializeField]
        private string _deadAnimationTrigger = "NONE";

        [SerializeField]
        private ParticleSystem _spawnVisualEffect;

        [SerializeField]
        private Transform _head_position ;


        [Header("Use for specially set where (CastTransform, TarTransform) position will be set to #Can leave blank")]
        [SerializeField]
        public Transform _timelineAnimationRootLocation ;

        #region GETTER
        public Vector3 GetEntityTimelineAnimationLocation()
        {
            if (_timelineAnimationRootLocation == null)
            {
                return GetGUISpawnTransform().position;
            }
            return _timelineAnimationRootLocation.position  ;
        }
        public GameObject GetVisualMesh()
        {
            if (_visualMesh == null)
            {
                throw new Exception("VisualMesh is null"); 
            }
            return _visualMesh;
        }
     

        public Transform GetGUISpawnTransform()
        {
            if (_guiPos == null || _guiPos.position == null)
                throw new Exception("GUI Spawn Position of " + gameObject.name + "hasn't never been assigned");
            
            return _guiPos ;
        }

        

      
        #endregion
        private Animator _animator;
        private void Awake()
        {
            if (_visualMesh == null)
                throw new Exception("Visual Mesh  of " + gameObject + " need to be assigned");
 
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
                return _head_position;
            }

            else if (whereToAttach == "CENTERMESH")
            {
                return _visualMesh.transform;
            }
            else
            {
                throw new Exception("whereToAttach is not match");
            }
        }




    }
}