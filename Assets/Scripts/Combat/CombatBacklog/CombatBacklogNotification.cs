using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
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
        #region TextShaping Comment 
        public string GetAilmentBacklog(CombatEntity entity,Ailment ailment , bool takeControl, bool recover)
        {
            string comment = null;
            if (takeControl)
            {
                comment =  ailment.GetOnTakeControlComment().GetComment(entity);
            }
            else if (recover)
            {
                comment = ailment.GetOnRecoverComment().GetComment(entity);
            }
            else
                throw new Exception("TakeControl and Reocver both are false");

            return comment; 

        }

        public string GetStatusEffectComment(EntityStatusEffectPair pair, bool onApplied)
        {
            CombatEntity entity = pair.Actor;
            var action = pair.ApplierFactory;
            string comment = null ;
            
            if (onApplied)
                comment = action.GetCommentOnApplied.GetComment(entity);
             
            return comment; 
        }
        public string GetPerformedActionBacklog(EntityActionPair entityActionPair)
        {
            CombatEntity entity = entityActionPair.Actor;
            ActorAction action = entityActionPair.PerformedAction;
            string comment = action.GetDescription().FieldName;

            return comment; 

        }
        #endregion

        #region Ailment Comment Notifier 
        private void NotifyAilmentRecover(EntityAilmentEffectPair ailment)
        {
            string comment = GetAilmentBacklog(ailment.Actor, ailment.Ailment, false, true);

            if (comment != "")
                _combatBacklogDisplayer.EnqueueUtilityTab(comment);

        }
        private void NotifyAilmentControl(EntityAilmentEffectPair ailment)
        {
            string comment = GetAilmentBacklog(ailment.Actor, ailment.Ailment, true, false);

            if (comment != "")
                _combatBacklogDisplayer.EnqueueUtilityTab(comment);

        }
        

        #endregion



        private void NotifyOnStatusEffectApplied(EntityStatusEffectPair pair)
        {
            string comment = GetStatusEffectComment(pair, true);

            if (comment != "")
                _combatBacklogDisplayer.EnqueueUtilityTab(comment);

        }
        private void NotifyOnEntityPerformAction(EntityActionPair entityActionPair)
        {
            string comment = GetPerformedActionBacklog(entityActionPair);
            
            if (comment != "")
                _combatBacklogDisplayer.EnqueueActionTab(comment); 
        }
      
    
       
    }
}
