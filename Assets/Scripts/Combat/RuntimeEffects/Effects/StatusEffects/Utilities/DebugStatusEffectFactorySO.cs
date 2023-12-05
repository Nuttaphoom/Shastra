
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace Vanaring 
{

    [CreateAssetMenu(fileName = "DebugStatusEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/DebugStatusEffectFactory/DebugStatusEffectFactorySO")]
    public class DebugStatusEffectFactorySO : StatusRuntimeEffectFactorySO
    {
        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            DebugStatusEffectApplier retEffect = new DebugStatusEffectApplier(this);
            foreach (CombatEntity target in targets)
                retEffect.AssignTarget(target);

            return retEffect;
        }

 
    }


    public class DebugStatusEffectApplier : StatusRuntimeEffect
    {
        public DebugStatusEffectApplier(StatusRuntimeEffectFactorySO factory) : base(factory)
        {
 
        }


        //_cgs can be null =, be careful not assuming he got _cgs 
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {

            List<CombatEntity> aTargetForNewStatus = new List<CombatEntity>();
            foreach (CombatEntity target in _targets)
            {
                if (target is CombatEntity)
                {
                    Debug.Log("Status debug on " + target.name) ; 
                }

            }

            yield return null;
        }
    }


}