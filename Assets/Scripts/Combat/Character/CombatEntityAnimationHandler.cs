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
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring_DepaDemo
{
    [Serializable]
    public class CombatEntityAnimationHandler : MonoBehaviour
    {

        [SerializeField]
        private GameObject _mesh ;

        [SerializeField]
        public Transform _vfxPos ; 

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
            _animator.SetTrigger(triggerName) ;

            yield return new WaitForSeconds(3.0f);
        }
        public IEnumerator PlayActionAnimation(ActionAnimationInfo actionAnimation )
        {
            //Self VFX
            List<IEnumerator> coroutines = new List<IEnumerator>(); 
            VFXCallbackHandler<string> callbackHandler = new VFXCallbackHandler<string>(gameObject, actionAnimation.CasterVfxEntity, GetVFXSpawnPos(), null );
            coroutines.Add (callbackHandler.PlayVFX(null));

            //Play Animation 
            coroutines.Add (PlayTriggerAnimation(actionAnimation.SelfTrigerID) ) ;

            yield return new WaitAll(this, coroutines.ToArray() );
 
        }

        public IEnumerator PlayVFXActionAnimation<T>(VFXEntity vfxEntity, CombatEntity target, VFXCallbackHandler<T>.VFXCallback argc, T param  )
        {
            CombatEntity entity = target;
            VFXCallbackHandler<T> callbackHandler = new VFXCallbackHandler<T>(target.gameObject, vfxEntity , target.CombatEntityAnimationHandler.GetVFXSpawnPos(),  argc  );
            yield return (callbackHandler.PlayVFX(param));
        }


    }
}