using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring
{
    public class ElseConditionSO : BaseConditionSO
    {
        public override bool ConditionsMet(CombatEntity combatEntity, float conditionAmount)
        {
            return true;
        }
    }
}
