using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace Vanaring_DepaDemo
{
    [CreateAssetMenu(fileName = "EvokeHavocRuntimeEffectFactory", menuName = "ScriptableObject/RuntimeEffect/EvokeHavocRuntimeEffectFactory")]
    public class EvokeHavocRuntimeEffectFactory : RuntimeEffectFactorySO
    {
        [SerializeField]
        [Header("Deal BaseMultiplier ^ n dmg to the target")]
        private int _baseMultiplier; 
        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            EvokeHavocRuntimeEffect retEffect = new EvokeHavocRuntimeEffect(_baseMultiplier);
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }

    public class EvokeHavocRuntimeEffect : RuntimeEffect
    {
        private int _baseMultiplier;
        public EvokeHavocRuntimeEffect(int baseMultiplier)
        {
            _baseMultiplier = baseMultiplier; 
        }
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            foreach (CombatEntity entity in _targets) {
                List<StatusRuntimeEffect> havoc = entity.GetStatusEffectHandler().GetStatusRuntimeEffectWithEvokeKey(EEvokeKey.HAVOC, true);
                double realDmg = Math.Pow(_baseMultiplier, havoc.Count) ;

                 
            }

            yield return null;
        }
    }
}