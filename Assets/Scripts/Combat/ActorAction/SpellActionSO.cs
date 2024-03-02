using Kryz.CharacterStats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
namespace Vanaring
{ 
    [CreateAssetMenu(fileName = "Spell Ability", menuName = "ScriptableObject/Combat/SpellAbility")]
    public class SpellActionSO : ActorActionFactory , IRewardable
    {
        [Header("===Require Energy amount before casting===")]
        [SerializeField]
        private EnergyModifierData _requiredEnergy;

        [SerializeField]
        private int _MPCost; 

        public RuntimeMangicalEnergy.EnergySide RequiredSide => _requiredEnergy.Side ;
        public int RequiredAmout => _requiredEnergy.Amount > 0 ? (_requiredEnergy.Amount * -1) : (_requiredEnergy.Amount);

        public int MPCost => _MPCost    ;
        public override ActorAction FactorizeRuntimeAction(CombatEntity caster)
        {
            return new SpellAbilityRuntime(RequiredSide, MPCost, caster, this);
        }

        public RewardData GetRewardData()
        {
            return new RewardData()
            {
                RewardName = DescriptionBaseField.FieldName,
                RewardIcon = DescriptionBaseField.FieldImage  
            };
        }

        public void SubmitReward()
        {
            PersistentPlayerPersonalDataManager.Instance.PartyMemberDataLocator.GetProtagonistRuntimeData.UnlockSpellActionSO(this);
        }
    }

    public class SpellAbilityRuntime : ActorAction
    {
        private int _requiredEnergy;
        private RuntimeMangicalEnergy.EnergySide _requiredSide ; 
        private int _MPCost;
        public SpellAbilityRuntime(RuntimeMangicalEnergy.EnergySide  side, int MPCost, CombatEntity caster, ActorActionFactory factory) : base(factory,caster)
        {
            _requiredEnergy = MPCost; 
            _requiredSide = side ;
            _MPCost = MPCost; 

            if (_requiredEnergy > 0)
                 _requiredEnergy = -1 * _requiredEnergy; 
            
        }

        public override IEnumerator PreActionPerform()
        {
            _caster.SpellCaster.ModifyMP(new StatModifier(_MPCost > 0 ? -_MPCost : _MPCost, StatModType.Flat));

            yield return null;
        }

        public override IEnumerator PostActionPerform()
        {
            //_caster.SpellCaster.ModifyEnergy(_requiredSide, _requiredEnergy );
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
