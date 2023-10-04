
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace Vanaring 
{

    [CreateAssetMenu(fileName = "StuntStatusEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/StatusEffect/StuntStatusEffectFactorySO")]
    public class StuntStatusEffectFactorySO : StatusRuntimeEffectFactorySO
    {
        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            StuntStatusEffect retEffect = new StuntStatusEffect(this  );
            foreach (CombatEntity target in targets)
            {
                 retEffect.AssignTarget(target);
            }

            return retEffect;
        }

        public override void SimulateEnergyModifier(CombatEntity target)
        {
            throw new System.NotImplementedException();
        }
    }


    public class StuntStatusEffect : StatusRuntimeEffect
    {
        public StuntStatusEffect(StatusRuntimeEffectFactorySO factory) : base(factory)   
        {

        }


        //_cgs can be null =, be careful not assuming he got _cgs 
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        { 
            foreach (CombatEntity target in _targets)
            {
                if (target is not IStatusEffectable)
                    throw new System.Exception("Assigned target is not IStatusEffectable");

                if (target is CombatEntity)
                {
                    target.StatsAccumulator.ApplyStunt(); 
                }

            }

            yield return null;
        }

        public override IEnumerator OnStatusEffecExpire(CombatEntity caster)
        {
            caster.GetComponent<EnergyOverflowHandler>().ResetOverflow(); 
            yield return caster.CombatEntityAnimationHandler.PlayTriggerAnimation("StuntRelieve");
        }


    } 


}