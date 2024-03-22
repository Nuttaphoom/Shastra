using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngineInternal;
using static Vanaring.NodeTransition;

namespace Vanaring
{
    public class DungeonNodeTransitionManager : MonoBehaviour
    {

        [SerializeField] 
        private NodeTransition _nodeTransitionTemplate ;

        private List<NodeTransition> _nodeTransitions  ; 

        private void Awake()
        {
            if (_nodeTransitionTemplate == null)
                throw new System.Exception("Node Transition Tempalte is null");

            _nodeTransitions = new List<NodeTransition>(); 
        }

        private void Update()
        {
            foreach (NodeTransition nodeTransition in _nodeTransitions)
            {
                NodeTransitionData data;
                if ( ( data = nodeTransition.ReceiveTransitionSignal() ) != null) {
                    StartCoroutine(FindObjectOfType<DungeonManager>().VisiteNextNode(data.DestinationNode )) ;
                }
            }
        }

        public IEnumerator SetUpDungeonNodeTransition(BaseDungeonNode startNode, BaseDungeonNode destinationNode)
        {
            TransitionDirection direction = CalculateTransitionDirect(startNode, destinationNode);
 
            NodeTransition nodeTransition = Instantiate(_nodeTransitionTemplate, transform);
            nodeTransition.SetUpTransitionData(startNode, destinationNode);

            nodeTransition.transform.position = UISpaceSingletonHandler.ObjectToUISpace(startNode.NodeVisualTransitionHandler.GetCorrectPosition(direction));

            _nodeTransitions.Add(nodeTransition);

            yield return null;
        } 
        
        private TransitionDirection CalculateTransitionDirect(BaseDungeonNode startNode, BaseDungeonNode destinationNode)
        {
            Vector3 directionVector = (destinationNode.transform.position - startNode.transform.position).normalized ;

            if (directionVector == startNode.transform.right)
                return TransitionDirection.Right_X;

            else if (directionVector == -startNode.transform.right)
                return TransitionDirection.MinusRight_X;

            if (directionVector == startNode.transform.forward)
                return TransitionDirection.Forward_Z;

            if (directionVector == -startNode.transform.forward)
                return TransitionDirection.MinusForward_Z;

            throw new System.Exception("DirectionVector of " + directionVector + " doesn't match with direction vector of startNode"); 

        }


    }
    public enum TransitionDirection
    {
        Right_X,
        MinusRight_X,
        Forward_Z,
        MinusForward_Z
    };
}
