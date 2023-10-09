using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring
{
    public abstract class BaseConditionSO : ScriptableObject
    {
        public abstract bool ConditionsMet(CombatEntity combatEntity, float conditionAmount);
    }
}
