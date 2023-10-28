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

        public override void SimulateEnergyModifier(CombatEntity target)
        {
            return; 
        }
    }

    public  class DebugLogRuntimeEffect : RuntimeEffect 
    {
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            Debug.Log("Debug Skill------------");
            Debug.Log("cast by " + caster.name);
            foreach (var target in _targets)
            {
                Debug.Log("target is " + target.name);
            }
            Debug.Log("End Debug Skill---------------"); 

            yield return null; 
        }
    }
}