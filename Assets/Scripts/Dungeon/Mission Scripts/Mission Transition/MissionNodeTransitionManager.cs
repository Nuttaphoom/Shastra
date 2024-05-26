using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngineInternal;
using static Vanaring.NodeTransition;

namespace Vanaring
{
    [Serializable]
    public class MissionNodeTransitionManager  
    {
        [SerializeField]
        private Canvas _transitionCanvas; 

        [SerializeField] 
        private NodeTransition _nodeTransitionTemplate ;

        private List<NodeTransition> _nodeTransitions  ;

        

        public void Init()
        {
            if (_nodeTransitionTemplate == null)
                throw new System.Exception("Node Transition Tempalte is null");

            _nodeTransitions = new List<NodeTransition>();

            
         
        }


        public IEnumerator SetUpDungeonNodeTransition(BaseMissionNode startNode, BaseMissionNode destinationNode)
        {
            TransitionDirection direction = CalculateTransitionDirect(startNode, destinationNode);
 
            NodeTransition nodeTransition = MonoBehaviour.Instantiate(_nodeTransitionTemplate, _transitionCanvas.transform);
            nodeTransition.SetUpTransitionData(startNode, destinationNode, direction);

            nodeTransition.transform.position = UISpaceSingletonHandler.ObjectToUISpace(startNode.NodeVisualTransitionHandler.GetCorrectPosition(direction));

            _nodeTransitions.Add(nodeTransition);

            yield return null;
        } 

        public void ClearDungeonNodeTransition()
        {
            for (int i =  _nodeTransitions.Count - 1; i >= 0; i--)
            {
                NodeTransition nodeTransition = _nodeTransitions[i];
                _nodeTransitions.RemoveAt(i);
                MonoBehaviour.Destroy(nodeTransition.gameObject);
            }

        }
        
        public TransitionDirection CalculateTransitionDirect(BaseMissionNode startNode, BaseMissionNode destinationNode)
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
