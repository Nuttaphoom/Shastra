using CustomYieldInstructions;
using Kryz.CharacterStats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.EventSystems.EventTrigger;


namespace Vanaring
{
    [CreateAssetMenu(fileName = "HealRuntimeEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/HealRuntimeEffectFactorySO")]
    public class HealRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        private StatModifier _healStatModifer ;

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            HealRuntimeEffect retEffect = new HealRuntimeEffect(_healStatModifer);
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            return retEffect;
        }

      
    }

    public class HealRuntimeEffect : RuntimeEffect
    {
        private StatModifier _healStatModifer;
        public HealRuntimeEffect(StatModifier  healStatModifer)
        {
            _healStatModifer = healStatModifer; 
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            //Command caster to attack enemy 
            if (caster == null)
                throw new Exception("Caster can not be null");

            //creating vfx for coroutine for targets
            foreach (CombatEntity target in _targets)
            {
                CombatEntity entity = target;
                entity.LogicHeal(_healStatModifer) ;
            }

            yield return null;
        }










    }
}