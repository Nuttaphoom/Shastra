using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace Vanaring 
{
    [CreateAssetMenu(fileName = "DebugLogRuntimeEffectFactory", menuName = "ScriptableObject/RuntimeEffect/Utility/DebugLogRuntimeEffectFactory")]
    public class DebugLogRuntimeEffectFactory : RuntimeEffectFactorySO
    {
        public override RuntimeEffect Factorize( List<CombatEntity> targets)
        {
            RuntimeEffect retEffect = new DebugLogRuntimeEffect();
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            return retEffect ;
        }

    
    }

    public  class DebugLogRuntimeEffect : RuntimeEffect 
    {
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
   
             
          

            yield return null; 
        }
    }
}