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
            return new EmptyAbilityRuntime(this,  caster);
        }
    }

    public class EmptyAbilityRuntime : ActorAction
    {
        public EmptyAbilityRuntime(ActorActionFactory actorAction, CombatEntity caster) : base(actorAction, caster)
        {

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
