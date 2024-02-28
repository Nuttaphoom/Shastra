
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
 
using Kryz.CharacterStats;

namespace Vanaring 
{

    [CreateAssetMenu(fileName = "EnchanceEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/StatusEffect/EnchanceEffectFactorySO")]
    public class EnhanceATKStatusEffectFactorySO : StatusRuntimeEffectFactorySO
    {
        [SerializeField]
        private StatModifier _enchanceATKStats  ; 

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            EnhanceATKStatusEffect retEffect = new EnhanceATKStatusEffect(this, _enchanceATKStats);
            foreach (CombatEntity target in targets)
            {
                retEffect.AssignTarget(target);
            }

            return retEffect;
        }
    }


    public class EnhanceATKStatusEffect : StatusRuntimeEffect
    {
        private StatModifier _enchanceATKStats ;
       

        public EnhanceATKStatusEffect(StatusRuntimeEffectFactorySO factory, StatModifier statsMod) : base(factory)
        {
            _enchanceATKStats = statsMod; 
        }


        //_cgs can be null =, be careful not assuming he got _cgs 
        
        public override IEnumerator BeforeAttackEffect(CombatEntity caster)
        {
            caster.StatsAccumulator.ModifyATKAmount(_enchanceATKStats);

            _timeToLive = 0;

            yield return null;
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {

            yield return null; 
        }
    }


}