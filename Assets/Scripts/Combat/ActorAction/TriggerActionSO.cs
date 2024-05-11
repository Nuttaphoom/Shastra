 
 

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
    [CreateAssetMenu(fileName = "Trigger Ability", menuName = "ScriptableObject/Combat/TriggerActionSO")]
    public class TriggerActionSO : ScriptableObject
    {
        [SerializeField]
        private ActorActionFactory triggerAction; 
        public ActorAction FactorizeRuntimeAction(CombatEntity caster)
        {
            return triggerAction.FactorizeRuntimeAction(caster) ;  // new TriggerActionAbilityRuntime(caster, this);
        }
    }

    //public class TriggerActionAbilityRuntime : ActorAction
    //{
    //    public TriggerActionAbilityRuntime(CombatEntity caster, ActorActionFactory factory) : base(factory, caster)
    //    {

    //    }

    //    public override IEnumerator PreActionPerform()
    //    {
    //        yield return null;
    //    }

    //    public override IEnumerator PostActionPerform()
    //    {
    //        yield return null;
    //    }

    //    public override IEnumerator Simulate(CombatEntity target)
    //    {
    //        yield return null;
    //    }


    //}


}
