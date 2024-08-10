using Kryz.CharacterStats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "HealBackpackRuntimeEffectFactorySO", menuName = "ScriptableObject/BackpackRuntimeEffect/MPRestoreBackpackRuntimeEffectFactorySO")]

    public class RestoreMPBackpackActionFactorySO : BackpackActionFactorySO
    {
        [SerializeField]
        private float _flat_MPRestoreValue;

        public override BackpackItemAbilityRuntime FactorizeBackpackItemAbilityRuntime(RuntimePartyMember caster, List<RuntimePartyMember> targets)
        {
            return new RestoreMPBackpackRuntimeEffect(new StatModifier(_flat_MPRestoreValue, StatModType.Flat), caster,this,targets);
        }
    }

    public class RestoreMPBackpackRuntimeEffect : BackpackItemAbilityRuntime
    {
        private StatModifier _mpRestoreStatModifer;
        public RestoreMPBackpackRuntimeEffect(StatModifier mpRestoreStatModifer, RuntimePartyMember caster, BackpackActionFactorySO factory, List<RuntimePartyMember> targets) : base(caster,factory,targets) 
        {
            _mpRestoreStatModifer = mpRestoreStatModifer ;
        }

        public override IEnumerator UseItemAbilityOutsideCombat()
        {
            Debug.Log("" + caster + " restore mp " + targets[0].GetRuntimeCombatMemberData.GetMemberName + " for " + _mpRestoreStatModifer.Value); 
            
            foreach (var target in targets)
            {
                target.ModifyPartyMembetMP(_mpRestoreStatModifer.Value);
            }
            yield return null;
        }










    }


}

 
 