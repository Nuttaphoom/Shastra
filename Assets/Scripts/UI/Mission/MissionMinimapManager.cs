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
        private List<BaseMissionNode> allConnectedNodeList = new List<BaseMissionNode>();
        private Queue<MissionNodeObject> unConnectNodeList = new Queue<MissionNodeObject>();
        private List<MissionNodeObject> nodeObjectList = new List<MissionNodeObject>();
        private MissionNodeTransitionManager missionNodeTransitionManager;
        private MissionNodeEnvironment mission;

        [SerializeField] private MissionSetupHandler setUpHandler;
        [SerializeField] private GameObject nodeField;
        [SerializeField] private MissionNodeObject curNode;
        [SerializeField] private MissionNodeObject dungeonNode;
        [SerializeField] private MissionPathObject pathNode_x;
        [SerializeField] private MissionPathObject pathNode_z;
        private GameObject focusNode;
        private MissionPathObject path;
        private int runningIndex = 0;

        [ContextMenu("Init Minimap")]

        private void Start()
        {
            setUpHandler.SubOnEnvironmentSetup(Init);
        }

        public void OnDisable()
        {
            setUpHandler.UnSubOnEnvironmentSetup(Init);
        }

        public void Init(Null n)
        {
            missionNodeTransitionManager = FindObjectOfType<MissionNodeManager>().MissionNodeTransitionManager; 
            mission = setUpHandler.DungeonEnvironment;
            firstNode = mission.GetFirstNode;
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
                                yForward = 70;
                                break;
                            case TransitionDirection.MinusForward_Z:
                                path = pathNode_x;
                                yForward = -70;
                                break;
                            case TransitionDirection.MinusRight_X:
                                path = pathNode_z;
                                xForward = -70;
                                break;
                            case TransitionDirection.Right_X:
                                path = pathNode_z;
                                xForward = 70;
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
                    ColorfulLogger.LogWithColor("No node has to find its connect", Color.red);
                    curNode.NodeReveal();
                    foreach (MissionPathObject path in curNode.GetPathList)
                    {
                        path.PathReveal();
                    }
                }


            }
        }

        
    }
}
