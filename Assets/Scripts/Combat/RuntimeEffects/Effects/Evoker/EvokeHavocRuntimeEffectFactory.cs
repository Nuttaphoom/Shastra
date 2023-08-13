using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace Vanaring_DepaDemo
{
    [CreateAssetMenu(fileName = "EvokeHavocRuntimeEffectFactory", menuName = "ScriptableObject/RuntimeEffect/EvokeHavocRuntimeEffectFactory")]
    public class EvokeHavocRuntimeEffectFactory : RuntimeEffectFactorySO
    {
 

        [Header("Energy will be n^ number of havoc")]
        [SerializeField]
        private EnergyModifierData _energyModifierData;

        [SerializeField]
        private ActionAnimationInfo _actionAnimationInfo; 
        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            EvokeHavocRuntimeEffect retEffect = new EvokeHavocRuntimeEffect(_energyModifierData,_actionAnimationInfo);
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }

    public class EvokeHavocRuntimeEffect : RuntimeEffect
    {
        private EnergyModifierData _energyModifierData;

        private ActionAnimationInfo _actionAnimationInfo;
        public EvokeHavocRuntimeEffect(EnergyModifierData data, ActionAnimationInfo animation )
        {
            _energyModifierData = data ;
            _actionAnimationInfo = animation;

        }
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            //Command caster to attack enemy 
            if (caster == null)
                throw new Exception("Caster can not be null");


            foreach (CombatEntity entity in _targets)
            {
                List<StatusRuntimeEffect> havoc = entity.GetStatusEffectHandler().GetStatusRuntimeEffectWithEvokeKey(EEvokeKey.HAVOC, true);

                entity.SpellCaster.ModifyEnergy(_energyModifierData.Side, _energyModifierData.Amount * havoc.Count);

            }

            yield return caster.LogicAttack(_targets, EDamageScaling.High) ;

            //2 Visual 
            List<IEnumerator> coroutines = new List<IEnumerator>();

            //2.1.) creating vfx for coroutine for targets
            foreach (CombatEntity target in _targets)
            {
                CombatEntity entity = target;
                coroutines.Add(entity.CombatEntityAnimationHandler.PlayVFXActionAnimation<string>(_actionAnimationInfo.TargetVfxEntity, (string s) => entity.VisualHurt(caster, s), _actionAnimationInfo.TargetTrigerID));
            }

            //2.2.) create action animation coroutine for self
            coroutines.Add(caster.CombatEntityAnimationHandler.PlayActionAnimation(_actionAnimationInfo));

            //2.3.) running animation scheme
            yield return new WaitAll(caster, coroutines.ToArray());


            coroutines.Clear(); 

            //2.1.) creating vfx for coroutine for targets
            foreach (CombatEntity target in _targets)
            {
                CombatEntity entity = target;
                coroutines.Add(caster.GetStatusEffectHandler().ExecuteAfterAttackStatusRuntimeEffectCoroutine(target));
            }

            yield return new WaitAll(caster, coroutines.ToArray());

        }
    }
}