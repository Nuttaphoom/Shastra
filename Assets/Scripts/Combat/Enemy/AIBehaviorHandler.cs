using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;


namespace Vanaring
{
    public class AIBehaviorHandler : MonoBehaviour
    {       
        [SerializeField]
        private List<BehaviorInstance> _behaviors;

        private AIEntity aiEntity;

        private int _comboBehaviorIndex = -1 ; 

        private void Awake()
        {
            if (!TryGetComponent(out aiEntity))
            {
                throw new Exception("AI entity can not be found");
            }

            foreach (var behavior in _behaviors)
            {
                behavior.SetUpBehavior(this); 
            }
        }

        private int CheckingCondition()
        {
            int priority = -100;
            int conditionIndex = -1;
            for (int i = 0; i < _behaviors.Count; i++)
            {
                if (_behaviors[i].GetPriority() < priority)
                    continue;

                if (_behaviors[i].IsConditionMet(aiEntity))
                {
                    conditionIndex = i; 
                    priority = _behaviors[i].GetPriority();
                }
                
            }

            if (conditionIndex == -1)
                throw new Exception("No valid behavior's condition has been met");

            return conditionIndex ; 
        }

        public IEnumerator GetNextAction()
        {
            if (_comboBehaviorIndex != -1)
            {
                yield return _behaviors[_comboBehaviorIndex].ExecuteBehavior(aiEntity);
            }
            else
            {
                int behaviorsIndex = CheckingCondition();

                yield return _behaviors[behaviorsIndex].ExecuteBehavior(aiEntity);

                if (_behaviors[behaviorsIndex].GetComboHandler().IsCombo)
                    RegisterCombo(behaviorsIndex);
            }
        }

        private void RegisterCombo(int behaviorsIndex)
        {
            if (_behaviors[behaviorsIndex].GetComboHandler().IsPerformingCombo)
                return;

            if (_comboBehaviorIndex != -1)
                throw new Exception("Try to register new combo while _comboBehaviorIndex don't equal to -1");

            _comboBehaviorIndex = behaviorsIndex;

            _behaviors[_comboBehaviorIndex].GetComboHandler().PerformCombo() ;

        }

        public void ResetOrderNotification()
        {
            if (_comboBehaviorIndex != -1)
            {
                _behaviors[_comboBehaviorIndex].GetComboHandler().OnResetOrder_EndPerformingCombo();
                _comboBehaviorIndex = -1;
            }
        }

        public void OnBehaviorOwnerStun()
        {
            if (_comboBehaviorIndex == -1)
                return; 

            if (!_behaviors[_comboBehaviorIndex].GetComboHandler().IsPerformingCombo)
                return;

            if (_behaviors[_comboBehaviorIndex].GetComboHandler().OnOwnerStun_BreakCombo() )
                _behaviors[_comboBehaviorIndex].ResetActionOrder();

        }


    }

    [Serializable]
    public class BehaviorInstance
    {
        [Header("BehaviorDescription served as a note for devs")]
        [SerializeField]
        private string _behaviorDescription;

        #region PrivateStructEnum
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

        #endregion

        #region SetUpVariables
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

        #endregion

        private int _currentOrder = 0;

        private AIBehaviorHandler _aiBehaviorHandler;

        [SerializeField]
        private BehaviorComboHandler _comboHandler; 

        #region GETTER
        public int GetPriority()
        {
            return _priority;
        }

        public BehaviorComboHandler GetComboHandler()
        {
            return _comboHandler ; 
        }

        #endregion

        public void ResetActionOrder()
        {
            _currentOrder = 0;
            _aiBehaviorHandler.ResetOrderNotification();
        }
        private void ProgressActionOrder()
        {
            _currentOrder = (_currentOrder + 1);

            if (_currentOrder >= _actions.Count)
            {
                ResetActionOrder(); 
            }
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
                if (rand <= actionInstance.Posibility + total)
                    return actionInstance.ActionFactory;

                total += actionInstance.Posibility;
            }

            throw new Exception("no matching return action");
        }

        private ActorActionFactory GetBehaviorAction()
        {
            if (_invokeOrder == ActionInvokeOrder.Order)
                return _actions[_currentOrder].ActionFactory; ;


            if (_invokeOrder == ActionInvokeOrder.Random)
                return GetRandomAction();

            throw new Exception("_invokeOrder was set to invalid value ");
        }

        #region PublicMethods
        public void SetUpBehavior(AIBehaviorHandler ai)
        {
            _aiBehaviorHandler = ai; 
        }
        public bool IsConditionMet(CombatEntity actor)
        {
            if (AND && OR)
                throw new Exception("AND and OR cannot both be true");

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
       
        public IEnumerator ExecuteBehavior(CombatEntity actor )
        {
            yield return TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme(actor,
    GetBehaviorAction().FactorizeRuntimeAction(actor), true);

            if (_invokeOrder == ActionInvokeOrder.Order)
                ProgressActionOrder();

            yield return null; 
        }

        #endregion
    }

    [Serializable]
    public class BehaviorComboHandler
    {
        #region Public variable
        [Header("Combo behavior will keep performing until last action is performed")]
        [SerializeField]
        private bool _combo = false;

        [SerializeField]
        private bool _breakOnStun = false; 
        #endregion

        private bool _performingCombo = false;

        #region GETTER
        public bool IsPerformingCombo => _performingCombo;

        public bool IsCombo => _combo; 

        #endregion

        public void PerformCombo(   )
        {
            if (!_combo)
                return;

            if (!_performingCombo)
                _performingCombo = true; 
        }

        public void OnResetOrder_EndPerformingCombo()
        {
            if (!_combo)
                return; 
        
            _performingCombo= false;
        }

        public bool OnOwnerStun_BreakCombo()
        {
         

            return _breakOnStun; 


        }
      
 
    }

}
