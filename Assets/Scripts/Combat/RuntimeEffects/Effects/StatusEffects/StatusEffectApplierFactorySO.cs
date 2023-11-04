
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
        private List<StatusRuntimeEffectFactorySO> _effects;
        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            StatusEffectApplierRuntimeEffect retEffect = new StatusEffectApplierRuntimeEffect(_effects);
            foreach (CombatEntity target in targets)
                retEffect.AssignTarget(target);

            return retEffect;
        }

        public override void SimulateEnergyModifier(CombatEntity target)
        {
            
        }
    }


    public class StatusEffectApplierRuntimeEffect : RuntimeEffect
    {
        [SerializeField]
        protected List<StatusRuntimeEffectFactorySO> _effects;

        public StatusEffectApplierRuntimeEffect(List<StatusRuntimeEffectFactorySO> effects)
        {
            _effects = effects;
        }


        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
             foreach (CombatEntity target in _targets)
            {
                CombatEntity tar = target;

                if (target is not IStatusEffectable)
                    throw new System.Exception("Assigned target is not IStatusEffectable");


                List<IEnumerator> coroutines = new List<IEnumerator>();
                foreach (StatusRuntimeEffectFactorySO effect in _effects)
                {
                    StatusRuntimeEffectFactorySO eff = effect;
                    coroutines.Add(target.GetStatusEffectHandler().ApplyNewEffect(effect, caster)); 
                }

                yield return new WaitAll(caster,coroutines.ToArray());
            }

            yield return null; 
        }

        protected virtual IEnumerator ApplyEffectCoroutine(StatusRuntimeEffectFactorySO effect, CombatEntity target, CombatEntity applier)
        {
            yield return target.GetStatusEffectHandler().ApplyNewEffect(effect, applier);
        }


    }

}