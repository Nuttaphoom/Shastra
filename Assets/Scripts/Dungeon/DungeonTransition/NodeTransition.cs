using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class NodeTransition : MonoBehaviour
    {

        [SerializeField]
        private Button _nodeTransitionButton; 

        private NodeTransitionData transitionData;

        private bool _isSetup = false;  

        private bool _useTransition = false; 
        #region GETTER 

        public NodeTransitionData ReceiveTransitionData{
            get
            {
                if (! _isSetup)
                    throw new System.Exception("TranstionData hans't never been assigend "); 

                return transitionData; 
            }
        }
        #endregion

        public void SetUpTransitionData(BaseDungeonNode startNode, BaseDungeonNode destinationNode)
        {
            _isSetup = true; 

            transitionData = new NodeTransitionData()
            {
                StartNode = startNode,
                DestinationNode = destinationNode,
            }; 

            transform.position = startNode.transform.position;

            _nodeTransitionButton.onClick.AddListener(EnableTransition) ;
        }

        private void EnableTransition()
        {
            _useTransition = true; 
        }

        public NodeTransitionData ReceiveTransitionSignal()
        {
            if (_useTransition == true) 
                return transitionData;

            return null; 
        }
         
        public class NodeTransitionData
        {
            public BaseDungeonNode DestinationNode;
            public BaseDungeonNode StartNode;
        }
    }
}
