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
        public enum SpellOrderType {
            Random = 0,
            Order
        }

        [Serializable]
        public struct SpellSocket
        {
            [SerializeField]
            public SpellActionSO spell;
            [SerializeField]
            public int chance;
        }

        [Serializable]
        public struct ConditionData
        {
            [SerializeField]
            public SpellOrderType spellOrder;
            [SerializeField]
            public BaseConditionSO Condition;
            [SerializeField]
            public float conditionAmount;
            [SerializeField]
            public List<SpellSocket> SpellList;
        }

        [SerializeField]
        List<ConditionData> _ProrityCondition = new List<ConditionData>();

        private int conditionIndex = 0;
        private int currentspellIndex = 0; //only for spell in order

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
                    return;
                }
            }
            Debug.LogError("Error : No ConditionsMet");
        }

        public void GetNextAction()
        {
            UseSpell(_ProrityCondition[conditionIndex].spellOrder);
        }

        private void UseSpell(SpellOrderType type)
        {
            if (type == SpellOrderType.Random)
            {
                int totalchance = 0;
                for (int i = 0; i < _ProrityCondition[conditionIndex].SpellList.Count; i++)
                {
                    totalchance += _ProrityCondition[conditionIndex].SpellList[i].chance;
                }
                if (totalchance != 100)
                {
                    Debug.LogError("totalchance should be equal to 100");
                }

                int randomNum = UnityEngine.Random.Range(0, totalchance);

                for (int i = 0; i < _ProrityCondition[conditionIndex].SpellList.Count; i++)
                {
                    if (randomNum > _ProrityCondition[conditionIndex].SpellList[i].chance)
                    {
                        randomNum -= _ProrityCondition[conditionIndex].SpellList[i].chance;
                    }
                    else
                    {
                        //Debug.Log("Spell name : " +_ProrityCondition[conditionIndex].SpellList[i].spell.name);
                        StartCoroutine(TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme(aiEntity,
                            _ProrityCondition[conditionIndex].SpellList[i].spell.Factorize(aiEntity), true));
                        return;
                    }
                }
                Debug.LogError("Error : Spell out of range!");
            }
            else
            {
                StartCoroutine(TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme(aiEntity,
                    _ProrityCondition[conditionIndex].SpellList[currentspellIndex].spell.Factorize(aiEntity), true));
                currentspellIndex++;
                currentspellIndex = currentspellIndex % _ProrityCondition[conditionIndex].SpellList.Count;
            }
        }

        public void TakeControlLeave()
        {
            return;
        }
    }
}
