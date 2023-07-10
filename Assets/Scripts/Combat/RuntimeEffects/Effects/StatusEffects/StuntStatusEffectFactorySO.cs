
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace Vanaring_DepaDemo
{

    [CreateAssetMenu(fileName = "StuntStatusEffectFactorySO", menuName = "ScriptableObject/RuntimeFactory/StatusEffect/StuntStatusEffectFactorySO")]
    public class StuntStatusEffectFactorySO : StatusRuntimeEffectFactorySO
    {

        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            StuntStatusEffect retEffect = new StuntStatusEffect(_TTL);
            foreach (CombatEntity target in targets)
            {
                 retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }


    public class StuntStatusEffect : StatusRuntimeEffect
    {
        public StuntStatusEffect(int TTL)
        {
            this._timeToLive = TTL;
        }


        //_cgs can be null =, be careful not assuming he got _cgs 
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {

            List<CombatEntity> aTargetForNewStatus = new List<CombatEntity>();
            foreach (CombatEntity target in _targets)
            {
                if (target is not IStatusEffectable)
                    throw new System.Exception("Assigned target is not IStatusEffectable");

                if (target is CombatEntity)
                {
                    Debug.Log("apply stunt to " + target.name);
                    target.StatsAccumulator.ApplyStunt(); 
                }

            }

            yield return null;
        }
    }


}