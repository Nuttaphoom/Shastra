using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public abstract class BackpackActionFactorySO : ScriptableObject
    {
        public abstract BackpackItemAbilityRuntime FactorizeBackpackItemAbilityRuntime(RuntimePartyMember caster, List<RuntimePartyMember> targets) ; 
    }

    /// <summary>
    /// use for item that can be used in Backpack menu (on mission) 
    /// </summary>
    public abstract class BackpackItemAbilityRuntime
    {
        protected RuntimePartyMember caster; 
        protected List<RuntimePartyMember> targets;

        TargetSelector _targetSelector;

        public BackpackItemAbilityRuntime(RuntimePartyMember caster, BackpackActionFactorySO factory, List<RuntimePartyMember> targets)
        {
            this.caster = caster;
            this.targets = targets;  
        }

        public virtual IEnumerator UseItemAbilityOutsideCombat()
        {
            yield return null;
        }

    }
}
