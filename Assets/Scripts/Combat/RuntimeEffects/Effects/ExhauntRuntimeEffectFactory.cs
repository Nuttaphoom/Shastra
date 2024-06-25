using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.EventSystems.EventTrigger;


namespace Vanaring
{
    [CreateAssetMenu(fileName = "ExhauntRuntimeEffectFactory", menuName = "ScriptableObject/RuntimeEffect/ExhauntRuntimeEffectFactory")]
    public class ExhauntRuntimeEffectFactory : RuntimeEffectFactorySO
    {
        [SerializeField]
        private bool relieveExhaunt;

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            return SetUpRuntimeEffect(new ExhauntRuntimeEffect(relieveExhaunt), targets);
        }

    }

    public class ExhauntRuntimeEffect : RuntimeEffect
    {

        private bool relieveExhaunt; 

        public ExhauntRuntimeEffect(bool relieveExhaunt)
        {
            this.relieveExhaunt = relieveExhaunt; 
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            Debug.Log("start to relieve with target.count : " + _targets.Count);
            if (this.relieveExhaunt)
            {
                foreach (var target in _targets)
                {
                    target.SetExhaunst(! relieveExhaunt, true); 
                }
            }

            yield return null; 

        }










    }
}