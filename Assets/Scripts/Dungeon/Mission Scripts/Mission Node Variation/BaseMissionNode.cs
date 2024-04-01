using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Vanaring
{

   
    public class BaseMissionNode : MonoBehaviour
    {
        protected enum VisitationState
        {
            NotVisited,
            Visited,
            Visiting
        }


        protected VisitationState visistationState = VisitationState.NotVisited ; 

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
            if (! IsThisNodeVisited)
                throw new System.Exception("This node hasn't never been visited " ) ;

            visistationState = VisitationState.Visited ;



            yield return null; 
        }

        public virtual IEnumerator OnVisiteThisNode()
        {

            if (!IsThisNodeVisited)
            {            
                visistationState = VisitationState.Visiting;

                yield return OnVisiteThisNodeFirstTime(); 
            }



        }

        public virtual IEnumerator OnVisiteThisNodeFirstTime()
        {
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
