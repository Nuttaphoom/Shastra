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
    [CreateAssetMenu(fileName = "AnimationPlayerRuntimeEffectFactory", menuName = "ScriptableObject/RuntimeEffect/Utility/AnimationPlayerRuntimeEffectFactory")]
    public class AnimationPlayerRuntimeEffectFactory : RuntimeEffectFactorySO
    {

        [SerializeField]
        private string _triggerName;

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            AnimationPlayerRuntimeEffect retEffect = new AnimationPlayerRuntimeEffect(_triggerName);
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            return retEffect;
        }

        public override void SimulateEnergyModifier(CombatEntity target)
        {
            //throw new NotImplementedException();
        }
    }

    public class AnimationPlayerRuntimeEffect : RuntimeEffect
    {
        private string _triggerName; 
        public AnimationPlayerRuntimeEffect(string triggerName)
        {
            _triggerName = triggerName; 
        }
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            yield return caster.CombatEntityAnimationHandler.PlayTriggerAnimation(_triggerName) ;
        }
    }
}