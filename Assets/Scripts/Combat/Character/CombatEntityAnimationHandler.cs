using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


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

        public IEnumerator PlayActionAnimation(ActionAnimationInfo actionAnimation, List<CombatEntity> targets = null)
        {
            //AnimatorOverrideController overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

            //overrideController.runtimeAnimatorController.animationClips[0] = null;  

            //_animator.StopPlayback();

            //_animator.Play(actionAnimation.SkeletonAnimationClip);  
            _animator.SetTrigger(actionAnimation.TrigerID) ;

            List<GameObject> vfxs = new List<GameObject>();
            GameObject vfx = Instantiate(actionAnimation.CasterVfxAnimationPrefab, _mesh.transform.position, Quaternion.identity);
            vfxs.Add(vfx);
            if (targets != null)
            {
                foreach (CombatEntity combatEntity in targets)
                {
                    vfx = Instantiate(actionAnimation.TargetVfxAnimationPrefab, combatEntity.transform.position, Quaternion.identity);
                    vfxs.Add(vfx);
                }
            }

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null ;  
            }

            yield return new WaitForSeconds(3.0f) ;

            foreach (GameObject gameObject in vfxs)
            {
                gameObject.SetActive(false);
            }
        }
    }
}