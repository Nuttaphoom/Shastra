using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring_DepaDemo
{ 
    [CreateAssetMenu(fileName = "Item Ability", menuName = "ScriptableObject/Combat/ItemAbility")]
    public class ItemAbilitySO : CombatActionSO
    {
        [Header("===Effect of the item===")]
        [SerializeField]
        private int _itemEffect;

        //[Header("===Require Energy amount before casting===")]
        //[SerializeField]
        //private EnergyModifierData _requiredEnergy;

        //[Header("===Energy modified after the spell is cast===")]
        //[SerializeField]
        //private EnergyModifierData _energyModifier;

        public ItemAbilityRuntime Factorize()
        {
            return new ItemAbilityRuntime(_itemEffect, EffectFactory);
        }

    }

    public class ItemAbilityRuntime
    {
        public ItemAbilityRuntime(int itemEffect, RuntimeEffectFactorySO effect)
        {
            _effect = effect;
            _itemEffect = itemEffect;
        }

        private int _itemEffect;
        private RuntimeEffectFactorySO _effect;

        //public RuntimeMangicalEnergy.EnergySide RequireEnergySide { get { return _requiredEnergy.Side; } }
        //public int RequireEnergyAmount { get { return _requiredEnergy.Amount; } }

        //public RuntimeMangicalEnergy.EnergySide ModifiedEnergySide { get { return _energyModifier.Side; } }

        public int ItemEffect { get { return _itemEffect; } }

        public RuntimeEffectFactorySO EffectFactory { get { return _effect; } }
    }

}
