using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    public class MissionMinimapManager : MonoBehaviour
    {
        
        private BaseMissionNode firstNode;
        private List<BaseMissionNode> allConnectedNodeList = new List<BaseMissionNode>();
        private Queue<BaseMissionNode> unConnectNodeList = new Queue<BaseMissionNode>();
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
            
            StartCoroutine(SetupNodeTransitionMinimap(firstNode));
        }

        private IEnumerator SetupNodeTransitionMinimap(BaseMissionNode startNode)
        {
            yield return new WaitForSeconds(0f);
            if (startNode.ConnectedNode != null)
            {
                int xForward = 0;
                int yForward = 0;
                Debug.Log("Connected node: " + startNode.ConnectedNode.Count);
                if(nodeObjectList.Count == 0)
                {
                    nodeObjectList.Add(curNode);
                }
                foreach (BaseMissionNode connectNode in startNode.ConnectedNode)
                {
                    xForward = 0;
                    yForward = 0;
                    //yield return new WaitForSeconds(.2f);
                    if (!allConnectedNodeList.Contains(connectNode))
                    {
                        TransitionDirection direction = missionNodeTransitionManager.CalculateTransitionDirect(startNode, connectNode);
                        Debug.Log(direction);
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
                        ColorfulLogger.LogWithColor("Create Path", Color.green);
                        //yield return new WaitForSeconds(.2f);

                        MissionNodeObject newDun = Instantiate(dungeonNode, nodeField.transform);
                        RectTransform rectDun = newPath.GetComponent<RectTransform>();
                        newDun.GetComponent<RectTransform>().localPosition = new Vector3(
                            rectDun.localPosition.x + xForward, rectDun.localPosition.y + yForward, rectDun.transform.localPosition.z);
                        newDun.gameObject.SetActive(true);
                        newDun.Init(connectNode);
                        newDun.transform.SetAsLastSibling();
                        ColorfulLogger.LogWithColor("Create Dungeon Node", Color.green);

                        //newPath.InitConnectedNode(startNode, newDun);
                        unConnectNodeList.Enqueue(connectNode);
                        nodeObjectList.Add(newDun);
                    }
                }

                allConnectedNodeList.Add(startNode);

                runningIndex++;
                if (unConnectNodeList.Count != 0)
                {
                    Debug.Log("Prepare in queue: " + unConnectNodeList.Count);
                    BaseMissionNode nextNode = unConnectNodeList.Dequeue();
                    StartCoroutine(SetupNodeTransitionMinimap(nextNode));
                }
                else
                {
                    ColorfulLogger.LogWithColor("No node has to find its connect", Color.red);
                }


            }
        }

        
    }
}
