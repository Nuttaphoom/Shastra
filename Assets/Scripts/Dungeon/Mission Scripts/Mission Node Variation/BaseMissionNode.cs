using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring
{
    public class BaseMissionNode : MonoBehaviour
    {
        #region Event Broadcaster 
        private EventBroadcaster _eventBroadcaster;
        protected EventBroadcaster EventBroadcaster
        {
            get
            { 
                if (_eventBroadcaster == null)
                {
                    _eventBroadcaster = new EventBroadcaster();
                    _eventBroadcaster.OpenChannel<Null>("OnBeforeVisitThisNode");
                    _eventBroadcaster.OpenChannel<Null>("OnBeforeVisitThisNodeFirstTime");
                    _eventBroadcaster.OpenChannel<Null>("OnBeforeExitThisNode");

                }

                return _eventBroadcaster; 
            }
        }

        
        public void SubOnBeforeVisitThisNode(UnityAction<Null> func)
        {
            EventBroadcaster.SubEvent(func, "OnBeforeVisitThisNode");
        }

        public void UnSubBeforeOnVisitThisNode(UnityAction<Null> func)
        {
            EventBroadcaster.UnSubEvent(func, "OnBeforeVisitThisNode");
        }

        public void SubOnBeforeVisitThisNodeFirstTime(UnityAction<Null> func)
        {
            EventBroadcaster.SubEvent(func, "OnBeforeVisitThisNodeFirstTime");
        }

        public void UnSubOnBeforeVisitThisNodeFirstTime(UnityAction<Null> func)
        {
            EventBroadcaster.UnSubEvent(func, "OnBeforeVisitThisNodeFirstTime");
        }

        public void SubOnBeforeExitThisNode(UnityAction<Null> func)
        {
            EventBroadcaster.SubEvent(func, "OnBeforeExitThisNode");
        }

        public void UnSubOnBeforeExitThisNode(UnityAction<Null> func)
        {
            EventBroadcaster.UnSubEvent(func, "OnBeforeExitThisNode");
        }

        #endregion 

        public enum VisitationState
        {
            NotVisited,
            Visited,
            Visiting
        }

        public VisitationState visistationState = VisitationState.NotVisited ; 

        [SerializeField]
        private List<BaseMissionNode> _connectedNode ;

        [SerializeField]
        private NodeVisualTransitionHandler _nodeVisualTransitionHandler;

        #region GETTER 
        public bool IsThisNodeVisited { 
        get
            {
                return visistationState == VisitationState.Visited || IsCurrentlyVisiting  ;
            }
        }       

        public bool IsCurrentlyVisiting
        {
            get
            {
                return visistationState == VisitationState.Visiting ; 
            }
        }
        
        public NodeVisualTransitionHandler NodeVisualTransitionHandler
        {
            get { return _nodeVisualTransitionHandler ; }

        }
        public List<BaseMissionNode> ConnectedNode
        {
            get {
                if (_connectedNode == null)
                    throw new System.Exception("ConnectedNode of " + gameObject.name + " hasn't been assigned");  

                return _connectedNode; 
            }
        }

        #endregion  

        
        public virtual IEnumerator OnLeaveThisNode()
        {
            EventBroadcaster?.InvokeEvent<Null>(null, "OnBeforeExitThisNode");

            if (! IsThisNodeVisited)
                throw new System.Exception("This node hasn't never been visited " ) ;

            visistationState = VisitationState.Visited ;

            yield return null; 
        }

        public virtual IEnumerator OnVisiteThisNode()
        {

            EventBroadcaster?.InvokeEvent<Null>(null,"OnBeforeVisitThisNode");

            if (!IsThisNodeVisited)
            {            
                visistationState = VisitationState.Visiting;

                yield return OnVisiteThisNodeFirstTime(); 
            }



        }

        public virtual IEnumerator OnVisiteThisNodeFirstTime()
        {
            EventBroadcaster.InvokeEvent<Null>(null, "OnBeforeVisitThisNodeFirstTime");

            yield return null; 
        }

       

        public bool IsConnectedNode(BaseMissionNode nextNode)
        {
            foreach (var node in ConnectedNode)
            {
                if (nextNode == node) 
                    return true ;
            }

            return false;
        }

        public virtual NodeRuntimeData CaptureNodeData()
        {
            return new NodeRuntimeData()
            {
                IsVisited = IsThisNodeVisited,
                CurrentlyVisited = IsCurrentlyVisiting ,  
            };
        }

        public virtual void RestoreNodeData(NodeRuntimeData data)
        {
            if (data.CurrentlyVisited)
            {
                visistationState = VisitationState.Visiting;
            }
            else if (data.IsVisited)
            {
                visistationState = VisitationState.Visited ;
            }
        }

        public virtual void OnExitMission_ClearNodeData()
        {
            visistationState = VisitationState.NotVisited; 
        } 

         
    }
}
