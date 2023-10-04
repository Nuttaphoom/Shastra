using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace Vanaring 
{
    [CreateAssetMenu(fileName = "EnergyModifierRuntimeEffectFactory", menuName = "ScriptableObject/RuntimeEffect/EnergyModifierRuntimeEffectFactory")]
    public class EnergyModifierRuntimeEffectFactory : RuntimeEffectFactorySO
    {
        [SerializeField]
        private EnergyModifierData _data ;

        [SerializeField]
        private ActionAnimationInfo _actionAnimation;

        public EnergyModifierData ModifierData => _data;
        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            EnergyModifierRuntimeEffect retEffect = new EnergyModifierRuntimeEffect(_data, _actionAnimation);
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            return retEffect;
        }

        public override void SimulateEnergyModifier(CombatEntity target)
        {
            throw new NotImplementedException();
        }
    }

    public class EnergyModifierRuntimeEffect : RuntimeEffect
    {
        private EnergyModifierData _data;
        private ActionAnimationInfo _actionAnimationInfo;
        private CombatEntity _caster; 
        public EnergyModifierRuntimeEffect(EnergyModifierData data, ActionAnimationInfo actionAnimation)
        {
            _actionAnimationInfo = actionAnimation; 
            _data = data;
        }
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            List<IEnumerator> iEnumerators = new List<IEnumerator>();
            _caster = caster;

            iEnumerators.Add(_caster.CombatEntityAnimationHandler.PlayTriggerAnimation(_actionAnimationInfo.SelfTrigerID));

            foreach (var target in _targets)
            {
                CombatEntity entity = target;
                iEnumerators.Add (target.CombatEntityAnimationHandler.PlayVFXActionAnimation<CombatEntity>(_actionAnimationInfo.TargetVfxEntity, ModifyenergyCoroutine, entity));
                iEnumerators.Add(_caster.CombatEntityAnimationHandler.PlayVFXActionAnimation<CombatEntity>(_actionAnimationInfo.CasterVfxEntity, null, entity));

            }

            yield return new WaitAll(_caster,iEnumerators.ToArray() ) ;
        }

        private IEnumerator ModifyenergyCoroutine(CombatEntity target)
        {
            target.SpellCaster.ModifyEnergy(_caster, _data.Side, _data.Amount);
            List<IEnumerator> iEnumerators = new List<IEnumerator>();
            iEnumerators.Add(target.CombatEntityAnimationHandler.PlayTriggerAnimation(_actionAnimationInfo.TargetTrigerID));

            yield return new WaitAll(_caster, iEnumerators.ToArray());
        }








    }
}