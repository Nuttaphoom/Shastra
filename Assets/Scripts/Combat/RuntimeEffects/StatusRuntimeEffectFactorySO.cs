
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics.Eventing.Reader;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

namespace Vanaring_DepaDemo
{

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
        [Header("Duration (turn unit) for this status effect")]
        protected int _TTL;

        [SerializeField]
        [Header("InfiniteTTL status wait until certain thing happens")] 
        protected bool _infiniteTTL ;

        [SerializeField]
        [Header("Status when applied multiple instance of same status effect")]
        protected StatusStackInfo _stackInfo   ;

        public StatusStackInfo StackInfo => _stackInfo ; 

    }


    //All of the status effect should have "target" assigned to them 
    public abstract class StatusRuntimeEffect : RuntimeEffect
    {

        protected bool _infiniteTTL = false; 
        //Turn base TTL
        protected int _timeToLive = 0 ;

        public StatusRuntimeEffect(bool infiniteTTL, int ttl)
        {
            this._infiniteTTL = infiniteTTL ;
            this._timeToLive = ttl ;
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

        public virtual IEnumerator BeforeHurtEffect(CombatEntity caster)
        {
            yield return null;
        }

        public virtual bool IsExpired()
        {
            return _timeToLive <= 0.0f;
        }

        public virtual void UpdateTTLCondition()
        {
            if (! _infiniteTTL) 
                _timeToLive -= 1;
        }



    }

}