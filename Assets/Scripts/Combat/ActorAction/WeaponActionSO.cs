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
    [CreateAssetMenu(fileName = "Physical Ability", menuName = "ScriptableObject/Combat/PhysicalActionSO")]
    public class WeaponActionSO : ActorActionFactory 
    {
        public override ActorAction FactorizeRuntimeAction(CombatEntity caster)
        {
            return new WeaponActionAbilityRuntime(caster, this);
        }
    }

    public class WeaponActionAbilityRuntime : ActorAction
    {
        public WeaponActionAbilityRuntime(CombatEntity caster, ActorActionFactory factory) : base(factory,caster)
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
