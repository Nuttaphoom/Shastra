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
    [CreateAssetMenu(fileName = "AttackModifyEnergyRuntimeEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/AttackModifyEnergyRuntimeEffectFactorySO")]
    public class AttackModifyEnergyRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        private EDamageScaling _damagScaling;

        [SerializeField]
        private int realDmg = -1;

        [SerializeField]
        private EnergyModifierData _energyModifierData;

        [SerializeField]
        private ActionAnimationInfo _actionAnimation;

        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            AttackModifyEnergyRuntimeEffect retEffect = new AttackModifyEnergyRuntimeEffect(_damagScaling, realDmg, _energyModifierData, _actionAnimation);
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }

    public class AttackModifyEnergyRuntimeEffect : RuntimeEffect
    {
        private EDamageScaling _damagScaling;
        private int _realDmg;
        private ActionAnimationInfo _actionAnimation;
        private EnergyModifierData _data;
        public AttackModifyEnergyRuntimeEffect(EDamageScaling scaling, int realDmg,EnergyModifierData data ,ActionAnimationInfo actionAnimation)
        {
            _damagScaling = scaling;
            _realDmg = realDmg;
            _actionAnimation = actionAnimation;
            _data = data; 

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

            //foreach (CombatEntity target in _targets)
            //{
            //    target.SpellCaster.ModifyEnergy(_data.Side, _data.Amount);
            //}

 

            yield return caster.LogicAttack(_targets, _damagScaling);

            //2 Visual 
            List<IEnumerator> coroutines = new List<IEnumerator>();

            //2.1.) creating vfx for coroutine for targets
            foreach (CombatEntity target in _targets)
            {
                CombatEntity entity = target;
                coroutines.Add(entity.CombatEntityAnimationHandler.PlayVFXActionAnimation<string>(_actionAnimation.TargetVfxEntity, (string s) => AttackModify(caster,entity), _actionAnimation.TargetTrigerID));
            }

            //2.2.) create action animation coroutine for self
            coroutines.Add(caster.CombatEntityAnimationHandler.PlayActionAnimation(_actionAnimation));

            //2.3.) running animation scheme
            yield return new WaitAll(caster, coroutines.ToArray());

        }

        private IEnumerator AttackModify(CombatEntity caster , CombatEntity target)
        {
            Debug.Log("start modify energy in attackmodify");
            List<IEnumerator> coroutines = new List<IEnumerator>();

            coroutines.Add(target.VisualHurt(caster, _actionAnimation.TargetTrigerID) ) ;

            target.SpellCaster.ModifyEnergy(_data.Side, _data.Amount);

            yield return new WaitAll(caster, coroutines.ToArray()); 

            yield return caster.GetStatusEffectHandler().ExecuteAfterAttackStatusRuntimeEffectCoroutine(target) ;


        }








    }
}