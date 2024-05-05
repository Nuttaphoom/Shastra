
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.Eventing.Reader;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace Vanaring 
{
    public enum EEvokeKey
    {
        NOKEY,
        HAVOC,
    }


    [Serializable]
    public struct StatusEffectProperty
    {
        [SerializeField]
        private string stackID ; 

        [SerializeField]
        private bool _stackable  ;

        [SerializeField]
        private bool _overwriten ;

        [SerializeField]
        private bool _overflowBreak;

        [SerializeField]
        [Header("InfiniteTTL status wait until certain thing happens, if it happens, it reduce _TTL by one")]
        private bool _infiniteTTL;

        [SerializeField]
        [Header("Duration (turn unit) for this status effect")]
        private int _TTL;

        public bool Stackable => _stackable  ;  
        public bool Overwrite => _overwriten ;
        public bool OverflowBreak => _overflowBreak ;
        public int TimeToLive => _TTL;
        public bool InfiniteTTL => _infiniteTTL ; 

        public bool IsSameStatus(string id) {
            return (stackID == id) ; 
        } 

        public string StackID()
        {
            if (stackID == "")
                throw new Exception("StackID is null"); 
            
            return stackID; 
        }
    }
    public abstract class StatusRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        private DescriptionBaseField _statusEffectDescription ;

        [SerializeField]
        [Header("Evoke keys when user want to find specific status effects")]
        protected EEvokeKey _evokeKey = EEvokeKey.NOKEY ;

        [SerializeField]
        [Header("Status when applied multiple instance of same status effect")]
        protected StatusEffectProperty _property;

        public StatusEffectProperty Property => _property ;
        public EEvokeKey EvokeKey => _evokeKey;
        public string StatusDescription => _statusEffectDescription.FieldDescription;
        public string StatusName => _statusEffectDescription.FieldName;
        public Sprite StatusImage => _statusEffectDescription.FieldImage; 
        
    }

    //All of the status effect should have "target" assigned to them 
    public abstract class StatusRuntimeEffect : RuntimeEffect
    {
        #region Events
        private EventBroadcaster _eventBroadcaster; 

        private EventBroadcaster EventBroadcaster
        {
            get
            {
                if (_eventBroadcaster == null)
                {
                    _eventBroadcaster = new EventBroadcaster();
                    _eventBroadcaster.OpenChannel<int>("OnTTLUpdate");
                    _eventBroadcaster.OpenChannel<bool>("OnStatusEffectExpire");
                    _eventBroadcaster.OpenChannel<bool>("OnStatusEffectBreak");
                }
                return _eventBroadcaster;
            }
        }

        public void SubOnTTLUpdate(UnityAction<int> func)
        {
            EventBroadcaster.SubEvent<int>(func, "OnTTLUpdate"); 
        }
        public void UnSubOnTTLUpdate(UnityAction<int> func)
        {
            EventBroadcaster.UnSubEvent<int>(func, "OnTTLUpdate");
        }
        public void SubOnStatusEffectExpire(UnityAction<bool> func)
        {
            EventBroadcaster.SubEvent<bool>(func, "OnStatusEffectExpire");
        }
        public void UnSubOnStatusEffectExpire(UnityAction<bool> func)
        {
            EventBroadcaster.UnSubEvent<bool>(func, "OnStatusEffectExpire");
        }
        public void SubOnStatusEffectBreak(UnityAction<bool> func)
        {
            EventBroadcaster.SubEvent<bool>(func, "OnStatusEffectBreak");
        }
        public void UnSubOnStatusEffectBreak(UnityAction<bool> func)
        {
            EventBroadcaster.UnSubEvent<bool>(func, "OnStatusEffectBreak");
        }

        #endregion

        //Turn base TTL
        protected int _timeToLive = 0;

        protected EEvokeKey _evokeKey;

        protected DescriptionBaseField _statusEffectDescription;

        protected StatusEffectProperty _property ; 
        
        public StatusRuntimeEffect(StatusRuntimeEffectFactorySO effectFactory)
        {
            this._property = effectFactory.Property ;
            this._evokeKey = effectFactory.EvokeKey ; 
            this._timeToLive = _property.TimeToLive  ;
            this._statusEffectDescription = new DescriptionBaseField(effectFactory.StatusName,
            effectFactory.StatusDescription, effectFactory.StatusImage);
        }

        #region Event Effect
        public virtual IEnumerator OnStatusEffectApplied(CombatEntity applier)
        {
            Debug.Log("on status effect applied in parent");
            yield return null; 
        }

        /// <summary>
        /// Occurs when the owning entity is overflow 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator OnStatusEffectBreakWithOverflow()
        {
            yield return null; 
        }

        /// <summary>
        /// Called before "start" attack scheme (to get benefit from the effect) 
        /// </summary>
        /// <param name="caster"></param>
        /// <returns></returns>
        public virtual IEnumerator BeforeAttackEffect(CombatEntity caster)
        {
            yield return null;
        }

        /// <summary>
        /// attacker = CombatEntity who attacks
        /// subject = CombatEntity who is attacked
        /// </summary>
        /// <param name="caster"></param>
        /// <returns></returns>
        public virtual IEnumerator AfterAttackEffect(CombatEntity attacker, CombatEntity subject)
        {
            yield return null;
        }

        #endregion

        public bool IsExpired()
        {
            Debug.Log("Check  TTL  is expired : " + _timeToLive); 

            EventBroadcaster.InvokeEvent<bool>(_timeToLive <= 0, "OnStatusEffectExpire") ; 
            return _timeToLive <= 0.0f;
        }

        public bool IsBreakWhenStun()
        {
            EventBroadcaster.InvokeEvent<bool>(_property.OverflowBreak, "OnStatusEffectBreak") ;
            return _property.OverflowBreak; 
        }

        public void ForceExpire()
        {
            EventBroadcaster.InvokeEvent<bool>(true, "OnStatusEffectExpire");
            _timeToLive = 0; 
        }
        public void UpdateTTLCondition()
        {
            Debug.Log("Update TTL prev : " + _timeToLive); 

            if (! _property.InfiniteTTL)
                _timeToLive -= 1;


            EventBroadcaster.InvokeEvent<int>(_timeToLive, "OnTTLUpdate");

            
        }
        public bool IsCorrectEvokeKey(EEvokeKey evokeKey)
        {
            return (_evokeKey == evokeKey) ;
        }

        /// <summary>
        /// applier is NOT the applied entity
        /// </summary>
        /// <param name="applier"></param>
        /// <returns></returns>
        public virtual IEnumerator OnStatusEffecExpire(CombatEntity applier) 
        {
            yield return null; 
        }

        #region GETTER
        public bool IsInfiniteTTL => _property.InfiniteTTL;
        public int TimeToLive => _timeToLive;
        public StatusEffectProperty Property => _property;
        public DescriptionBaseField GetStatusEffectDescription()
        {
            return _statusEffectDescription;
        }
        #endregion
    }
}