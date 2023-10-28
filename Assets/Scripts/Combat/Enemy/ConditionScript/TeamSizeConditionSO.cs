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

        [SerializeField]
        private int _sizeAmout;

        /// <summary>
        ///  
        /// </summary>
        public override bool ConditionsMet(CombatEntity entity)
        {
            if (Current_MoreThan)
                return (CombatReferee.instance.GetCompetatorsBySide(_side).Count > _sizeAmout);

            return (CombatReferee.instance.GetCompetatorsBySide(_side).Count < _sizeAmout);

        }
    }
}
