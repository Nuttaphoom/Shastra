using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;


namespace Vanaring 
{
    public abstract class RuntimeEffectFactorySO : ScriptableObject
    {
        public virtual void SimulateEnergyModifier(CombatEntity target)
        {
            target.SpellCaster.Simulate(RuntimeMangicalEnergy.EnergySide.LightEnergy, 0);
        }
        public abstract RuntimeEffect Factorize( List<CombatEntity> targets)  ;

        protected RuntimeEffect SetUpRuntimeEffect(RuntimeEffect effect,List<CombatEntity> targets)
        {
            AssignTarget(effect, targets);
            return effect; 
        }

        protected void AssignTarget(RuntimeEffect effect, List<CombatEntity> targets)
        {
            if (effect == null)
                throw new NullReferenceException();

            if (targets == null || targets.Count == 0)
                return; 

            foreach (CombatEntity target in targets)
            {
                effect.AssignTarget(target);
            }

        }
    }

    public abstract class RuntimeEffect 
    {
        protected List<CombatEntity> _targets = new List<CombatEntity>();
        public virtual IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            yield return null; 
        }
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