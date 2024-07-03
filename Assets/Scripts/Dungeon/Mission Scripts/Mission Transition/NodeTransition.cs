using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class NodeTransition : MonoBehaviour
    {

        [SerializeField]
        private Button _nodeTransitionButton;
        public Button NodeTransitionButton => _nodeTransitionButton;
        [SerializeField] private Sprite forwardArrow;
        [SerializeField] private Sprite backwardArrow;
        [SerializeField] private Sprite rightArrow;
        [SerializeField] private Sprite leftArrow;

        private NodeTransitionData transitionData;

        private bool _isSetup = false;  

        private bool _useTransition = false; 
        #region GETTER 

        public NodeTransitionData ReceiveTransitionData {
            get
            {
                if (! _isSetup)
                    throw new System.Exception("TranstionData hans't never been assigend "); 

                return transitionData; 
            }
        }
        #endregion

        public void SetUpTransitionData(BaseMissionNode startNode, BaseMissionNode destinationNode, TransitionDirection direction)
        {
            _isSetup = true; 

            transitionData = new NodeTransitionData()
            {
                StartNode = startNode,
                DestinationNode = destinationNode,
            }; 

            transform.position = startNode.transform.position;

            _nodeTransitionButton.onClick.AddListener(EnableTransition) ;

            Sprite newSprite = null;
            switch (direction)
            {
                case TransitionDirection.Forward_Z:
                    newSprite = leftArrow;
                    break;
                case TransitionDirection.MinusForward_Z:
                    newSprite = rightArrow;
                    break;
                case TransitionDirection.MinusRight_X:
                    gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.15f, 1.15f, 1.15f);
                    newSprite = backwardArrow;
                    break;
                case TransitionDirection.Right_X:
                    newSprite = forwardArrow;
                    
                    break;
            }

            _nodeTransitionButton.GetComponent<Image>().sprite = newSprite;
        }

        private void EnableTransition()
        {
            _useTransition = true;

            FindObjectOfType<MissionNodeManager>().StartCoroutine(FindObjectOfType<MissionNodeManager>().VisiteNextNode(transitionData.DestinationNode));
        }
         
        public class NodeTransitionData
        {
            public BaseMissionNode DestinationNode;
            public BaseMissionNode StartNode;
        }

        
    }
}
