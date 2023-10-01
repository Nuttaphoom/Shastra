using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring
{
    public class AIBehaviorHandler : MonoBehaviour
    {
        AIEntity aiEntity;


        [Serializable]
        public struct SpellSocket
        {
            [SerializeField]
            public SpellActionSO spell;
            [SerializeField]
            public int change;
        }

        [Serializable]
        public struct ConditionData
        {
            [SerializeField]
            public BaseConditionSO Condition;
            [SerializeField]
            public int conditionAmount;
            [SerializeField]
            public List<SpellSocket> SpellList;
        }

        [SerializeField]
        List<ConditionData> _ProrityCondition = new List<ConditionData>();

        private int conditionIndex = 0;

        public void SetEntity(AIEntity entity)
        {
            aiEntity = entity;
        }

        public void CheckingCondition()
        {
            for (int i = 0; i < _ProrityCondition.Count; i++)
            {
                if (_ProrityCondition[i].Condition.ConditionsMet(aiEntity, _ProrityCondition[i].conditionAmount))
                {
                    conditionIndex = i;
                    break;
                }
            }
        }

        public IEnumerator GetNextAction()
        {
            int totalchange = 0;
            for (int i = 0; i < _ProrityCondition[conditionIndex].SpellList.Count; i++)
            {
                totalchange += _ProrityCondition[conditionIndex].SpellList[i].change;
            }

            int randomNum = UnityEngine.Random.Range(0, totalchange);

            for (int i = 0; i < _ProrityCondition[conditionIndex].SpellList.Count; i++)
            {
                if (randomNum > _ProrityCondition[conditionIndex].SpellList[i].change)
                {
                    randomNum -= _ProrityCondition[conditionIndex].SpellList[i].change;
                }
                else 
                {
                    yield return _ProrityCondition[conditionIndex].SpellList[i].spell;
                }
            }

            yield return null;
        }
        //public void SubOnDamageVisualEvent(UnityAction<int> argc)
        //{
        //    _OnUpdateVisualDMG += argc;
        //}

        //public void UnSubOnDamageVisualEvent(UnityAction<int> argc)
        //{
        //    _OnUpdateVisualDMG -= argc;
        //}

        //public void SubOnDamageVisualEventEnd(UnityAction<int> argc)
        //{
        //    _OnUpdateVisualDMGEnd += argc;
        //}

        //public void UnSubOnDamageVisualEventEnd(UnityAction<int> argc)
        //{
        //    _OnUpdateVisualDMGEnd -= argc;
        //}
    }
}
