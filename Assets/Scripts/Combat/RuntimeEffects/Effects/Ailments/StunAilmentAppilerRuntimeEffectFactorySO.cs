
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace Vanaring
{

    [CreateAssetMenu(fileName = "StunAilmentAppilerRuntimeEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/AilmentApplier/StunAilmentAppilerRuntimeEffectFactorySO")]
    public class StunAilmentAppilerRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            StunAilmentAppilerRuntimeEffect retEffect = new StunAilmentAppilerRuntimeEffect( );
            foreach (CombatEntity target in targets)
            {
                retEffect.AssignTarget(target);
            }

            return retEffect;
        }

        public override void SimulateEnergyModifier(CombatEntity target)
        {

        }
    }


    public class StunAilmentAppilerRuntimeEffect : RuntimeEffect
    {
        public StunAilmentAppilerRuntimeEffect(   )  
        {

        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity owner)
        {
            foreach (CombatEntity target in _targets)
            {
                if (target is CombatEntity)
                    target.ApplyStun();
            }

            yield return null;
        }


         


    }


}