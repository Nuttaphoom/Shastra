using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace Vanaring_DepaDemo
{
    [CreateAssetMenu(fileName = "AttackModifyEnergyRuntimeEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/AttackModifyEnergyRuntimeEffectFactorySO")]
    public class AttackModifyEnergyRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        private EDamageScaling _damagScaling;

        [SerializeField]
        private int realDmg = -1;

        [SerializeField]
        private EnergyModifierData _energyModifierData;

        [SerializeField]
        private ActionAnimationInfo _actionAnimation;

        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            AttackModifyEnergyRuntimeEffect retEffect = new AttackModifyEnergyRuntimeEffect(_damagScaling, realDmg, _energyModifierData, _actionAnimation);
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }

    public class AttackModifyEnergyRuntimeEffect : RuntimeEffect
    {
        private EDamageScaling _damagScaling;
        private int _realDmg;
        private ActionAnimationInfo _actionAnimation;
        private EnergyModifierData _data;
        public AttackModifyEnergyRuntimeEffect(EDamageScaling scaling, int realDmg,EnergyModifierData data ,ActionAnimationInfo actionAnimation)
        {
            _damagScaling = scaling;
            _realDmg = realDmg;
            _actionAnimation = actionAnimation;
            _data = data; 

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

            yield return caster.Attack(_targets, _damagScaling, _actionAnimation);

            foreach (CombatEntity target in _targets) {
                target.SpellCaster.ModifyEnergy(_data.Side,_data.Amount) ;
            }

        }








    }
}