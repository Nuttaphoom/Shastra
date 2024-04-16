using Kryz.CharacterStats;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine; 
namespace Vanaring
{
    [CreateAssetMenu(fileName = "Attribute Modification StatusEffect FactorySO", menuName = "ScriptableObject/RuntimeEffect/StatusEffect/Attribute Mod")]

    public class AttributeModificationStatusEffectFactorySO : StatusRuntimeEffectFactorySO
    {
        [SerializeField]
        private ECharacterSecondaryAttributes secondaryAttributeType;

        [SerializeField]
        private float flat_enchanceATKPercentage;
        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            var statMod = new StatModifier(flat_enchanceATKPercentage, StatModType.PercentAdd);
            var ret = new AttributeModificationStatusRuntimeEffect(this, statMod, secondaryAttributeType); 
            foreach (var entity in targets)
                ret.AssignTarget(entity);
            return ret;
        }
    }

    public class AttributeModificationStatusRuntimeEffect : StatusRuntimeEffect
    {

        private ECharacterSecondaryAttributes secondaryAttributeType ;

        private StatModifier statsMod;
        public AttributeModificationStatusRuntimeEffect(StatusRuntimeEffectFactorySO factory, StatModifier mod, ECharacterSecondaryAttributes attributeType) : base(factory)
        {
            secondaryAttributeType = attributeType;
            statsMod = mod;
        }

        public override IEnumerator OnStatusEffectApplied(CombatEntity applier)
        {
            foreach (var target in _targets)
            {
                target.StatsAccumulator.AddModifierSecondaryAtttributes(secondaryAttributeType, statsMod);
            }

            yield return null;//(applier);
        }
        public override IEnumerator OnStatusEffecExpire(CombatEntity caster)
        {
            Debug.Log("Remove physical mod because of expiration");
            yield return base.OnStatusEffecExpire(caster);
            foreach (var target in _targets)
                target.StatsAccumulator.RemoveModifierSecondaryAttributes(secondaryAttributeType, statsMod);
        }



      
    }

}
