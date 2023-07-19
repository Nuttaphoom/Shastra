using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace Vanaring_DepaDemo
{
    public abstract class RuntimeEffectFactorySO : ScriptableObject
    {
        [SerializeField]
        private TargetSelector _targetSelector ;
        

        
        public TargetSelector TargetSelect => _targetSelector;
        public abstract IEnumerator Factorize( List<CombatEntity> targets)  ;
    }

    public abstract class RuntimeEffect 
    {
        protected List<CombatEntity> _targets = new List<CombatEntity>();
 
        public abstract IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster);

        public virtual IEnumerator OnExecuteRuntimeDone(CombatEntity caster)
        {
            yield return null;
        }

        public void AssignTarget(CombatEntity target)
        {
            _targets.Add(target);
        }

    }
}