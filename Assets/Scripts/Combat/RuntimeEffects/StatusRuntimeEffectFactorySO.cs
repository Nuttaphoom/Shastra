
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.Eventing.Reader;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;

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
        private Comment _comment_on_applied;
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
        public Comment GetCommentOnApplied => _comment_on_applied; 
        public override void SimulateEnergyModifier(CombatEntity target)
        {

        }
    }


    //All of the status effect should have "target" assigned to them 
    public abstract class StatusRuntimeEffect : RuntimeEffect
    {
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
            return _timeToLive <= 0.0f;
        }

        public bool IsBreakWhenStun()
        {
            return _property.OverflowBreak; 
        }

        public void ForceExpire()
        {
            _timeToLive = 0; 
        }
        public void UpdateTTLCondition()
        {
            if (! _property.InfiniteTTL)
                _timeToLive -= 1;
        }
        public bool IsCorrectEvokeKey(EEvokeKey evokeKey)
        {
            return (_evokeKey == evokeKey) ;
        }

        public virtual IEnumerator OnStatusEffecExpire(CombatEntity caster) 
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