
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
 
using System.Runtime.CompilerServices;
using CustomYieldInstructions;
using UnityEngine.VFX;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "StatusEffectApplierFactorySO", menuName = "ScriptableObject/RuntimeEffect/StatusEffectApplierFactorySO")]
    public class StatusEffectApplierFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        ActionAnimationInfo _actionAnimationInfo;

        [SerializeField]
        private List<StatusRuntimeEffectFactorySO> _effects;
        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            StatusEffectApplierRuntimeEffect retEffect = new StatusEffectApplierRuntimeEffect(_effects, _actionAnimationInfo);
            foreach (CombatEntity target in targets)
                retEffect.AssignTarget(target);

            return retEffect;
        }

        public override void SimulateEnergyModifier(CombatEntity target)
        {
            throw new System.NotImplementedException();
        }
    }


    public class StatusEffectApplierRuntimeEffect : RuntimeEffect
    {
        [SerializeField]
        protected List<StatusRuntimeEffectFactorySO> _effects;

        protected ActionAnimationInfo _actionAnimationInfo;
        public StatusEffectApplierRuntimeEffect(List<StatusRuntimeEffectFactorySO> effects, ActionAnimationInfo actionAnimationInfo)
        {
            _effects = effects;
            _actionAnimationInfo = actionAnimationInfo;
        }


        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            List<IEnumerator> applyEffectCoroutine = new List<IEnumerator>();
            foreach (CombatEntity target in _targets)
            {
                CombatEntity tar = target;

                if (target is not IStatusEffectable)
                    throw new System.Exception("Assigned target is not IStatusEffectable");

                foreach (StatusRuntimeEffectFactorySO effect in _effects)
                {
                    StatusRuntimeEffectFactorySO eff = effect;
                    applyEffectCoroutine.Add(tar.CombatEntityAnimationHandler.PlayVFXActionAnimation<StatusRuntimeEffectFactorySO>(_actionAnimationInfo.TargetVfxEntity, (param) => ApplyEffectCoroutine(param, tar,caster), eff));
                }
            }


            applyEffectCoroutine.Add(caster.CombatEntityAnimationHandler.PlayActionAnimation(_actionAnimationInfo));


            yield return new WaitAll(caster, applyEffectCoroutine.ToArray());

        }

        protected virtual IEnumerator ApplyEffectCoroutine(StatusRuntimeEffectFactorySO effect, CombatEntity target, CombatEntity applier)
        {
            yield return target.GetStatusEffectHandler().ApplyNewEffect(effect, applier);
        }


    }

}