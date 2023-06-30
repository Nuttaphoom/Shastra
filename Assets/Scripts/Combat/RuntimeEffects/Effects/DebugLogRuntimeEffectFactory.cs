using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace Vanaring_DepaDemo
{
    [CreateAssetMenu(fileName = "DebugLogRuntimeEffectFactory", menuName = "ScriptableObject/RuntimeEffectFactory/Utility/DebugLogRuntimeEffectFactory")]
    public class DebugLogRuntimeEffectFactory : RuntimeEffectFactorySO
    {
        public override IEnumerator Factorize( List<CombatEntity> targets)
        {
            RuntimeEffect retEffect = new DebugLogRuntimeEffect();
            foreach (CombatEntity target in targets) 
                retEffect.AssignTarget(target) ;

            yield return retEffect ;
        }
    }

    public  class DebugLogRuntimeEffect : RuntimeEffect 
    {
        

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            Debug.Log("cast by " + caster.name);
            foreach (var target in _targets)
            {
                Debug.Log(target.name); 
            }
            yield return null; 
        }

        public override IEnumerator OnExecuteRuntimeDone(CombatEntity caster)
        {
            yield return null;
        }

        



    }
}