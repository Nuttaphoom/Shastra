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
    public class SpellActionSO : ActorActionFactory 
    {
        [Header("===Require Energy amount before casting===")]
        [SerializeField]
        private EnergyModifierData _requiredEnergy;

        public RuntimeMangicalEnergy.EnergySide RequiredSide => _requiredEnergy.Side ;
        public int RequiredAmout => _requiredEnergy.Amount > 0 ? (_requiredEnergy.Amount * -1 ) : (_requiredEnergy.Amount) ; 

        public override ActorAction FactorizeRuntimeAction(CombatEntity caster)
        {
            return new SpellAbilityRuntime(RequiredSide, RequiredAmout, caster, this);
        }
    }

    public class SpellAbilityRuntime : ActorAction
    {
        private int _requiredEnergy;
        private RuntimeMangicalEnergy.EnergySide _requiredSide ; 
        public SpellAbilityRuntime(RuntimeMangicalEnergy.EnergySide  side, int amt, CombatEntity caster, ActorActionFactory factory) : base(factory,caster)
        {
            _requiredEnergy = amt ; 
            _requiredSide = side ;
            
            
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
