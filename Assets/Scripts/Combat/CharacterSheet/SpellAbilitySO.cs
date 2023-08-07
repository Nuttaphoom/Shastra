using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring_DepaDemo
{ 
    [CreateAssetMenu(fileName = "Spell Ability", menuName = "ScriptableObject/Combat/SpellAbility")]
    public class SpellAbilitySO : CombatActionSO
    {
        [Header("===Require Energy amount before casting===")]
        [SerializeField]
        private EnergyModifierData _requiredEnergy;

        [Header("===Energy modified after the spell is cast===")]
        [SerializeField]
        private EnergyModifierData _energyModifier;


        public EnergyModifierData RequiredEnergy => _requiredEnergy;
        public EnergyModifierData EnergyModifer => _energyModifier;

        public SpellAbilityRuntime Factorize()
        {
            return new SpellAbilityRuntime(_requiredEnergy,_energyModifier, EffectFactory); 
        } 

    }

    public class SpellAbilityRuntime
    {
        public SpellAbilityRuntime(EnergyModifierData requiredEnergy, EnergyModifierData energyModifier, RuntimeEffectFactorySO effect)
        {
            _effect = effect; 
            _requiredEnergy = requiredEnergy ;
            _energyModifier = energyModifier ;
        }

        private EnergyModifierData _requiredEnergy;

        private EnergyModifierData _energyModifier;

        private RuntimeEffectFactorySO _effect; 
        public RuntimeMangicalEnergy.EnergySide RequireEnergySide { get { return _requiredEnergy.Side; } }
        public int RequireEnergyAmount { get { return _requiredEnergy.Amount; } }

        public RuntimeMangicalEnergy.EnergySide ModifiedEnergySide { get { return _energyModifier.Side; } }
        public int ModifiedEnergyAmount { get { return _energyModifier.Amount; } }

        public RuntimeEffectFactorySO EffectFactory { get {  return _effect; } }
    }


}
