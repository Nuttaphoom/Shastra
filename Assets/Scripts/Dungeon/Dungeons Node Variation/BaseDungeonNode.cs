using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Vanaring
{

   
    public class BaseDungeonNode : MonoBehaviour
    {
        protected bool _isVisited = false;

        [SerializeField]
        private List<BaseDungeonNode> _connectedNode ;

        [SerializeField]
        private NodeVisualTransitionHandler _nodeVisualTransitionHandler;

        private DungeonNodeTransitionManager _dungeonNodeTransitionManager;

        #region GETTER 
        public bool IsThisNodeVisited { 
        get
            {
                return _isVisited;
                
            }
        }       
        protected DungeonNodeTransitionManager GetDungeonNodeTransitionManager
        {
            get
            {
                if (_dungeonNodeTransitionManager == null)
                    _dungeonNodeTransitionManager = FindObjectOfType<DungeonNodeTransitionManager>();

                return _dungeonNodeTransitionManager ;
            }
        }
        public NodeVisualTransitionHandler NodeVisualTransitionHandler
        {
            get { return _nodeVisualTransitionHandler ; }

        }
        public List<BaseDungeonNode> ConnectedNode
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
            if (!_isVisited)
            {
                throw new System.Exception("This node hasn't never been visited " ) ;
            }

            ClearUpNodeTransitions() ;

            yield return null; 
        }

        public virtual IEnumerator OnVisiteThisNode()
        {
            if (!_isVisited)
            {
                Debug.Log("set visite this node"); 
                _isVisited = true;
            }

            yield return SetUpNodeTransitions() ; 

         }

        protected IEnumerator SetUpNodeTransitions()
        {
            foreach (BaseDungeonNode node in _connectedNode)
            {
                yield return GetDungeonNodeTransitionManager.SetUpDungeonNodeTransition(this, node); 
            }

            yield return null; 
        }

        private void ClearUpNodeTransitions()
        {
            GetDungeonNodeTransitionManager.ClearDungeonNodeTransition();

        }

        public bool IsConnectedNode(BaseDungeonNode nextNode)
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
            };
        }

        public virtual void RestoreNodeData(NodeRuntimeData data)
        {
            _isVisited = data.IsVisited;
        }

         
    }
}
