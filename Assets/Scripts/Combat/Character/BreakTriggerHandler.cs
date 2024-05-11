using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [Serializable] 
    public class BreakTriggerHandler    
    {
        /// <summary>
        /// If DebugMode is enable, _triggerActionSO won't be loaded
        /// </summary>
        [SerializeField]
        private List<TriggerActionSO> _triggerActionSO = new List<TriggerActionSO>() ;

        private CombatEntity _entity;
        public void Initialize(CombatEntity entity)
        {
            _entity = entity;  
    
        }
        
        public IEnumerator TriggerAction()
        {
            return _entity.GetAction();
        }

        public void LoadTriggerActionFromDatabase(List<TriggerActionSO> loadedTriggerAction)
        {
            _triggerActionSO = new List<TriggerActionSO>();

        }
    }
}
