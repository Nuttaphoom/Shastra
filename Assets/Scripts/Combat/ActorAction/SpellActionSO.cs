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
            return new SpellAbilityRuntime(RequiredEnergy, caster, _targetSelector, _actionSignal);
        }
    }

    public class SpellAbilityRuntime : ActorAction
    {
        private EnergyModifierData _energyModifier;
        public SpellAbilityRuntime(EnergyModifierData energyModifier, CombatEntity caster, TargetSelector targetSelector, ActionSignal actionSignal)
        {
            _energyModifier = energyModifier;
            _caster = caster;
            _targetSelector = targetSelector;
            _actionSignal = new ActionSignal(actionSignal);
        }

        public override IEnumerator PreActionPerform()
        {
            yield return null;
        }

        public override IEnumerator PostActionPerform()
        {
            _caster.SpellCaster.ModifyEnergy(_energyModifier.Side, _energyModifier.Amount);
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
