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
        Dictionary<string, List<StatusRuntimeEffect>> _effects = new Dictionary<string, List<StatusRuntimeEffect>>(); 

        public StatusEffectHandler(CombatEntity appliedEntity)
        {
            this._appliedEntity = appliedEntity ;

        }

        //the effect should be factorize exactly before being applied 
        public IEnumerator ApplyNewEffect(StatusRuntimeEffectFactorySO factory  )
        {
            IEnumerator co = factory.Factorize(new List<CombatEntity>() { _appliedEntity } )  ;

            string key = factory.StackInfo.StackID() ;  
            while (co.MoveNext())
            {
                if (co.Current != null && co.Current.GetType().IsSubclassOf(typeof(RuntimeEffect) ))
                {
                    if (_effects.ContainsKey(key))
                    {
                        if (factory.StackInfo.Stackable)
                        {
                            _effects[key].Add(co.Current as StatusRuntimeEffect);

                        }
                        else if (factory.StackInfo.Overwrite)
                        {
                            while (_effects[key].Count > 0)
                                _effects[key].RemoveAt(0);
                            
                            _effects[key].Add(co.Current as StatusRuntimeEffect);

                        }
                    }
                    else
                    {
                        _effects.Add(key, new List<StatusRuntimeEffect>() );
                        Debug.Log("status runtime apply " + key);
                        _effects[key].Add(co.Current as StatusRuntimeEffect); 
                    }

                    //TODO - Visually 
                }
                yield return new WaitForEndOfFrame() ; 
            }
        }


        public IEnumerator ExecuteStatusRuntimeEffectCoroutine()
        {
            foreach (var key in _effects.Keys)
            {
                for (int i = 0; i < _effects[key].Count; i++)
                {
                    StatusRuntimeEffect statusEffect = _effects[key][i];

                    yield return statusEffect.ExecuteRuntimeCoroutine(_appliedEntity);
                    yield return statusEffect.OnExecuteRuntimeDone(_appliedEntity);

                    statusEffect.UpdateTTLCondition();

                    if (statusEffect.IsExpired())
                    {
                        //TODO - Remove status effect visually
                        _effects[key].RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }
        }

        public IEnumerator ExecuteAttackStatusRuntimeEffectCoroutine()
        {
            foreach (var key in _effects.Keys)
            {
                for (int i = 0; i < _effects[key].Count; i++)
                {
                    Debug.Log("call atk");
                    StatusRuntimeEffect statusEffect = _effects[key][i];

                    yield return statusEffect.BeforeAttackEffect(_appliedEntity);

                    if (statusEffect.IsExpired())
                    {
                        //TODO - Remove status effect visually
                        _effects[key].RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }

        }


    }
}
