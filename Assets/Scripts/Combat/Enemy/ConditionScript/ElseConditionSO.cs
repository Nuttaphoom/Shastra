using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring
{
    [CreateAssetMenu( fileName = "ElseCondition", menuName = "ScriptableObject/Combat/Enemy/ConditionSO/Else")]
    public class ElseConditionSO : BaseConditionSO
    {
        public override bool ConditionsMet(CombatEntity aiEntity)
        {
            return true;
        }
    }
}
