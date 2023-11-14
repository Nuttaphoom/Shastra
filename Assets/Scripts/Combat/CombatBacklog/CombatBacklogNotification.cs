using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring
{
    public class CombatBacklogNotification : MonoBehaviour
    {
        [SerializeField]
        private CombatBacklogDisplayer _combatBacklogDisplayer; 

        private CombatReferee _referee;
        private List<CombatEntity> _entitiesWithNotification = new List<CombatEntity>() ;

        private void Awake()
        {
            if (_combatBacklogDisplayer == null)
                throw new System.Exception("_combatBacklogDisplayer is null");

        }
        private void Start()
        {
            _referee = CombatReferee.Instance;

            _referee.SubOnCompetitorEnterCombat(OnNewEntityEnterCombat);  
            
        }

        private void OnDisable()
        {
            _referee.UnSubOnCompetitorEnterCombat(OnNewEntityEnterCombat);
            foreach (CombatEntity entity in _entitiesWithNotification)
            {
                UnSubscriptionWithEntity(entity);
            }
        }

        private void OnNewEntityEnterCombat(CombatEntity entity)
        {
            _entitiesWithNotification.Add(entity);

            SubscriptionWithEntity(entity); 

        }

        private void SubscriptionWithEntity(CombatEntity entity)
        {
            entity.SubOnPerformAction(NotifyOnEntityPerformAction);
            entity.SubOnEntityStunEvent(NotifyOnEntityStun);
        }

        private void UnSubscriptionWithEntity(CombatEntity entity)
        {
            entity.UnSubOnPerformAction(NotifyOnEntityPerformAction);
            entity.UnSubOnEntityStunEvent(NotifyOnEntityStun);
        }

        private void NotifyOnEntityPerformAction(EntityActionPair entityActionPair)
        {
            _combatBacklogDisplayer.DisplayPerformedActionBacklog(entityActionPair); 
        }

        private void NotifyOnEntityStun(CombatEntity entity)
        {
            ColorfulLogger.LogWithColor(entity.CharacterSheet.CharacterName + " Stun", Color.cyan);
        }
    }
}
