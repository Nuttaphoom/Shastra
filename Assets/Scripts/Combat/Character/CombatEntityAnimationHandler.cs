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
        private CombatEntity _combatEntity ;

        [SerializeField]
        private GameObject _mesh ;

        private Animator _animator;  
        public CombatEntityAnimationHandler(CombatEntity combatEntity, CombatEntityAnimationHandler copied) {
            _combatEntity = combatEntity;
            _mesh = copied._mesh; 

            _animator = _mesh.GetComponent<Animator>() ;  
        }

        public IEnumerator PlayTriggerAnimation(string triggerName)
        {
            _animator.SetTrigger(triggerName) ;

            yield return new WaitForSeconds(3.0f);
        }
        public IEnumerator PlayActionAnimation(ActionAnimationInfo actionAnimation )
        {
            Debug.Log("PlayActionAnimation Start"); 

            //Self VFX
            GameObject vfx = Instantiate(actionAnimation.CasterVfxAnimationPrefab, _mesh.transform.position, Quaternion.identity);

            //Play Animation 
            yield return PlayTriggerAnimation(actionAnimation.SelfTrigerID);

            Debug.Log("PlayActionAnimation End");
            Destroy(vfx); 
 
        }

        public IEnumerator PlayVFXActionAnimation<T>(ActionAnimationInfo actionAnimation, CombatEntity target, VFXEntity<T>.VFXCallback argc, T param  )
        {
            GameObject vfx = Instantiate(actionAnimation.TargetVfxAnimationPrefab, target.transform.position, Quaternion.identity);
            CombatEntity entity = target;
            VFXEntity<T> vfxEntity = new VFXEntity<T>(target.gameObject, vfx, 3.0f, argc  );
            yield return (vfxEntity.PlayVFX(param));
        }


    }
}