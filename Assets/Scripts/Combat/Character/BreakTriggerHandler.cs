using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [Serializable] 
    public class BreakTriggerHandler    
    {
        private class TriggerStatus
        {
            public List<CombatEntity> BrokenTargets = new List<CombatEntity>() ; 
        }
        /// <summary>
        /// If DebugMode is enable, _triggerActionSO won't be loaded
        /// </summary>
        [SerializeField]
        private List<TriggerActionSO> _triggerActionSO = new List<TriggerActionSO>() ;

        private CombatEntity _entity;

        private TriggerStatus _currentTriggerStatus; 
        public void Initialize(CombatEntity entity)
        {
            _entity = entity;  
    
        }
         
        public void ResolveTrigger()
        {
            if (_currentTriggerStatus == null)
                return;

            if (_triggerActionSO[0] == null)
                throw new Exception("there is no valid _triggerActionSO") ; 
            
            ActorAction action = _triggerActionSO[0].FactorizeRuntimeAction(_entity);
            throw new Exception("Right now we need to determine the target detail within TriggeActionSO"); 

            action.SetActionTarget(_currentTriggerStatus.BrokenTargets) ;

            _entity.ActionHandler.AddActionQueue(action);
        }
        public void EnableTriggerAction(CombatEntity target)
        {
            if (_currentTriggerStatus == null)
                _currentTriggerStatus = new TriggerStatus();

            _currentTriggerStatus.BrokenTargets.Add(target); 

            Debug.Log("Right now we just call TriggerAction everytime Caster break energy");

        }

        public void LoadTriggerActionFromDatabase(List<TriggerActionSO> loadedTriggerAction)
        {
            _triggerActionSO = new List<TriggerActionSO>();

        }
    }
}
