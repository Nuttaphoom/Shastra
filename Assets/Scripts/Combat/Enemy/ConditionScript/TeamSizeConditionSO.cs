using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "TeamSizeConditionSO", menuName = "ScriptableObject/Combat/Enemy/ConditionSO/TeamSizeConditionSO")]
    public class TeamSizeConditionSO : BaseConditionSO
    {
        [SerializeField]
        private ECompetatorSide _side;

        [SerializeField]
        private bool Current_MoreThan = false ;

        /// <summary>
        ///  
        /// </summary>
        public override bool ConditionsMet(AIEntity aiEntity, float conditionAmount)
        {
            if (Current_MoreThan)
                return (CombatReferee.instance.GetCompetatorsBySide(_side).Count > conditionAmount);    

            return (CombatReferee.instance.GetCompetatorsBySide(_side).Count < conditionAmount);
        }
    }
}
