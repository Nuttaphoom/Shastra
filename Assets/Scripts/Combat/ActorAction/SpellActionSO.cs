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

        public SpellAbilityRuntime Factorize(CombatEntity caster)
        {
            return new SpellAbilityRuntime(RequiredEnergy, EffectFactory, caster, _targetSelector, _actionSignal); 
        } 

    }

    public class SpellAbilityRuntime : IActorAction
    {       
        private CombatEntity _caster; 

        private EnergyModifierData _energyModifier;

        private RuntimeEffectFactorySO _effect;

        private List<CombatEntity> _targets;

        private TargetSelector _targetSelector; 
        public RuntimeMangicalEnergy.EnergySide ModifiedEnergySide { get { return _energyModifier.Side; } }
        public int ModifiedEnergyAmount { get { return _energyModifier.Amount; } }

        public RuntimeEffectFactorySO EffectFactory { get {  return _effect; } }

        private ActionSignal _actionSignal;

        public SpellAbilityRuntime(EnergyModifierData energyModifier, RuntimeEffectFactorySO effect, CombatEntity caster, TargetSelector targetSelector, ActionSignal actionSignal)
        {
            _effect = effect;
            _energyModifier = energyModifier;
            _caster = caster;
            _targetSelector = targetSelector;
            _actionSignal = new ActionSignal(actionSignal) ;
        }
        public IEnumerator PreActionPerform()
        {
            yield return null; 
        }

        public IEnumerator PostActionPerform()
        {
            _caster.SpellCaster.ModifyEnergy(_caster, _energyModifier.Side, _energyModifier.Amount);

            yield return null; 

        }

        public void SetActionTarget(List<CombatEntity> targets)
        {
            _targets = new List<CombatEntity>(); 
            foreach (var entity in targets)
            {
                _targets.Add(entity);
            }
        }

        public RuntimeEffect GetRuntimeEffect()
        {
             
            return _effect.Factorize(_targets);
        }

        public TargetSelector GetTargetSelector()
        {
            return _targetSelector; 
        }

      
    }


}
