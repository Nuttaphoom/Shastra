using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

namespace Vanaring_DepaDemo
{
    public interface IStatusEffectable
    {
        public StatusEffectHandler GetStatusEffectHandler() ;
    }

    [Serializable]
    public class StatusEffectHandler
    {
        [SerializeField]
        private CombatEntity _appliedEntity;
        Dictionary<string, List<StatusRuntimeEffect>> _effects = new Dictionary<string, List<StatusRuntimeEffect>>();

        //[SerializeField]
        //private StatusWindowManager _statusWindowManager;

        private UnityAction<Dictionary<string, List<StatusRuntimeEffect>>> _OnUpdateStatus;

        //private StatusWindowManager _statusWindowTopleft;

        public void UpdateStatusUI()
        {
            _OnUpdateStatus?.Invoke(_effects);
            //_statusWindowManager.ClearCurrentStatus();
            //foreach (KeyValuePair<string, List<StatusRuntimeEffect>> entry in _effects)
            //{
            //    Debug.Log(_appliedEntity.gameObject.name + " have Buff : " + entry.Value.Count);
            //}
            //_statusWindowManager.InstantiateStatusUI(_effects);
        }

        //the effect should be factorize exactly before being applied 
        private IEnumerator LogicApplyNewEffect(StatusRuntimeEffectFactorySO factory)
        {
            IEnumerator co = factory.Factorize(new List<CombatEntity>() { _appliedEntity });

            string key = factory.StackInfo.StackID();
            while (co.MoveNext())
            {
                if (co.Current != null && co.Current.GetType().IsSubclassOf(typeof(RuntimeEffect)))
                {
                    if (_effects.ContainsKey(key)) {
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

                        _effects.Add(key, new List<StatusRuntimeEffect>());
                        _effects[key].Add(co.Current as StatusRuntimeEffect);
                    }

                }
                yield return new WaitForEndOfFrame();
            }
        }

        public IEnumerator ApplyNewEffect(StatusRuntimeEffectFactorySO factory/*, ActionAnimationInfo actionAnimationInfo*/)
        {
            yield return LogicApplyNewEffect(factory);
            //create Status UI
            UpdateStatusUI();

        }


        #region ExecuteStatus Effects
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
                }
            }

        }

        public IEnumerator ExecuteStatusRuntimeEffectCoroutineWithEvokeKey(EEvokeKey evokeKey)
        {
            foreach (var key in _effects.Keys)
            {
                for (int i = 0; i < _effects[key].Count; i++)
                {
                    StatusRuntimeEffect statusEffect = _effects[key][i];

                    if (!statusEffect.IsCorrectEvokeKey(evokeKey))
                        break;

                    yield return statusEffect.ExecuteRuntimeCoroutine(_appliedEntity);
                    yield return statusEffect.OnExecuteRuntimeDone(_appliedEntity);

                    statusEffect.UpdateTTLCondition();

                }
            }


        }

        public IEnumerator ExecuteAttackStatusRuntimeEffectCoroutine()
        {
            foreach (var key in _effects.Keys)
            {
                for (int i = 0; i < _effects[key].Count; i++)
                {
                    StatusRuntimeEffect statusEffect = _effects[key][i];

                    yield return statusEffect.BeforeAttackEffect(_appliedEntity);
                }
            }


        }

        /// <summary>
        /// attacker can be null for direct dmg (no attacker) situation 
        /// mostly call with attacker side
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public IEnumerator ExecuteAfterAttackStatusRuntimeEffectCoroutine(CombatEntity subject)
        {
            foreach (var key in _effects.Keys)
            {
                for (int i = 0; i < _effects[key].Count; i++)
                {
                    StatusRuntimeEffect statusEffect = _effects[key][i];
                    yield return statusEffect.AfterAttackEffect(_appliedEntity, subject);
                }
            }
        }

        #endregion


        public IEnumerator RunStatusEffectExpiredScheme()
        {
            foreach (var key in _effects.Keys)
            {
                for (int i = 0; i < _effects[key].Count; i++)
                {
                    StatusRuntimeEffect statusEffect = _effects[key][i];

                    if (statusEffect.IsExpired())
                    {
                        //TODO - Remove status effect visually
                        yield return statusEffect.OnStatusEffecExpire(_appliedEntity);
                        _effects[key].RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }
            UpdateStatusUI();
        }

        public void SubOnStatusVisualEvent(UnityAction<Dictionary<string, List<StatusRuntimeEffect>>> argc)
        {
            _OnUpdateStatus += argc;
        }
        public void UnSubOnStatusVisualEvent(UnityAction<Dictionary<string, List<StatusRuntimeEffect>>> argc)
        {
            _OnUpdateStatus -= argc;
        }

        #region GETTER
        public List<StatusRuntimeEffect> GetStatusRuntimeEffectWithEvokeKey(EEvokeKey evokeKey, bool updateTTLAfterGet = true)
        {
            List<StatusRuntimeEffect> ret = new List<StatusRuntimeEffect>();

            foreach (var key in _effects.Keys)
            {
                for (int i = 0; i < _effects[key].Count; i++)
                {
                    StatusRuntimeEffect statusEffect = _effects[key][i];
                    if (statusEffect.IsCorrectEvokeKey(evokeKey))
                    {
                        ret.Add(statusEffect);
                    }

                    if (updateTTLAfterGet)
                        statusEffect.UpdateTTLCondition();

                }
            }

            
            return ret;
        }

        #endregion


    }
}
