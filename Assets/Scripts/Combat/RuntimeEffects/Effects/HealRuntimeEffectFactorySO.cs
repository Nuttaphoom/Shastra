using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.EventSystems.EventTrigger;


namespace Vanaring_DepaDemo
{
    [CreateAssetMenu(fileName = "HealRuntimeEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/HealRuntimeEffectFactorySO")]
    public class HealRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        private float _hp = 0 ;
 

        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            HealRuntimeEffect retEffect = new HealRuntimeEffect(_actionAnimation, _hp);
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }

    public class HealRuntimeEffect : RuntimeEffect
    {
        ActionAnimationInfo _actionAnimationInfo ; 
        private float _hp = 0; 
        public HealRuntimeEffect(ActionAnimationInfo actionAnimation , float hp )
        {
            _actionAnimationInfo = actionAnimation;
            _hp = hp; 
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            //Deal Dmg directly to enemy ignoring the caster 

            //Command caster to attack enemy 
            if (caster == null)
                throw new Exception("Caster can not be null"); 

        }

        private IEnumerator AttackModify(CombatEntity caster, CombatEntity target)
        {
            Debug.Log("start modify energy in attackmodify");
            List<IEnumerator> coroutines = new List<IEnumerator>();

            coroutines.Add(target.VisualHurt(caster, _actionAnimation.TargetTrigerID));

            target.SpellCaster.ModifyEnergy(_data.Side, _data.Amount);

            yield return new WaitAll(caster, coroutines.ToArray());

            yield return caster.GetStatusEffectHandler().ExecuteAfterAttackStatusRuntimeEffectCoroutine(target);


        }








    }
}