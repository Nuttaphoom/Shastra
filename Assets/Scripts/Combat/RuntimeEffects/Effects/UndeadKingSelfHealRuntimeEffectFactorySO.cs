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
            CombatEntity undeadKing = null ;
            List<CombatEntity> killTarget = new List<CombatEntity>(); 

            //creating vfx for coroutine for targets
            foreach (CombatEntity t in _targets)
            {
                if (t.CombatCharacterSheet.CharacterName == "Undead King")
                {
                    undeadKing = t;  
                }else
                {
                    killTarget.Add(t); 
                }
                
            }

            if (undeadKing == null)
                throw new Exception("undeaKing variable can not be null");

            Debug.Log("undead king is " + undeadKing);
            Debug.Log("killed target .count is " + killTarget.Count); 

            yield return caster.LogicAttack(killTarget, EDamageScaling.High);

            undeadKing.LogicHeal(_healStatModifer);


            yield return null;
        }










    }
}