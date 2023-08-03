
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace Vanaring_DepaDemo
{

    [CreateAssetMenu(fileName = "WaitForActionStatusEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/StatusEffect/WaitForActionStatusEffectFactorySO")]
    public class WaitForActionStatusEffectFactorySO : StatusRuntimeEffectFactorySO
    {
        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            WaitForActionStatusEffect retEffect = new WaitForActionStatusEffect(this);
            foreach (CombatEntity target in targets)
            {
                 retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }


    public class WaitForActionStatusEffect : StatusRuntimeEffect
    {
        public WaitForActionStatusEffect(StatusRuntimeEffectFactorySO factory) : base(factory)   
        {

        }

        //_cgs can be null =, be careful not assuming he got _cgs 
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        { 
            yield return null;
        }

    } 


}