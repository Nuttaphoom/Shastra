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

        private bool isVisisted;
        private bool isVisisting;
        //private BaseMissionNode.VisitationState state;

        public void Init(BaseMissionNode node)
        {
            baseNode = node;
            isVisisted = baseNode.IsThisNodeVisited;
            isVisisting = baseNode.IsCurrentlyVisiting;

            floorGraphic.color = Color.black;
            if (baseNode.IsCurrentlyVisiting)
            {
                floorGraphic.color = Color.yellow;
                Debug.Log("pathList Count: " + pathList.Count);
                foreach (MissionPathObject path in pathList)
                {
                    path.PathReveal();
                }
            }
            else if (baseNode.IsThisNodeVisited)
            {
                floorGraphic.color = Color.grey;
            }
            


            baseNode.SubOnBeforeVisitThisNode(BeforeVisitNode);

            baseNode.SubOnBeforeExitThisNode(ExitNode);

            baseNode.SubOnBeforeVisitThisNodeFirstTime(FirstTimeVisit);
        }

        public void AddPathConnectToThisNode(MissionPathObject newPath)
        {
            pathList.Add(newPath);
        }

        private void BeforeVisitNode(Null n)
        {
            Debug.Log("beVisit");
            floorGraphic.color = Color.magenta;
        }

        private void ExitNode(Null n)
        {
            Debug.Log("exit");
            floorGraphic.color = Color.grey;
        }
        private void FirstTimeVisit(Null n)
        {
            floorGraphic.color = Color.yellow;
        }

        public void OnDisable()
        {
            baseNode.UnSubBeforeOnVisitThisNode(BeforeVisitNode);

            baseNode.UnSubOnBeforeExitThisNode(ExitNode);

            baseNode.UnSubOnBeforeVisitThisNodeFirstTime(FirstTimeVisit);
        }

        public void NodeReveal()
        {
            floorGraphic.gameObject.SetActive(true);
        }
    }
}
