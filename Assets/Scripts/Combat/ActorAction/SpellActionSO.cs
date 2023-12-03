using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{ 
    [CreateAssetMenu(fileName = "Spell Ability", menuName = "ScriptableObject/Combat/SpellAbility")]
    public class SpellActionSO : ActorActionFactory 
    {
        [Header("===Require Energy amount before casting===")]
        [SerializeField]
        private EnergyModifierData _requiredEnergy;

        public EnergyModifierData RequiredEnergy => _requiredEnergy;

        public override ActorAction FactorizeRuntimeAction(CombatEntity caster)
        {
            return new SpellAbilityRuntime(RequiredEnergy, caster, this);
        }
    }

    public class SpellAbilityRuntime : ActorAction
    {
        private int _requiredEnergy;
        private RuntimeMangicalEnergy.EnergySide _requiredSide ; 
        public SpellAbilityRuntime(EnergyModifierData energyModifier, CombatEntity caster, ActorActionFactory factory) : base(factory,caster)
        {
            _requiredEnergy = energyModifier.Amount; 
            _requiredSide = energyModifier.Side;
            
            
            if (_requiredEnergy > 0)
                 _requiredEnergy = -1 * _requiredEnergy; 
            
        }

        public override IEnumerator PreActionPerform()
        {
            yield return null;
        }

        public override IEnumerator PostActionPerform()
        {
            _caster.SpellCaster.ModifyEnergy(_requiredSide, _requiredEnergy );
            yield return null;
        }

        public override IEnumerator Simulate(CombatEntity target)
        {
            foreach (var factory in _actionSignal.GetRuntimeEffects() )
            {
                factory.SimulateEnergyModifier(target); 
            }
            yield return null;
        }
    }

}
