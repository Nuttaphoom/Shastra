using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Vanaring
{

   
    public class BaseDungeonNode : MonoBehaviour
    {
        private bool _isVisited = false;

        [SerializeField]
        private List<BaseDungeonNode> _connectedNode ;

        [SerializeField]
        private NodeVisualTransitionHandler _nodeVisualTransitionHandler;

        private DungeonNodeTransitionManager _dungeonNodeTransitionManager;

        #region GETTER 
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
                _isVisited = true;
            }

            yield return SetUpNodeTransitions() ; 

         }

        private IEnumerator SetUpNodeTransitions()
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

         
    }
}
