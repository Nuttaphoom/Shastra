using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring
{
    public class LowHpConditionSO : BaseConditionSO
    {
        public override bool ConditionsMet(CombatEntity combatEntity, float conditionAmount)
        {
            float maxHp = combatEntity.CharacterSheet.GetHP;
            float currHp = combatEntity.StatsAccumulator.GetHPAmount();

            return (currHp * 100.0f / maxHp) <= conditionAmount;
        }
    }
}
