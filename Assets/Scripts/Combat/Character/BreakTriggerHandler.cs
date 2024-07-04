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

        [SerializeField]
        private CutsceneDirector _breakTriggerDirector ; 

        private CombatEntity _entity;

        private TriggerStatus _currentTriggerStatus;

        private bool ShouldCheckTrigger = true ;

        public void DisableTriggerChecker(bool disable)
        {
            ShouldCheckTrigger = ! disable;

        }
        public bool TriggerActive
        {
            get
            {
                return _currentTriggerStatus != null;
            }
        } 

        public void Initialize(CombatEntity entity)
        {
            _entity = entity;  
        }
         
        public IEnumerator ResolveTrigger()
        {
            if (_currentTriggerStatus == null || CombatReferee.Instance.IsGameEnd() )
            {
                //ColorfulLogger.LogWithColor("Trigger Not Active", Color.red);


                goto End;

            }

            ActorAction action = _triggerActionSO[0].FactorizeTriggerEffect(_entity, _currentTriggerStatus.BrokenTargets);
            
            if (action.GetActionTargets().Count == 0)
                goto End;

            var s = MonoBehaviour.Instantiate(_breakTriggerDirector); 
            yield return s.PlayCutscene(); 

            MonoBehaviour.Destroy(s.gameObject) ;

            if (_triggerActionSO[0] == null)
                throw new Exception("there is no valid _triggerActionSO") ; 
            
            

       
            
            _entity.ActionHandler.AddActionQueue(action);

            _currentTriggerStatus = null;

            End:
            yield return null;
        }
        public void EnableTriggerAction(CombatEntity target)
        {
            if (! ShouldCheckTrigger)
                return;
            
            if (_triggerActionSO == null ||  _triggerActionSO.Count == 0)
                return;

            if (_currentTriggerStatus == null)
                _currentTriggerStatus = new TriggerStatus();

            _currentTriggerStatus.BrokenTargets.Add(target); 

            //Debug.Log("Right now we just call TriggerAction everytime Caster break energy");

        }

        public void LoadTriggerActionFromDatabase(List<TriggerActionSO> loadedTriggerAction)
        {
            _triggerActionSO = new List<TriggerActionSO>();

        }
    }
}
