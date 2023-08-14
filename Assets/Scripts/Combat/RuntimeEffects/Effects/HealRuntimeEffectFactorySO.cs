using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.EventSystems.EventTrigger;


namespace Vanaring_DepaDemo
{
    [CreateAssetMenu(fileName = "HealRuntimeEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/HealRuntimeEffectFactorySO")]
    public class HealRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        ActionAnimationInfo _actionAnimationInfo;

        [SerializeField]
        private int _hp = 0 ;
 

        public override IEnumerator Factorize(List<CombatEntity> targets)
        {
            HealRuntimeEffect retEffect = new HealRuntimeEffect(_actionAnimationInfo, _hp);
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            yield return retEffect;
        }
    }

    public class HealRuntimeEffect : RuntimeEffect
    {
        ActionAnimationInfo _actionAnimationInfo ; 
        private int _hp = 0; 
        public HealRuntimeEffect(ActionAnimationInfo actionAnimation , int hp )
        {
            _actionAnimationInfo = actionAnimation;
            _hp = hp; 
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            //Deal Dmg directly to enemy ignoring the caster 

            //Command caster to attack enemy 
            if (caster == null)
                throw new Exception("Caster can not be null");
 
  
            List<IEnumerator> coroutines = new List<IEnumerator>();

            //creating vfx for coroutine for targets
            foreach (CombatEntity target in _targets)
            {
                CombatEntity entity = target;
                entity.LogicHeal(_hp); 
                if (!_actionAnimationInfo.IsProjectile)
                {
                    coroutines.Add(entity.CombatEntityAnimationHandler.PlayVFXActionAnimation<string>(
                        _actionAnimationInfo.TargetVfxEntity, (string s) => entity.VisualHeal(s) ,
                        _actionAnimationInfo.TargetTrigerID)) ;
                }
                else
                {
                    coroutines.Add(caster.CombatEntityAnimationHandler.PlayVFXActionAnimation<string>(
                        _actionAnimationInfo.TargetVfxEntity, (string s) => entity.VisualHeal(s),
                        _actionAnimationInfo.TargetTrigerID, caster.gameObject.transform.position, entity.gameObject.transform.position));
                }
            }

            //create action animation coroutine for self
            coroutines.Add(caster.CombatEntityAnimationHandler.PlayActionAnimation(_actionAnimationInfo));

            yield return new WaitAll(caster, coroutines.ToArray());

        }










    }
}