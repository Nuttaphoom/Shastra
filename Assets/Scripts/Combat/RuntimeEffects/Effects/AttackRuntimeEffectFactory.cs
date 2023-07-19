using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace Vanaring_DepaDemo
{
    [CreateAssetMenu(fileName = "AttackRuntimeEffectFactory", menuName = "ScriptableObject/RuntimeEffect/AttackRuntimeEffectFactory")]
    public class AttackRuntimeEffectFactory : RuntimeEffectFactorySO
    {
        [SerializeField]
        private EDamageScaling _damagScaling;

        [SerializeField]
        private int realDmg = -1;

        [SerializeField]
        private ActionAnimationInfo _actionAnimation;


        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            AttackRuntimeEffect retEffect = new AttackRuntimeEffect(_damagScaling,realDmg, _actionAnimation );
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }

    public class AttackRuntimeEffect : RuntimeEffect
    {
        private EDamageScaling _damagScaling;
        private int _realDmg;
        private ActionAnimationInfo _actionAnimation;  
        public AttackRuntimeEffect(EDamageScaling scaling, int realDmg, ActionAnimationInfo actionAnimation)
        {
            _damagScaling  = scaling;
            _realDmg = realDmg; 
            _actionAnimation = actionAnimation;  

        }
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            //Deal Dmg directly to enemy ignoring the caster 
            if (_realDmg > 0)
            {
                throw new Exception("Direct dmg, without caster, is not function, TODO "); 
            }

            //Command caster to attack enemy 
            if (caster == null)
                throw new Exception("Caster can not be null");

            float mul = 1;  
            switch (_damagScaling)
            {
                case (EDamageScaling.Low):
                    mul = 0.5f; break;
                case (EDamageScaling.Medium):
                    mul = 1.0f; break;
                case (EDamageScaling.High):
                    mul = 1.5f; break;  
            }

            yield return caster.Attack(_targets,mul, _actionAnimation ) ; 
            
        }

         
         





    }
}