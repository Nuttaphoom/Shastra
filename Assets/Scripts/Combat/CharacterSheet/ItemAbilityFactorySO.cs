using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring_DepaDemo
{ 
    [CreateAssetMenu(fileName = "Item Ability", menuName = "ScriptableObject/Combat/ItemAbility")]
    public class ItemAbilityFactorySO : CombatActionSO
    {
         

        //[Header("===Require Energy amount before casting===")]
        //[SerializeField]
        //private EnergyModifierData _requiredEnergy;

        //[Header("===Energy modified after the spell is cast===")]
        //[SerializeField]
        //private EnergyModifierData _energyModifier;

        public ItemAbilityRuntime FactorizeRuntimeItem()
        {
            return new ItemAbilityRuntime(EffectFactory,this) ;
        }

    }

    public class ItemAbilityRuntime
    {
        private string _itemName;
        private string _description;

        private ItemAbilityFactorySO _factory; 
        private RuntimeEffectFactorySO _effectFactory ;

        public ItemAbilityRuntime(RuntimeEffectFactorySO effect, ItemAbilityFactorySO factory)
        {
            _effectFactory = effect;
             _factory = factory;  
        }


        //public RuntimeMangicalEnergy.EnergySide RequireEnergySide { get { return _requiredEnergy.Side; } }
        //public int RequireEnergyAmount { get { return _requiredEnergy.Amount; } }

        //public RuntimeMangicalEnergy.EnergySide ModifiedEnergySide { get { return _energyModifier.Side; } }

        public RuntimeEffectFactorySO EffectFactory { get { return _effectFactory; } }
        public string ItemName => _factory.AbilityName;
        public string ItemDescrption => _factory.Desscription;
        
        

    }

}
