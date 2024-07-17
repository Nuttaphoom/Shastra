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
using PixelCrushers.DialogueSystem.UnityGUI;
//using UnityEditor.SceneManagement;

namespace Vanaring 
{
    [RequireComponent(typeof(EntityCameraManager))]
    [Serializable]
    public class CombatEntityAnimationHandler : MonoBehaviour
    {
        private GameObject _visualMesh ;

        [Header("Use for Enemy location, entity wtih dedicated location won't be move from that location as long as the location is available")]
        private int _dedicateLocation = -1; 
        //Pivot Position for Visualization 

        #region Pivot Params
        
        private Transform _impactTransform;
        private Transform _groundTransform;
        private Transform _aboveHeadTransform;
        private Transform _hudTransform;
        private Transform _targetIconTransform;
        private Transform _characterCenterTransform; 
        private const string CharacterVisualMeshTag = "Character/VisualPivot/CharacterVisualMesh";
        private const string CharacterImpactPivotTag = "Character/VisualPivot/CharacterImpactPivot";
        private const string CharacterCenterPivotTag = "Character/VisualPivot/CharacterCenterPivot";

        private const string CharacterWorldHUDPivotTag = "Character/VisualPivot/CharacterWorldHUDPivot";
        private const string CharacterAboveHeadPivotTag = "Character/VisualPivot/CharacterAboveHeadPivot";
        private const string CharacterGroundPivotTag = "Character/VisualPivot/CharacterGroundPivot";
        private const string CharacterTargetIconPivotTag = "Character/VisualPivot/CharacterTargetIconPivot";

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

            if (child.CompareTag(CharacterTargetIconPivotTag))
                _targetIconTransform = child;
            
            if (child.CompareTag(CharacterCenterPivotTag))
            {
                _characterCenterTransform = child; 
            }
             
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

                if (child.CompareTag(CharacterTargetIconPivotTag))
                    _targetIconTransform = child;

                if (child.CompareTag(CharacterCenterPivotTag))
                    _characterCenterTransform = child;
                
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

            if (_targetIconTransform == null)
                throw new Exception("Object with tag " + CharacterTargetIconPivotTag + " can't be FOUND in " + gameObject.name);
        
            if (_characterCenterTransform == null)
                throw new Exception("Object with tag " + CharacterCenterPivotTag + " can't be FOUND in " + gameObject.name);

        }
        private Transform TargetIconTransform
        {
            get
            {
                if (_targetIconTransform == null)
                    SetUpVisualPivotTransform();

                if (_targetIconTransform == null)
                    throw new Exception("_targetIconTransform transform of " + gameObject.name + " can't be found");

                return _targetIconTransform;
            }
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

        private List<GameObject> _attachedVFXs = new List<GameObject>() ;

        private const string _deadAnimationTrigger = "Dead";

        private CombatEntity _combatEntity;


        //[Header("Use for specially set where (CastTransform, TarTransform) position will be set to #Can leave blank")]
        //[SerializeField]
        //public Transform _timelineAnimationRootLocation ; 

        private Animator _animator;

        private void Awake()
        {
            SetUpVisualPivotTransform();

            _animator = GetVisualMesh().GetComponent<Animator>();
            _combatEntity = GetComponent<CombatEntity>();
        }

        #region Mesh Methods 

        private float captured_normalizedTimeAnimation = -1;

        private int capturedAnimatorHash = -1 ;
        private void CaptureAnimatorState()
        {
            if (gameObject.GetComponent<AIEntity>() != null)
            {
                //ColorfulLogger.LogWithColor("Capture " + gameObject.name + " animation state", Color.yellow); 
            }

            
            var animator = GetVisualMesh().GetComponent<Animator>();
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (! stateInfo.IsName("Idle") && ! stateInfo.IsName("Stun Stay"))
                return;
            
            captured_normalizedTimeAnimation = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            capturedAnimatorHash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash ;
            
        }


        private void RestoreAnimatorState()
        {
            if (gameObject.GetComponent<AIEntity>() != null)
            {
                //ColorfulLogger.LogWithColor("Restore " + gameObject.name + " animation state", Color.green);
            }
            if (capturedAnimatorHash == -1 || captured_normalizedTimeAnimation == -1)
                return; 

            var animator = GetVisualMesh().GetComponent<Animator>();

            //ColorfulLogger.LogWithColor("restore captured_normalizedTimeAnimation in " + gameObject.name +" : " + captured_normalizedTimeAnimation, Color.red);
            animator.Play(capturedAnimatorHash, 0, captured_normalizedTimeAnimation);

            ResetCapturedAnimationData(); 


        }

        public void ResetCapturedAnimationData()
        {
            capturedAnimatorHash = -1;
            captured_normalizedTimeAnimation = -1;
        } 

        public void HideVisualMesh()
        {
            if (! GetVisualMesh().gameObject.activeSelf)
                return;

            foreach (var attachedvfx in _attachedVFXs)
            {
                if (attachedvfx != null)
                {
                    attachedvfx.gameObject.SetActive(false);
                }
            }
            CaptureAnimatorState();

            GetVisualMesh().gameObject.SetActive(false);

        }

        public void ShowVisualMesh()
        {
            if (GetVisualMesh().gameObject.activeSelf)
                return;

            if (_combatEntity.IsDead)
                return;

            foreach (var attachedvfx in _attachedVFXs)
            {
                if (attachedvfx != null)
                {
                    attachedvfx.gameObject.SetActive(true);
                }
            }

            GetVisualMesh().gameObject.SetActive(true);

            RestoreAnimatorState(); 
        }
        public IEnumerator DeadVisualPresentation()
        {
            for (int i =  _attachedVFXs.Count - 1; i >= 0; i--) 
                Destroy(_attachedVFXs[i].gameObject) ; 

            if (_deadVisualEffect)
            {
                StartCoroutine(PlayTriggerAnimation("Hurt") ) ;

                _deadVisualEffect.gameObject.SetActive(true);
                _deadVisualEffect.Play();

                yield return new WaitForSeconds(0.6f);

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
        #endregion 
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
        public Transform GetHUDSpawnTransform()
        {
            //if (_guiPos == null || _guiPos.position == null)
            //    throw new Exception("GUI Spawn Position of " + gameObject.name + "hasn't never been assigned");
            
            return HudTransform ;
        }
        public Transform GetTargetIconTransform()
        {
            return TargetIconTransform ; 
        }

        public Transform GetCenterMesh()
        {
            return _characterCenterTransform ; 
        }

        public Transform GetImpactTransform()
        {
            return _impactTransform.transform ;
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

        #region Animation Methods 
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
        public IEnumerator PlaySpawnVisualEffectCoroutine()
        {
            float overallTime = 0.0f;
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
        #endregion

        #region VFX Methods 
        /// <summary>
        /// Attahment position include 
        /// "HEAD" , "CENTERMESH" 
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="whereToAttach"></param>
        public void AttachVFXToMeshComponent(GameObject vfxPrefab, string whereToAttach, string vfxName)
        {
            if (_attachedVFXs.Contains(vfxPrefab))
            {
                throw new Exception("vfxPrefab of same object is trying to attached multiple time"); 
            }


            Transform parent = GetAttachmentFromName(whereToAttach);

            var newVFX = Instantiate(vfxPrefab, parent);

            _attachedVFXs.Add(newVFX);


            newVFX.name = vfxName;
            newVFX.transform.position = parent.position;
            newVFX.transform.rotation = parent.rotation;
        }

        public void DeAttachVFXFromMeshComponent(string vfxName, string whereToAttach)
        {
            Transform parent = GetAttachmentFromName(whereToAttach);

            GameObject removedObj = parent.Find(vfxName).gameObject;

            if (removedObj == null)
                return; 

            _attachedVFXs.Remove(removedObj);
 
            Destroy(removedObj) ; 

        }

        private Transform GetAttachmentFromName(string whereToAttach)
        {
            if (whereToAttach == "HEAD")
            {
                return AboveHeadTransform;
            }

            else if (whereToAttach == "CENTERMESH")
            {
                return _visualMesh.transform;
            }
            else if (whereToAttach == "VFXPOS")
            {
                return ImpactTransform;
            }
            else
            {
                throw new Exception("whereToAttach is not match");
            }
        }
        #endregion
        public void RotateMeshLookAtToThisPosition(Vector3 worldPosition)
        {
            Vector3 lookAtVector = worldPosition - GetVisualMesh().transform.position; 

            // Create a rotation that looks at the specified position
            Quaternion rotation = Quaternion.LookRotation(lookAtVector);

            // Apply the rotation to the visual mesh
            GetVisualMesh().transform.rotation = rotation;


        }

        public void RestoreLookAt()
        {
            GetVisualMesh().transform.rotation = transform.rotation; 
        }












    }
}