
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.Eventing.Reader;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;

namespace Vanaring_DepaDemo
{
    public enum EEvokeKey
    {
        NOKEY,
        HAVOC,
    }


    [Serializable]
    public struct StatusStackInfo
    {
        [SerializeField]
        private string stackID ; 

        [SerializeField]
        private bool _stackable  ;
        [SerializeField] 
        private bool _overwriten ;

        public bool Stackable => _stackable  ;  
        public bool Overwrite => _overwriten ;

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
        [Header("Duration (turn unit) for this status effect")]
        protected int _TTL;

        [SerializeField]
        [Header("InfiniteTTL status wait until certain thing happens, if it happens, it reduce _TTL by one")] 
        protected bool _infiniteTTL ;

        [SerializeField]
        [Header("Evoke keys when user want to find specific status effects")]
        protected EEvokeKey _evokeKey = EEvokeKey.NOKEY ;

        [SerializeField]
        [Header("Status when applied multiple instance of same status effect")]
        protected StatusStackInfo _stackInfo   ;



        public StatusStackInfo StackInfo => _stackInfo ;
        public int TTL => _TTL;
        public bool InfiniteTTL => _infiniteTTL ;
        public EEvokeKey EvokeKey => _evokeKey;
        public string StatusDescription => _statusEffectDescription.FieldDescription;
        public string StatusName => _statusEffectDescription.FieldName;
        public Sprite StatusImage => _statusEffectDescription.FieldImage;

    }


    //All of the status effect should have "target" assigned to them 
    public abstract class StatusRuntimeEffect : RuntimeEffect
    {

        protected bool _infiniteTTL = false;
        //Turn base TTL
        protected int _timeToLive = 0;

        protected EEvokeKey _evokeKey;

        protected DescriptionBaseField _statusEffectDescription;

        protected StatusStackInfo _stackInfo;

        public StatusRuntimeEffect(StatusRuntimeEffectFactorySO effectFactory)
        {
            this._evokeKey = effectFactory.EvokeKey ; 
            this._infiniteTTL = effectFactory.InfiniteTTL ;
            this._timeToLive = effectFactory.TTL ;
            this._statusEffectDescription = new DescriptionBaseField(effectFactory.StatusName,
                effectFactory.StatusDescription, effectFactory.StatusImage);
            this._stackInfo = effectFactory.StackInfo;
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

        public virtual bool IsExpired()
        {
            return _timeToLive <= 0.0f;
        }

        public virtual void UpdateTTLCondition()
        {
            if (!_infiniteTTL)
                _timeToLive -= 1;
        }

        public bool IsCorrectEvokeKey(EEvokeKey evokeKey)
        {
            return (_evokeKey == evokeKey) ;
        }

        public virtual void OnStatusEffecExpire()
        {

        }

        #region GETTER
        public bool IsInfiniteTTL => _infiniteTTL;
        public int TimeToLive => _timeToLive;
        public StatusStackInfo StackInfo => _stackInfo;
        public DescriptionBaseField GetStatusEffectDescription()
        {
            return _statusEffectDescription;
        }
        #endregion
    }
}