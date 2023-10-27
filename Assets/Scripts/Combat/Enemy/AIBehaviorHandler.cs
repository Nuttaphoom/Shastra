using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.Events;


namespace Vanaring
{
    public class AIBehaviorHandler : MonoBehaviour
    {
        [Serializable]
        public enum SpellOrderType
        {
            Random = 0,
            Order,
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

        private AIEntity aiEntity;

        [SerializeField]
        private List<BehaviorInstance> _behaviors;

        private int conditionIndex = 0;

        private void Awake()
        {
            if (!TryGetComponent(out aiEntity))
            {
                throw new Exception("AI entity can not be found");
            }
        }

        public void CheckingCondition()
        {
            conditionIndex = -1;
            int priority = -100;
            for (int i = 0; i < _behaviors.Count; i++)
            {
                if (_behaviors[i].Priority < priority)
                    continue;

                if (_behaviors[i].IsConditionMet(aiEntity))
                {
                    Debug.Log("condition met at " + i);
                    priority = _behaviors[i].Priority;
                    conditionIndex = i;
                }
            }

            if (conditionIndex == -1)
                throw new Exception("No valid behavior's condition has been met");
        }

        public IEnumerator GetNextAction()
        {
            yield return _behaviors[conditionIndex].ExecuteBehavior(aiEntity);
        }

    }

    [Serializable]
    public class BehaviorInstance
    {
        [Serializable]
        public enum ActionInvokeOrder
        {
            Random = 0,
            Order,
        }

        [Serializable]
        private struct BehaviorActionInformation
        {
            [SerializeField]
            private ActorActionFactory _actionFactory;

            [Range(0.0f,1.0f)]
            [SerializeField]
            private float _possibility ;

            public ActorActionFactory ActionFactory => _actionFactory;

            public float Posibility => _possibility ; 
        }

        [Header("# Use 'OR' to return true if at least one condition is true.\r\n# Use 'AND' to require all conditions to be true for a true result.")]
        [SerializeField]
        private bool OR;

        [SerializeField]
        private bool AND;
 
        [Header("Conditions for this set of action to be executed")]
        [SerializeField]
        private List<BaseConditionSO> _behaviorCondition;

        [SerializeField]
        private ActionInvokeOrder _invokeOrder;

        [SerializeField]
        private List<BehaviorActionInformation> _actions;

        [Header("Behavior with highest priority will be executed first")]
        [SerializeField]
        private int _priority = 0;

        private int _currentOrder = 0; 

        #region GETTER
        public int Priority => _priority;
        #endregion

        #region PublicMethods
        public bool IsConditionMet(CombatEntity actor)
        {
            if (AND && OR)
                throw new Exception("AND and OR cannot both be true");

            if (_performingCombo)
                return true;

            if (AND)
            {
                foreach (var condition in _behaviorCondition)
                {
                    if (!condition.ConditionsMet(actor))
                        return false;
                }
                return true;
            }

            if (OR)
            {
                foreach (var condition in _behaviorCondition)
                {
                    if (condition.ConditionsMet(actor))
                        return true;
                }
            }

            return false;
        }

        #region ComboRegion
        [Header("Combo behavior will keep performing until last action is performed")]
        [SerializeField]
        private bool _combo = false;
        private bool _performingCombo = false;
        private bool _comboFinished = false; 
        private int _tempPri = 0 ; 
        private void LockCombo()
        {
            _performingCombo = true;
            _tempPri = _priority; 
            _priority = VanaringMathConst.InfinityValue ; 
        }

        private void ReleaseCombo()
        {
            _priority = _tempPri ;
            _performingCombo = false ;
            _comboFinished = false; 
        }
        #endregion

        private ActorActionFactory GetOrderAction()
        {
            ActorActionFactory ret = _actions[_currentOrder].ActionFactory;
            _currentOrder = (_currentOrder + 1); 

            if ( (_currentOrder = (_currentOrder % _actions.Count) ) == 0)
            {
                _comboFinished = true; 
            }

            return ret; 
        }

        private ActorActionFactory GetRandomAction()
        {
            float total = 0;
            foreach (var actionInstance in _actions)
            {
                total += actionInstance.Posibility;
            }

            if (total != 1.0f)
                throw new Exception("Sum of posibility is not equal to 1.0 ");

            float rand = UnityEngine.Random.Range(0.0f, 1.0f);

            total = 0; 

            foreach (var actionInstance in _actions)
            {
                if (total + rand <= actionInstance.Posibility)
                    return actionInstance.ActionFactory;

                total += actionInstance.Posibility ; 
            }

            throw new Exception("no matching return action");
        }

        private ActorActionFactory GetBehaviorAction()
        {
            if (_combo && !_performingCombo)
                LockCombo(); 
            

            if (_invokeOrder == ActionInvokeOrder.Order || _performingCombo)
                return GetOrderAction();  
           

            if (_invokeOrder == ActionInvokeOrder.Random)
                return GetRandomAction();
            
            throw new Exception("_invokeOrder was set to invalid value ") ; 
        }
        public IEnumerator ExecuteBehavior(CombatEntity actor)
        {
            yield return TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme(actor,
    GetBehaviorAction().FactorizeRuntimeAction(actor), true);

            if (_comboFinished)
                ReleaseCombo() ; 
        }

        #endregion



    }
}
