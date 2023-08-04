using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace Vanaring_DepaDemo
{
    [CreateAssetMenu(fileName = "EnergyModifierRuntimeEffectFactory", menuName = "ScriptableObject/RuntimeEffect/EnergyModifierRuntimeEffectFactory")]
    public class EnergyModifierRuntimeEffectFactory : RuntimeEffectFactorySO
    {
        [SerializeField]
        private EnergyModifierData _data ;

        [SerializeField]
        private ActionAnimationInfo _actionAnimation;

        public EnergyModifierData ModifierData => _data;
        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            EnergyModifierRuntimeEffect retEffect = new EnergyModifierRuntimeEffect(_data, _actionAnimation);
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }

    public class EnergyModifierRuntimeEffect : RuntimeEffect
    {
        private EnergyModifierData _data;
        private ActionAnimationInfo _actionAnimationInfo;
        public EnergyModifierRuntimeEffect(EnergyModifierData data, ActionAnimationInfo actionAnimation)
        {
            _actionAnimationInfo = actionAnimation; 
            _data = data;
        }
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            foreach (var target in _targets)
            {
                CombatEntity entity = target;
                yield return (target.CombatEntityAnimationHandler.PlayVFXActionAnimation<CombatEntity>(_actionAnimationInfo.TargetVfxEntity, ModifyenergyCoroutine, entity)); 
            }


        }

        private IEnumerator ModifyenergyCoroutine(CombatEntity target)
        {
            target.SpellCaster.ModifyEnergy(_data.Side, _data.Amount);
            yield return null; 
        }








    }
}