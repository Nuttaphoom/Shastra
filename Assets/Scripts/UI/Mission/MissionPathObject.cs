using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class MissionPathObject : MonoBehaviour
    {
        [SerializeField] private Image graphic;
        private MissionNodeObject startNode;
        private MissionNodeObject desNode;
        

        public void InitConnectedNode(MissionNodeObject startNode, MissionNodeObject destinationNode)
        {
            graphic.gameObject.SetActive(false);
            this.startNode = startNode;
            desNode = destinationNode;
        }

        public void PathReveal()
        {
            //Debug.Log("Path Reveal");
            graphic.gameObject.SetActive(true);
            desNode.NodeReveal();
        }

    }
}
