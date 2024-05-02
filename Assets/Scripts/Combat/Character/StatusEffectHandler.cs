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
using Unity.VisualScripting;

namespace Vanaring 
{
    [Serializable]
    public class StatusEffectHandler
    {
        #region EventBroadcaster Methods 
        private EventBroadcaster _eventBroadcaster;
        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();

                _eventBroadcaster.OpenChannel<EntityStatusEffectPair>("OnStatusEffectApplied");
                _eventBroadcaster.OpenChannel<StatusRuntimeEffect>("OnStatusEffectExpired");
            }

            return _eventBroadcaster;
        }
        public void SubOnStatusEffectApplied(UnityAction<EntityStatusEffectPair> func)
        {
            GetEventBroadcaster().SubEvent(func,"OnStatusEffectApplied");
        }

        public void SubOnStatusEffectExpired(UnityAction<StatusRuntimeEffect> func)
        {
            GetEventBroadcaster().SubEvent(func, "OnStatusEffectExpired");
        }

        public void UnSubOnStatusEffectExpired(UnityAction<StatusRuntimeEffect> func)
        {
            GetEventBroadcaster().UnSubEvent(func, "OnStatusEffectExpired");
        }

        public void UnSubOnStatusEffectApplied(UnityAction<EntityStatusEffectPair> func)
        {
            GetEventBroadcaster().UnSubEvent(func, "OnStatusEffectApplied");
        }
        #endregion
     
        private CombatEntity _appliedEntity;

        Dictionary<string, List<StatusRuntimeEffect>> _effects = new Dictionary<string, List<StatusRuntimeEffect>>();

        public Dictionary<string, List<StatusRuntimeEffect>> Effects => _effects;

        //private UnityAction<Dictionary<string, List<StatusRuntimeEffect>>> _OnUpdateStatus;

        public StatusEffectHandler(CombatEntity entity)
        {
            _appliedEntity = entity;
        }

        public void UpdateStatusUI()
        {
            //_OnUpdateStatus?.Invoke(_effects);
            
            //_statusWindowManager.ClearCurrentStatus();
            //foreach (KeyValuePair<string, List<StatusRuntimeEffect>> entry in _effects)
            //{
            //    Debug.Log(_appliedEntity.gameObject.name + " have Buff : " + entry.Value.Count);
            //}
            //_statusWindowManager.InstantiateStatusUI(_effects);
        }

        //

        /// <summary>
        /// the effect should be factorize exactly before being applied 
        ///  
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        private IEnumerator LogicApplyNewEffect(StatusRuntimeEffect effect, StatusRuntimeEffectFactorySO factory, CombatEntity applier)
        {

            string key = factory.Property.StackID();

            if (!_effects.ContainsKey(key))
            {
                _effects.Add(key, new List<StatusRuntimeEffect>());

            }
                
            if (_effects[key].Count == 0)
            {
                _effects[key].Add(effect);
                
                yield return effect.OnStatusEffectApplied(applier);   
            }
            
            else if (factory.Property.Stackable)
            {
                _effects[key].Add(effect); 
                
                yield return effect.OnStatusEffectApplied(applier);   
            }
            
            else if (factory.Property.Overwrite) 
            {
                while (_effects[key].Count > 0)   
                    _effects[key].RemoveAt(0);

                _effects[key].Add(effect);                
                yield return effect.OnStatusEffectApplied(applier);
            }

             

        }

        public IEnumerator ApplyNewEffect(StatusRuntimeEffectFactorySO statusEffectFactory, StatusEffectApplierRuntimeEffect applierFactory, CombatEntity applier)
        {
            StatusRuntimeEffect runtimeEffect = statusEffectFactory.Factorize(new List<CombatEntity>() { _appliedEntity }) as StatusRuntimeEffect;

            yield return LogicApplyNewEffect(runtimeEffect, statusEffectFactory, applier);

            GetEventBroadcaster().InvokeEvent(new EntityStatusEffectPair()
            {
                Actor = applier,
                StatusEffectFactory = statusEffectFactory, 
                ApplierFactory = applierFactory,
                StatusRuntime = runtimeEffect 
            }, "OnStatusEffectApplied") ;

            //create Status UI
            UpdateStatusUI();

            yield return null; 

        }


        #region ExecuteStatus Effects
        public IEnumerator ExecuteStatusRuntimeEffectCoroutine()
        {
            if (_effects == null)
                _effects = new Dictionary<string, List<StatusRuntimeEffect>>(); 

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

                        if (_effects[key].Count == 0)
                            GetEventBroadcaster().InvokeEvent(statusEffect , "OnStatusEffectExpired");

                        continue;
                    }
                }
            }
            UpdateStatusUI();
        }

        ////TODO : Remove these two functions 
        //public void SubOnStatusVisualEvent(UnityAction<Dictionary<string, List<StatusRuntimeEffect>>> argc)
        //{
        //    _OnUpdateStatus += argc;
        //}
        //public void UnSubOnStatusVisualEvent(UnityAction<Dictionary<string, List<StatusRuntimeEffect>>> argc)
        //{
        //    _OnUpdateStatus -= argc;
        //}


        #region EVENTListener 

        /// <summary>
        /// Listen to "OnStun", remove any effect that have Breakable when overflow
        /// </summary>
        public void StunBreakStatusEffect(CombatEntity entity)
        {
            foreach (var key in _effects.Keys)
            {
                if (_effects[key].Count == 0)
                    continue;

                if (! _effects[key][0].IsBreakWhenStun())
                    continue; 

                
                for (int i = 0; i < _effects[key].Count; i++)
                    _effects[key][i].ForceExpire();

                Debug.Log("Break " + key); 

            }
        }

        #endregion
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
