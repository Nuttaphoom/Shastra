
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace Vanaring 
{

    [CreateAssetMenu(fileName = "EnhanceEnergyModiferStatusEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/StatusEffect/EnhanceEnergyModiferStatusEffectFactorySO")]
    public class EnhanceEnergyModiferStatusEffectFactorySO : StatusRuntimeEffectFactorySO
    {
        [SerializeField]
        private EnergyModifierData _data;

        [SerializeField]
        private ActionAnimationInfo _actionAnimation;

        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            EnhanceEnergyModiferStatusEffect retEffect = new EnhanceEnergyModiferStatusEffect(this, _data, _actionAnimation);
            foreach (CombatEntity target in targets)
            {
                retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }


    public class EnhanceEnergyModiferStatusEffect : StatusRuntimeEffect
    {
        private EnergyModifierData _data;

        private ActionAnimationInfo _actionAnimation;

        public EnhanceEnergyModiferStatusEffect(StatusRuntimeEffectFactorySO factory , EnergyModifierData data, ActionAnimationInfo actionAnimation ) : base(factory)
        {
            _data = data; 
            _actionAnimation = actionAnimation; 
        }


        //_cgs can be null =, be careful not assuming he got _cgs 

        public override IEnumerator AfterAttackEffect(CombatEntity attacker, CombatEntity subject)
        {
             
            subject.SpellCaster.ModifyEnergy(attacker,_data.Side, _data.Amount);
            _timeToLive = 0;
            yield return new WaitForSeconds(0.5f) ;
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {

            yield return null;
        }
    }


}