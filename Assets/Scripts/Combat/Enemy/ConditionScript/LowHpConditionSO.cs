using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring
{
    [CreateAssetMenu( fileName = "LowHpCondition", menuName = "ScriptableObject/Combat/Enemy/ConditionSO/LowHp")]
    public class LowHpConditionSO : BaseConditionSO
    {
        /// <summary>
        /// LowHp by max 100% the input should be 0-100% when it lower or equal to that amount
        /// </summary>
        public override bool ConditionsMet(AIEntity aiEntity, int conditionAmount)
        {
            int maxHp = aiEntity.CharacterSheet.GetHP;
            int currHp = aiEntity.StatsAccumulator.GetHPAmount();

            return (currHp * 100.0f / maxHp) <= conditionAmount;
        }
    }
}
