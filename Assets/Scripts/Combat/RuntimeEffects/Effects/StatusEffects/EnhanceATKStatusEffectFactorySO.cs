
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace Vanaring_DepaDemo
{

    [CreateAssetMenu(fileName = "EnchanceEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/StatusEffect/EnchanceEffectFactorySO")]
    public class EnhanceATKStatusEffectFactorySO : StatusRuntimeEffectFactorySO
    {
        [Header("100 = increased by 100%")]
        [SerializeField]
        private int _modifiedPercent = 0 ;

        [SerializeField]
        private int _realIncreasedAmount = 0 ; 

        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            EnhanceATKStatusEffect retEffect = new EnhanceATKStatusEffect(this, _modifiedPercent, _realIncreasedAmount);
            foreach (CombatEntity target in targets)
            {
                retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }


    public class EnhanceATKStatusEffect : StatusRuntimeEffect
    {
        private int _modifiedPercent = 0 ;
        private int _realIncreasedAmount = 0; 

        public EnhanceATKStatusEffect(StatusRuntimeEffectFactorySO factory, int modifiedPercent,int realIncreasedAmount) : base(factory)
        {
            this._modifiedPercent = modifiedPercent ; 
            this._realIncreasedAmount = realIncreasedAmount ; 

        }


        //_cgs can be null =, be careful not assuming he got _cgs 
        
        public override IEnumerator BeforeAttackEffect(CombatEntity caster)
        {
            caster.StatsAccumulator.ModifyATKAmount(_realIncreasedAmount);
            caster.StatsAccumulator.ModifyATKAmountByPercent(_modifiedPercent);

            _timeToLive = 0; 

            yield return null;
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {

            yield return null; 
        }
    }


}