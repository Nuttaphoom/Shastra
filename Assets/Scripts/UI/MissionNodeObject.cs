using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace Vanaring
{
    public class MissionNodeObject : MonoBehaviour
    {
        private BaseMissionNode baseNode;
        public BaseMissionNode GetBaseMissionNode => baseNode;
        [SerializeField] private Image floorGraphic;
        [SerializeField] private Image iconShown;
        private List<MissionPathObject> pathList = new List<MissionPathObject>();
        public List<MissionPathObject> GetPathList => pathList;

        private NodeState state;

        
        private bool isVisisted;
        private bool isVisisting;
        //private BaseMissionNode.VisitationState state;

        public void Init(BaseMissionNode node)
        {
            InitBaseNode(node);
            isVisisted = baseNode.IsThisNodeVisited;
            isVisisting = baseNode.IsCurrentlyVisiting;

            SetFloorGraphicState(Color.black);
            if (isVisisted)
            {
                SetFloorGraphicState(Color.grey);
            }
            if (baseNode.IsCurrentlyVisiting)
            {
                SetFloorGraphicState(Color.yellow);
                Debug.Log("pathList Count: " + pathList.Count);
                foreach (MissionPathObject path in pathList)
                {
                    path.PathReveal();
                }
            }
            //else if (baseNode.IsThisNodeVisited)
            //{
            //    floorGraphic.color = Color.grey;
            //}
            
            if(baseNode != null)
            {
                baseNode.SubOnBeforeVisitThisNode(BeforeVisitNode);

                baseNode.SubOnBeforeExitThisNode(ExitNode);

                baseNode.SubOnBeforeVisitThisNodeFirstTime(FirstTimeVisit);
            }
            else
            {
                Debug.Log("No baseNode can be access!");
            }
        }
        public void InitBaseNode(BaseMissionNode node)
        {
            baseNode = node;
            baseNode.SubOnBeforeVisitThisNode(BeforeVisitNode);

            baseNode.SubOnBeforeExitThisNode(ExitNode);

            baseNode.SubOnBeforeVisitThisNodeFirstTime(FirstTimeVisit);
        }

        private void SetFloorGraphicState(Color color)
        {
            floorGraphic.color = color;
        }

        public void AddPathConnectToThisNode(MissionPathObject newPath)
        {
            pathList.Add(newPath);
        }

        private void BeforeVisitNode(Null n)
        {
           // Debug.Log("beforeVisit");
            //SetFloorGraphicState(Color.magenta);
            iconShown.gameObject.SetActive(true);
            foreach (MissionPathObject path in pathList)
            {
                path.PathReveal();
            }
        }

        private void ExitNode(Null n)
        {
            //Debug.Log("exit");
            SetFloorGraphicState(Color.grey);
            
            iconShown.gameObject.SetActive(false);
        }
        private void FirstTimeVisit(Null n)
        {
            //Debug.Log("firstTimeVisit");
            isVisisted = true;
            SetFloorGraphicState(Color.yellow);
            foreach (MissionPathObject path in pathList)
            {
                path.PathReveal();
            }
        }

        public void OnDisable()
        {
            if(baseNode != null) 
            {
                baseNode.UnSubBeforeOnVisitThisNode(BeforeVisitNode);

                baseNode.UnSubOnBeforeExitThisNode(ExitNode);

                baseNode.UnSubOnBeforeVisitThisNodeFirstTime(FirstTimeVisit);
            }
        }

        public void NodeReveal()
        {
            floorGraphic.gameObject.SetActive(true);
            floorGraphic.color = Color.white;
            if (baseNode.IsCurrentlyVisiting)
            {
                state = NodeState.VISITING;
                SetFloorGraphicState(Color.yellow);
            }
            else if(state == NodeState.VISITING || baseNode.IsThisNodeVisited)
            {
                state = NodeState.VISITED;
                SetFloorGraphicState(Color.grey);
            }
            //Debug.Log("Node Reveal");
        }
    }

    public enum NodeState
    {
        NOT_FOUND,
        NOT_VISIT,
        VISITING,
        VISITED
    }
}
