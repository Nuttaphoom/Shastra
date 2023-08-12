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

namespace Vanaring_DepaDemo
{
    [Serializable]
    public class CombatEntityAnimationHandler : MonoBehaviour
    {

        [SerializeField]
        private GameObject _mesh ;

        [SerializeField]
        public Transform _vfxPos ;

        [SerializeField]
        public VisualEffect _deadVisualEffect;

        [SerializeField]
        private string _deadAnimationTrigger = "NONE"; 

        public Vector3 GetVFXSpawnPos()
        {
            if (_vfxPos == null || _vfxPos.position == null )
            {
                throw new Exception("VFX Spawn Position of " + gameObject.name + "hasn't never been assigned");
            }
            
            return _vfxPos.position ;
        }

        private Animator _animator;
        private void Awake()
        {
            _animator = _mesh.GetComponent<Animator>();
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
        public IEnumerator PlayActionAnimation(ActionAnimationInfo actionAnimation )
        {
            Debug.Log("play action animation");

            List<IEnumerator> coroutines = new List<IEnumerator>();

            //Self VFX
            if (actionAnimation.CasterVfxEntity.IsValid() )
            {
                VFXCallbackHandler<string> callbackHandler = new VFXCallbackHandler<string>(GetComponent<CombatEntity>(), actionAnimation.CasterVfxEntity, GetVFXSpawnPos(), null);
                coroutines.Add(callbackHandler.PlayVFX(actionAnimation.TargetTrigerID)) ;
            }

            //Play Animation 
            coroutines.Add (PlayTriggerAnimation(actionAnimation.SelfTrigerID) ) ;

            yield return new WaitAll(this, coroutines.ToArray() );
            Debug.Log("finish play action animation");

        }

        public IEnumerator PlayVFXActionAnimation<T>(VFXEntity vfxEntity,  VFXCallbackHandler<T>.VFXCallback  argc  , T pam)
        {
            Debug.Log("play vfx action animation");
            VFXCallbackHandler<T> callbackHandler = new VFXCallbackHandler<T>(GetComponent<CombatEntity>(), vfxEntity , GetVFXSpawnPos(),  argc  );
            yield return (callbackHandler.PlayVFX(pam));

            Debug.Log("finish play vfx action animation");

        }

        public IEnumerator DestroyVisualMesh()
        {
            if (_deadVisualEffect)
            {
                _deadVisualEffect.gameObject.SetActive(true);
                _deadVisualEffect.Play();
            }

            yield return new WaitForSeconds(0.7f);

            if (_deadAnimationTrigger == "NONE")
                transform.Translate(new Vector2(10000000, 1000000));
            else
                PlayTriggerAnimation(_deadAnimationTrigger);
        }


    }
}