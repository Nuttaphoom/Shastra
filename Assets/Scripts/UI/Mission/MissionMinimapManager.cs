using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class MissionMinimapManager : MonoBehaviour
    {
        
        private BaseMissionNode firstNode;
        private BaseMissionNode visitingNode;
        private List<BaseMissionNode> allConnectedNodeList = new List<BaseMissionNode>();
        private Queue<MissionNodeObject> unConnectNodeList = new Queue<MissionNodeObject>();
        private List<MissionNodeObject> nodeObjectList = new List<MissionNodeObject>();
        private MissionNodeTransitionManager missionNodeTransitionManager;
        private MissionNodeEnvironment mission;

        //private MissionSetupHandler setUpHandler;
        [SerializeField] private GameObject nodeField;
        [SerializeField] private MissionNodeObject curNode;
        [SerializeField] private MissionNodeObject dungeonNode;
        [SerializeField] private MissionPathObject pathNode_x;
        [SerializeField] private MissionPathObject pathNode_z;
        private GameObject focusNode;
        private MissionPathObject path;
        private int runningIndex = 0;
        [SerializeField] private Sprite lootIcon;
        [SerializeField] private Sprite dungeonIcon;
        [SerializeField] private Sprite cutsceneIcon;

        private MissionNodeObject curNodeVisit;

        [ContextMenu("Init Minimap")]

        private void Start()
        {
            DungeonManagerSingleton.Instance.MissionSetupHandler.SubOnEnvironmentSetup(Init);
        }

        public void OnDisable()
        {
            //ARM -- This line causes bugs when exit Mission as DungeonManagerSingleton will be destroyed and can't find its ref
            //DungeonManagerSingleton.Instance.MissionSetupHandler.UnSubOnEnvironmentSetup(Init);
        }

        public void Init(Null n)
        {
            missionNodeTransitionManager = FindObjectOfType<MissionNodeManager>().MissionNodeTransitionManager; 
            mission = DungeonManagerSingleton.Instance.MissionSetupHandler.DungeonEnvironment;
            firstNode = mission.GetFirstNode;
            visitingNode = mission.GetLastVisitedNode;
            if(firstNode != null)
            {
                curNode.gameObject.SetActive(true);
            }
            
            StartCoroutine(SetupNodeTransitionMinimap(curNode));
        }

        private IEnumerator SetupNodeTransitionMinimap(MissionNodeObject startNode)
        {
            yield return new WaitForSeconds(0f);
            if (nodeObjectList.Count == 0)
            {
                nodeObjectList.Add(curNode);
                curNode.InitBaseNode(firstNode);
            }
            if (startNode.GetBaseMissionNode.ConnectedNode != null)
            {
                int xForward = 0;
                int yForward = 0;
                foreach (BaseMissionNode connectNode in startNode.GetBaseMissionNode.ConnectedNode)
                {
                    xForward = 0;
                    yForward = 0;
                    if (!allConnectedNodeList.Contains(connectNode))
                    {
                        TransitionDirection direction = missionNodeTransitionManager.CalculateTransitionDirect(startNode.GetBaseMissionNode, connectNode);
                        switch (direction)
                        {
                            case TransitionDirection.Forward_Z:
                                path = pathNode_x;
                                yForward = 70; //left
                                break;
                            case TransitionDirection.MinusForward_Z:
                                path = pathNode_x;
                                yForward = -70; //right
                                break;
                            case TransitionDirection.MinusRight_X:
                                path = pathNode_z;
                                xForward = -70; //back
                                break;
                            case TransitionDirection.Right_X:
                                path = pathNode_z;
                                xForward = 70; //front
                                break;
                        }

                        MissionPathObject newPath = Instantiate(path, nodeField.transform);
                        RectTransform rect = nodeObjectList[runningIndex].GetComponent<RectTransform>();
                        newPath.GetComponent<RectTransform>().localPosition = new Vector3(
                            rect.localPosition.x + xForward, rect.localPosition.y + yForward, rect.localPosition.z);
                        newPath.gameObject.SetActive(true);
                        newPath.transform.SetAsLastSibling();

                        MissionNodeObject newDun = Instantiate(dungeonNode, nodeField.transform);
                        RectTransform rectDun = newPath.GetComponent<RectTransform>();
                        newDun.GetComponent<RectTransform>().localPosition = new Vector3(
                            rectDun.localPosition.x + xForward, rectDun.localPosition.y + yForward, rectDun.transform.localPosition.z);
                        newDun.gameObject.SetActive(true);

                        if(connectNode is LootMissionNode)
                        {
                            newDun.GetNodeIcon.sprite = lootIcon;
                        }
                        if (connectNode is CombatMissionNode)
                        {
                            newDun.GetNodeIcon.sprite = dungeonIcon;
                        }
                        if (mission.GetFirstNode == mission.GetLastVisitedNode)
                        {
                            curNode.PlayVisitingAnimation();
                        }
                        else if(connectNode == mission.GetLastVisitedNode)
                        {
                            curNodeVisit = newDun;
                        }

                        newDun.Init(connectNode);
                        newDun.transform.SetAsLastSibling();

                        startNode.AddPathConnectToThisNode(newPath);
                        newPath.InitConnectedNode(startNode, newDun);

                        unConnectNodeList.Enqueue(newDun);
                        nodeObjectList.Add(newDun);
                    }
                }

                allConnectedNodeList.Add(startNode.GetBaseMissionNode);

                runningIndex++;
                if (unConnectNodeList.Count != 0)
                {
                    MissionNodeObject nextNode = unConnectNodeList.Dequeue();
                    StartCoroutine(SetupNodeTransitionMinimap(nextNode));
                }
                else
                {
                    //ColorfulLogger.LogWithColor("No node has to find its connect", Color.red);
                    yield return SetCurrentNodeAnim();
                    curNode.NodeReveal();
                    foreach (MissionPathObject path in curNode.GetPathList)
                    {
                        path.PathReveal();
                    }
                }
            }
            //curNode.InitBaseNode(visitingNode);
            //curNode.GetComponent<Animator>().Play("NodeObjectVisittingState");
        }

        private IEnumerator SetCurrentNodeAnim()
        {
            if(curNodeVisit != null)
            {
                curNodeVisit.PlayVisitingAnimation();
                Vector3 curVisitingNodePos = (curNodeVisit.GetComponent<RectTransform>().localPosition - curNode.GetComponent<RectTransform>().localPosition);
                //Debug.Log("X: " + curVisitingNodePos.x + "Y: " + curVisitingNodePos.y);
                float xforward = (float)(curVisitingNodePos.x / 140f);
                float xback = (float)(curVisitingNodePos.x / 140f);
                float yleft = Mathf.Abs((float)(curVisitingNodePos.y / 140f));
                float yright = Mathf.Abs((float)(curVisitingNodePos.y / 140f));
                //Debug.Log("Front: " + xforward + " Back" + xback + " LEFT" + yleft + " RIGHT" + yright);
                //float ymul = (curVisitingNodePos.y / 140f);
                for (int i = 0; i < xforward; i++)
                {
                    nodeField.GetComponent<RectTransform>().localPosition =
                    new Vector3(nodeField.GetComponent<RectTransform>().localPosition.x - 100,
                    nodeField.GetComponent<RectTransform>().localPosition.y - 40, 0);
                }
                //for (int i = 0; i < xback; i++)
                //{
                //    nodeField.GetComponent<RectTransform>().localPosition =
                //    new Vector3(nodeField.GetComponent<RectTransform>().localPosition.x + 100,
                //    nodeField.GetComponent<RectTransform>().localPosition.y + 40, 0);
                //}
                if (curNodeVisit.GetComponent<RectTransform>().localPosition.y > curNode.GetComponent<RectTransform>().localPosition.y)
                {
                    //Debug.Log("LEft");
                    for (int i = 0; i < yleft; i++)
                    {
                        nodeField.GetComponent<RectTransform>().localPosition =
                            new Vector3(nodeField.GetComponent<RectTransform>().localPosition.x + 100,
                            nodeField.GetComponent<RectTransform>().localPosition.y - 60, 0);
                    }
                }
                else
                {
                    //Debug.Log("Right");
                    for (int i = 0; i < yright; i++)
                    {
                        nodeField.GetComponent<RectTransform>().localPosition =
                            new Vector3(nodeField.GetComponent<RectTransform>().localPosition.x - 100,
                            nodeField.GetComponent<RectTransform>().localPosition.y + 60, 0);
                    }
                }
            }
            yield return null;
        }
    }
}
