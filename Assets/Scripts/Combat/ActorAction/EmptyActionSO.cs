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
    [CreateAssetMenu(fileName = "Empty Ability", menuName = "ScriptableObject/Combat/EmptyActionSO")]
    public class EmptyActionSO : ActorActionFactory 
    {
        [Header("===Require Energy amount before casting===")]
        [SerializeField]
        private EnergyModifierData _requiredEnergy;

        public EnergyModifierData RequiredEnergy => _requiredEnergy;

        public override ActorAction FactorizeRuntimeAction(CombatEntity caster)
        {
            return new EmptyAbilityRuntime(caster, _targetSelector, _actionSignal);
        }
    }

    public class EmptyAbilityRuntime : ActorAction
    {
        public EmptyAbilityRuntime(CombatEntity caster, TargetSelector targetSelector, ActionSignal actionSignal)
        {
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
            yield return null;
        }

        public override IEnumerator Simulate(CombatEntity target)
        { 
            yield return null;
        }
    }

}
