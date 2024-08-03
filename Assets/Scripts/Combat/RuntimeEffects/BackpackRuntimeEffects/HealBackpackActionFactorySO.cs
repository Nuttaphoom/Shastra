using Kryz.CharacterStats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "HealBackpackRuntimeEffectFactorySO", menuName = "ScriptableObject/BackpackRuntimeEffect/HealBackpackRuntimeEffectFactorySO")]

    public class HealBackpackActionFactorySO : BackpackActionFactorySO
    {
        [SerializeField]
        private float _flat_healValue;

        public override BackpackItemAbilityRuntime FactorizeBackpackItemAbilityRuntime(RuntimePartyMember caster, List<RuntimePartyMember> targets)
        {
            return new HealBackpackRuntimeEffect(new StatModifier(_flat_healValue, StatModType.Flat), caster,this,targets);
        }
    }

    public class HealBackpackRuntimeEffect : BackpackItemAbilityRuntime
    {
        private StatModifier _healStatModifer;
        public HealBackpackRuntimeEffect(StatModifier healStatModifer, RuntimePartyMember caster, BackpackActionFactorySO factory, List<RuntimePartyMember> targets) : base(caster,factory,targets) 
        {
            _healStatModifer = healStatModifer;
        }

        public override IEnumerator UseItemAbilityOutsideCombat()
        {
            Debug.Log("" + caster + " heal " + targets[0] + " for " + _healStatModifer.Value); 
            
            foreach (var target in targets)
            {
                target.ModifyPartyMembetHP(_healStatModifer.Value);
            }
            yield return null;
        }










    }


}

 
 