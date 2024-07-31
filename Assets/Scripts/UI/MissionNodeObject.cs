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
        [SerializeField] private Animator animator;
        private List<MissionPathObject> pathList = new List<MissionPathObject>();
        public List<MissionPathObject> GetPathList => pathList;
        public Image GetNodeIcon => iconShown;

        private NodeState state;
        public void Init(BaseMissionNode node)
        {
            InitBaseNode(node);

            SetFloorGraphicState(Color.black);

            if (baseNode.IsCurrentlyVisiting)
            {
                SetFloorGraphicState(Color.yellow);
                //Debug.Log("pathList Count: " + pathList.Count);
                foreach (MissionPathObject path in pathList)
                {
                    path.PathReveal();
                }
            }
            if (baseNode.IsThisNodeVisited)
            {
                animator.Play("NodeObjectNoState");
            }
        }
        public void InitBaseNode(BaseMissionNode node)
        {
            baseNode = node;

            if (baseNode != null)
            {
                baseNode.SubOnBeforeVisitThisNode(BeforeVisitNode);

                baseNode.SubOnBeforeExitThisNode(ExitNode);

                baseNode.SubOnBeforeVisitThisNodeFirstTime(FirstTimeVisit);
            }
            else
            {
                Debug.Log("No baseNode can be access!");
            }

            //animator.Play("NodeObjectVisittingState");
        }

        public void PlayVisitingAnimation()
        {
            //Debug.Log("NodeVisitAnim");
            //yield return new WaitForSeconds(1.0f);
            animator.Play("NodeObjectVisittingState");
        }

        private void SetFloorGraphicState(Color color)
        {
            //Debug.Log("Set Color" + color);
            floorGraphic.color = color;
        }

        public void AddPathConnectToThisNode(MissionPathObject newPath)
        {
            pathList.Add(newPath);
        }

        private void BeforeVisitNode(Null n)
        {
            //Debug.Log("BeforeVisit");
            iconShown.gameObject.SetActive(true);
            state = NodeState.VISITING;
            SetFloorGraphicState(Color.yellow);
            animator.Play("NodeObjectVisittingState");
            foreach (MissionPathObject path in pathList)
            {
                path.PathReveal();
            }
        }

        public void ExitNode(Null n)
        {
            //Debug.Log("ExitNode");
            SetFloorGraphicState(Color.grey);
            state = NodeState.VISITED;
            iconShown.gameObject.SetActive(false);
            animator.Play("NodeObjectNoState");
        }
        private void FirstTimeVisit(Null n)
        {
            //Debug.Log("First");
            animator.Play("NodeObjectVisittingState");
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

            if (!baseNode.IsCurrentlyVisiting && !baseNode.IsThisNodeVisited)
            {
                animator.Play("NodeObjectStateChange");
            }else if (baseNode.IsThisNodeVisited)
            {
                floorGraphic.color = Color.grey;
            }
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
