
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

        [SerializeField]
        private Comment _comment_on_applied;

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            StatusEffectApplierRuntimeEffect retEffect = new StatusEffectApplierRuntimeEffect(this,_effects);
            foreach (CombatEntity target in targets)
                retEffect.AssignTarget(target);

            return retEffect;
        }

       

        public Comment GetCommentOnApplied => _comment_on_applied;

    }


    public class StatusEffectApplierRuntimeEffect : RuntimeEffect
    {
        protected List<StatusRuntimeEffectFactorySO> _effects;
        private Comment _comment_on_applied;


        public StatusEffectApplierRuntimeEffect(StatusEffectApplierFactorySO applierSO, List<StatusRuntimeEffectFactorySO> effects)
        {
            _effects = effects;
            _comment_on_applied = applierSO.GetCommentOnApplied; 
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
             foreach (CombatEntity target in _targets)
            {
                List<IEnumerator> coroutines = new List<IEnumerator>();
                foreach (StatusRuntimeEffectFactorySO effect in _effects)
                {
                    StatusRuntimeEffectFactorySO eff = effect;
                    coroutines.Add(target.ApplyNewEffect(effect,this, caster)); 
                }

                yield return new WaitAll(caster,coroutines.ToArray());
            }

            yield return null; 
        }

        public Comment GetCommentOnApplied => _comment_on_applied;




    }

}