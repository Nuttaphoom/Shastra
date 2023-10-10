using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;


namespace Vanaring
{
    [CreateAssetMenu(fileName = "AnimationPlayerRuntimeEffectFactory", menuName = "ScriptableObject/RuntimeEffect/Utility/AnimationPlayerRuntimeEffectFactory")]
    public class AnimationPlayerRuntimeEffectFactory : RuntimeEffectFactorySO
    {

        [SerializeField]
        private string _triggerName;

        [SerializeField]
        private bool _triggerSelf;

        [SerializeField]
        private bool _triggerTarget ;

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            AnimationPlayerRuntimeEffect retEffect = new AnimationPlayerRuntimeEffect(_triggerName, _triggerSelf, _triggerTarget);
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
        private bool _triggerSelf; 
        private bool _triggerTarget; 
        public AnimationPlayerRuntimeEffect(string triggerName, bool triggerSelf, bool triggerTarget)
        {
            _triggerName = triggerName;
            _triggerTarget = triggerTarget;
            _triggerSelf = triggerSelf;
        }
        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            List<IEnumerator> coroutines = new List<IEnumerator>();

            if (_triggerSelf) 
                coroutines.Add(caster.CombatEntityAnimationHandler.PlayTriggerAnimation(_triggerName) ) ;
            if (_triggerTarget)
            {
                foreach (var target in _targets)
                {
                    coroutines.Add(target.CombatEntityAnimationHandler.PlayTriggerAnimation(_triggerName));
                }
            }
            yield return new WaitAll(caster, coroutines.ToArray());

        }
    }
}