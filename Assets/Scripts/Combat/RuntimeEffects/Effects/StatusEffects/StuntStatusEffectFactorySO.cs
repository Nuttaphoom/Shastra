
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

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity owner)
        {
            //foreach (CombatEntity target in _targets)
            //{
            //    if (target is CombatEntity)
            //        target.ApplyStun( ) ;
            //}

            yield return null;
        }
        

        public override IEnumerator OnStatusEffecExpire(CombatEntity caster)
        {
            Debug.Log("status effect expired");
            yield return caster.GetComponent<EnergyOverflowHandler>().ResetOverflow(); 
        }


    } 


}