using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace Vanaring
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

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            AttackRuntimeEffect retEffect = new AttackRuntimeEffect(_damagScaling,realDmg, _actionAnimation );
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            return retEffect;
        }

        public override void SimulateEnergyModifier(CombatEntity combatEntity)
        {
            throw new Exception("have been impleented"); 
            combatEntity.SpellCaster.Simulate(RuntimeMangicalEnergy.EnergySide.LightEnergy, 40, null); 
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
 
            yield return caster.LogicAttack(_targets, _damagScaling ) ;

            //2 Visual 
            List<IEnumerator> coroutines = new List<IEnumerator>();

            //2.1.) creating vfx for coroutine for targets
            foreach (CombatEntity target in _targets)
            {
                CombatEntity entity = target;
                if (!_actionAnimation.IsProjectile)
                {
                    coroutines.Add(entity.CombatEntityAnimationHandler.PlayVFXActionAnimation<string>(
                        _actionAnimation.TargetVfxEntity, (string s) => entity.VisualHurt(caster, s), 
                        _actionAnimation.TargetTrigerID));
                }
                else
                {
                    coroutines.Add(caster.CombatEntityAnimationHandler.PlayVFXActionAnimation<string>(
                        _actionAnimation.TargetVfxEntity, (string s) => entity.VisualHurt(caster, s),
                        _actionAnimation.TargetTrigerID, caster.gameObject.transform.position, entity.gameObject.transform.position));
                }
            }

            //2.2.) create action animation coroutine for self
            coroutines.Add(caster.CombatEntityAnimationHandler.PlayActionAnimation(_actionAnimation));

            //2.3.) running animation scheme

            yield return new WaitAll(caster, coroutines.ToArray());

            //3.) Running after attack
            coroutines.Clear(); 
            foreach (CombatEntity target in _targets)
            {
                CombatEntity entity = target;
                coroutines.Add(caster.GetStatusEffectHandler().ExecuteAfterAttackStatusRuntimeEffectCoroutine(target));
            }

            yield return new WaitAll(caster, coroutines.ToArray());


        }

       








    }
}