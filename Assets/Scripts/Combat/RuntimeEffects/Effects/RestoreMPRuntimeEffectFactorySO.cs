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
    [CreateAssetMenu(fileName = "RestoreMPRuntimeEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/RestoreMPRuntimeEffectFactorySO")]
    public class RestoreMPRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        private float _flat_RESTOREmpValue ;

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            RestoreMPRuntimeEffect retEffect = new RestoreMPRuntimeEffect(new StatModifier(_flat_RESTOREmpValue, StatModType.Flat));
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            return retEffect;
        }

      
    }

    public class RestoreMPRuntimeEffect : RuntimeEffect
    {
        private StatModifier _MPStatModifer;
        public RestoreMPRuntimeEffect(StatModifier MPStatModifer)
        {
			_MPStatModifer = MPStatModifer; 
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
                entity.SpellCaster.ModifyMP(_MPStatModifer) ;
            }

            yield return null;
        }










    }
}