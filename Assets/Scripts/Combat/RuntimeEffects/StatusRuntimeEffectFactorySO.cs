
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Vanaring_DepaDemo
{

    public abstract class StatusRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        [Header("Duration (turn unit) for this status effect")]
        protected int _TTL;
    }


    //All of the status effect should have "target" assigned to them 
    public abstract class StatusRuntimeEffect : RuntimeEffect
    {
        //Turn base TTL
        protected float _timeToLive = 0.0f;

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
            _timeToLive -= 1;
        }



    }

}