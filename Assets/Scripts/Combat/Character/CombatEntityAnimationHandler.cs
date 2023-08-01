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
            Debug.Log("OBJECT IS " + gameObject.name);
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
            GameObject vfx = null ; 
            if (actionAnimation.CasterVfxAnimationPrefab.gameObject != null)
            {
                 vfx = Instantiate(actionAnimation.CasterVfxAnimationPrefab.gameObject, _mesh.transform.position, Quaternion.identity);
            }
            //Play Animation 
            yield return PlayTriggerAnimation(actionAnimation.SelfTrigerID);

            if (vfx != null)
                Destroy(vfx); 
 
        }

        public IEnumerator PlayVFXActionAnimation<T>(VFXEntity vfxBasePrefab, CombatEntity target, VFXCallbackHandler<T>.VFXCallback argc, T param  )
        {
            VFXEntity vfx = Instantiate(vfxBasePrefab, target.CombatEntityAnimationHandler._vfxPos.position, Quaternion.identity);
            CombatEntity entity = target;
            VFXCallbackHandler<T> vfxEntity = new VFXCallbackHandler<T>(target.gameObject, vfx,target.CombatEntityAnimationHandler.GetVFXSpawnPos(), vfxBasePrefab.GetComponent<ParticleSystem>().main.duration, argc  );
            yield return (vfxEntity.PlayVFX(param));
        }


    }
}