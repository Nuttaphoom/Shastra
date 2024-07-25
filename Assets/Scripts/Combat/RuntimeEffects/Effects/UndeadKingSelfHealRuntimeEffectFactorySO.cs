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
    [CreateAssetMenu(fileName = "UndeadKingSelfHealRuntimeEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/UndeadKing/UndeadKingSelfHealRuntimeEffectFactorySO")]
    public class UndeadKingSelfHealRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        private float _flat_healValue;

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            UndeadKingSelfHealRuntimeEffect retEffect = new UndeadKingSelfHealRuntimeEffect(new StatModifier(_flat_healValue, StatModType.Flat));
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            return retEffect;
        }


    }

    public class UndeadKingSelfHealRuntimeEffect : RuntimeEffect
    {
        private StatModifier _healStatModifer;
        public UndeadKingSelfHealRuntimeEffect(StatModifier healStatModifer)
        {
            _healStatModifer = healStatModifer;
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            List<CombatEntity> killTarget = new List<CombatEntity>(); 

            //creating vfx for coroutine for targets
            foreach (CombatEntity t in _targets)
            {
                if (t.CombatCharacterSheet.CharacterName == "Undead King")
                {
                    throw new Exception("Undead King should not be selected as spell target"); 
                 }else
                {
                    killTarget.Add(t); 
                }
                
            }

            foreach (var target in killTarget) 
                target.GetComponent<CombatEntityAnimationHandler>().SetDeadVFXActivation(false);

            yield return caster.LogicAttack(killTarget, EDamageScaling.High);


            caster.LogicHeal(_healStatModifer);


            yield return null;
        }










    }
}