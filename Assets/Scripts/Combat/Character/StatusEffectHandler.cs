using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public interface IStatusEffectable
    {
        public StatusEffectHandler GetStatusEffectHandler(); 
    }

    public class StatusEffectHandler
    {
        private CombatEntity caster;
        List<StatusRuntimeEffect> _currentEffects = new List<StatusRuntimeEffect>();

        public StatusEffectHandler(CombatEntity caster)
        {
            this.caster = caster;   
        }

        //the effect should be factorize exactly before being applied 
        public IEnumerator ApplyNewEffect(StatusRuntimeEffectFactorySO factory, List<CombatEntity> targets )
        {
            IEnumerator  co =  factory.Factorize( targets) ; 
            while (co.MoveNext())
            {
                if (co.Current != null && co.Current.GetType().IsSubclassOf(typeof(RuntimeEffect) ))
                {
                    //TODO - Visually 
                    _currentEffects.Add(co.Current as StatusRuntimeEffect) ;
                }
                yield return new WaitForEndOfFrame() ; 
            }
        }


        public IEnumerator ExecuteStatusRuntimeEffectCoroutine()
        {
            for (int i = 0; i < _currentEffects.Count; i++)
            {
                StatusRuntimeEffect statusEffect = _currentEffects[i];

                yield return statusEffect.ExecuteRuntimeCoroutine(caster);

                statusEffect.UpdateTTLCondition();

                if (statusEffect.IsExpired())
                {
                    Debug.Log("effect is expired");
                    //TODO - Remove status effect visually
                    _currentEffects.RemoveAt(i);
                    i--;
                    continue;
                }
            }
        }

        public IEnumerator ExecuteAttackStatusRuntimeEffectCoroutine()
        {
            for (int i = 0; i < _currentEffects.Count; i++)
            {
                StatusRuntimeEffect statusEffect = _currentEffects[i];

                yield return statusEffect.BeforeAttackEffect(caster);

            }

        }


    }
}
