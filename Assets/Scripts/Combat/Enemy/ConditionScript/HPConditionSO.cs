using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring
{
    [CreateAssetMenu( fileName = "LowHpCondition", menuName = "ScriptableObject/Combat/Enemy/ConditionSO/LowHp")]
    public class HPConditionSO : BaseConditionSO
    {
        /// <summary>
        /// LowHp by max 100% the input should be 0-100% when it lower or equal to that amount
        /// </summary>


        [Range(0, 100)]
        [SerializeField]
        private float _lowerThan_Percent = 100.0f;

        [Range(0,100)]
        [SerializeField]
        private float _moreThan_Percent = 0.0f ;

        public override bool ConditionsMet(CombatEntity aiEntity)
        {
            float maxHP = aiEntity.CharacterSheet.GetHP;
            float currHP = aiEntity.StatsAccumulator.GetHPAmount();

            float result = (currHP * 100.0f / maxHP);
            Debug.Log("result is " + result + " <= " + _lowerThan_Percent + " >= " + _moreThan_Percent) ; 
            return result <= _lowerThan_Percent && result >= _moreThan_Percent ; 
        }
    }
}
