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
        private CombatEntity _appliedEntity ;
        List<StatusRuntimeEffect> _currentEffects = new List<StatusRuntimeEffect>();

        public StatusEffectHandler(CombatEntity appliedEntity)
        {
            this._appliedEntity = appliedEntity ;   
        }

        //the effect should be factorize exactly before being applied 
        public IEnumerator ApplyNewEffect(StatusRuntimeEffectFactorySO factory  )
        {
            Debug.Log("" + _appliedEntity.gameObject.name + " is applied status effect : " + _appliedEntity.name);

            IEnumerator co = factory.Factorize(new List<CombatEntity>() { _appliedEntity } )  ;

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

                yield return statusEffect.ExecuteRuntimeCoroutine(_appliedEntity);
                yield return statusEffect.OnExecuteRuntimeDone(_appliedEntity) ;

                statusEffect.UpdateTTLCondition();

                if (statusEffect.IsExpired())
                {
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

                yield return statusEffect.BeforeAttackEffect(_appliedEntity);

            }

        }


    }
}
