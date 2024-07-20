using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Vanaring.Assets.Scripts.Utilities.StringConstant;
using static UnityEngine.EventSystems.EventTrigger;

namespace Vanaring
{ 
    public class CombatEntityActionHandler
    {
        #region Event Broadcaster 
        private EventBroadcaster _eventBroadcaster;
        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
                _eventBroadcaster.OpenChannel<EntityActionPair>("OnPerformAction");
                _eventBroadcaster.OpenChannel<EntityActionPair>("OnPostPerformAction");
            }

            return _eventBroadcaster;
        }
        public void SubOnPerformAction(UnityAction<EntityActionPair> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnPerformAction");
        }

        public void UnSubOnPerformAction(UnityAction<EntityActionPair> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnPerformAction");
        }

        public void SubOnPostPerformAction(UnityAction<EntityActionPair> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnPostPerformAction");
        }

        public void UnSubOnPostPerformAction(UnityAction<EntityActionPair> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnPostPerformAction");
        }
        #endregion
        private CombatEntity _performerEntity = null; 
        public CombatEntityActionHandler(CombatEntity entity) {
            _performerEntity = entity;
        }   
        protected Queue<ActorAction> _actionQueue = new Queue<ActorAction>();

        

        public IEnumerator PerformActionInQueue()
        {

            ActorAction action = GetActionRuntimeEffect(); 

            //Call on perform action of the ActorAction
            yield return action.PreActionPerform();

            //Old one, now we need to call Timeline asset instead 
            //var eff = action.GetRuntimeEffect();

            //check if still be able to call the action
            if (_performerEntity.ReadyToPerformAction())
            {


                EntityActionPair entityActionPair =  new EntityActionPair() { Actor = _performerEntity, PerformedAction = action };
                GetEventBroadcaster().InvokeEvent<EntityActionPair>(entityActionPair, "OnPerformAction");

                _performerEntity.SetExhaunst(true);// = true;
                Debug.Log("BEFORE  perform action");

                yield return action.PerformAction();

                yield return action.PostActionPerform();


                GetEventBroadcaster().InvokeEvent<EntityActionPair>(entityActionPair, "OnPostPerformAction");


            }

        }

        public bool ActionQueueReady ()
        {
            if (_actionQueue == null)
                return false;

            if (_actionQueue.Count == 0)
                return false;

            return true; 
        }
        private ActorAction GetActionRuntimeEffect()
        {
            if (_actionQueue == null)
                _actionQueue = new Queue<ActorAction>();

            if (_actionQueue.Count == 0)
                return null;

            return _actionQueue.Dequeue();
        }

        public void AddActionQueue(ActorAction actorAction)
        {
            _actionQueue.Enqueue(actorAction);
        }
    }   
}
