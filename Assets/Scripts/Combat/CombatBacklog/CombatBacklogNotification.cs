using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring
{
    public class CombatBacklogNotification : MonoBehaviour
    {
        [SerializeField]
        private CombatBacklogDisplayer _combatBacklogDisplayer;

        [Header("TODO : Commentator should be loaded along with level / team data")]
        [SerializeField]
        private CommentatorSO _commentator;

        private CommentatorRuntime _commentatorRuntime;

        [SerializeField]
        private CombatReferee _referee;

        private List<CombatEntity> _entitiesWithNotification = new List<CombatEntity>() ;

        private void Awake()
        {
            if (_combatBacklogDisplayer == null)
                throw new System.Exception("_combatBacklogDisplayer is null");

            _commentatorRuntime = _commentator.CreateCommentatorRuntime();

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
            entity.SubOnStatusEffectApplied(NotifyOnStatusEffectApplied);
            entity.SubOnAilmentControlEventChannel(NotifyAilmentControl);
            entity.SubOnAilmentRecoverEventChannel(NotifyAilmentRecover);
        }

        private void UnSubscriptionWithEntity(CombatEntity entity)
        {
            entity.UnSubOnPerformAction(NotifyOnEntityPerformAction);
            entity.UnSubOnStatusEffectApplied(NotifyOnStatusEffectApplied);
            entity.UnSubOnAilmentControlEventChannel(NotifyAilmentControl);
            entity.UnSubOnAilmentRecoverEventChannel(NotifyAilmentRecover);

        }
        private void NotifyAilmentRecover(EntityAilmentEffectPair ailment)
        {
            _combatBacklogDisplayer.DisplayAilmentBacklog(ailment,false,true);

        }
        private void NotifyAilmentControl(EntityAilmentEffectPair ailment)
        {
            _combatBacklogDisplayer.DisplayAilmentBacklog(ailment,true,false);

        }
        private void NotifyOnStatusEffectApplied(EntityStatusEffectPair pair)
        {
            _combatBacklogDisplayer.DisplayStatusEffectAppliedBacklog(pair);
        }
        private void NotifyOnEntityPerformAction(EntityActionPair entityActionPair)
        {
            _combatBacklogDisplayer.DisplayPerformedActionBacklog(entityActionPair); 
        }
      
    
        public void NotifyString(string s)
        {
            _combatBacklogDisplayer.DisplayUtilityWithStringBacklog(s); 
        }
    }
}
