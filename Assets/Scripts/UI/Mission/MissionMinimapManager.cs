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
        private List<Vector3> nodeRectPosition = new List<Vector3>();
        private MissionNodeTransitionManager missionNodeTransitionManager;
        private MissionNodeEnvironment mission;

        [SerializeField] private MissionSetupHandler setUpHandler;
        [SerializeField] private GameObject nodeField;
        [SerializeField] private GameObject curNode;
        [SerializeField] private GameObject dungeonNode;
        [SerializeField] private GameObject pathNode_x;
        [SerializeField] private GameObject pathNode_z;
        private GameObject focusNode;
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
                curNode.SetActive(true);
            }
            nodeRectPosition.Add(Vector3.zero);
            focusNode = curNode;
            StartCoroutine(SetupNodeTransitionMinimap(firstNode, nodeRectPosition[runningIndex]));
        }

        private IEnumerator SetupNodeTransitionMinimap(BaseMissionNode startNode, Vector3 nodePos)
        {
            yield return new WaitForSeconds(2.0f);
            if (startNode.ConnectedNode != null)
            {
                int xForward = 0;
                int yForward = 0;
                Debug.Log("Connected node: " + startNode.ConnectedNode.Count);
                foreach (BaseMissionNode connectNode in startNode.ConnectedNode)
                {
                    if (!allConnectedNodeList.Contains(connectNode))
                    {
                        TransitionDirection direction = missionNodeTransitionManager.CalculateTransitionDirect(startNode, connectNode);
                        Debug.Log(direction);
                        switch (direction)
                        {
                            case TransitionDirection.Forward_Z:
                                //Debug.Log("ForwZ");
                                yForward = 70;
                                break;
                            case TransitionDirection.MinusForward_Z:
                                //Debug.Log("-ForwZ");
                                yForward = -70;
                                break;
                            case TransitionDirection.MinusRight_X:
                                //Debug.Log("-ForwX");
                                xForward = -70;
                                break;
                            case TransitionDirection.Right_X:
                                //Debug.Log("ForwX");
                                xForward = 70;
                                break;
                        }

                        

                        GameObject newPath = Instantiate(pathNode_z, nodeField.transform);
                        RectTransform rect = focusNode.GetComponent<RectTransform>();
                        newPath.GetComponent<RectTransform>().localPosition = new Vector3(
                            rect.localPosition.x + xForward, rect.localPosition.y + yForward, rect.localPosition.z);
                        newPath.SetActive(true);
                        newPath.transform.SetAsLastSibling();
                        ColorfulLogger.LogWithColor("Create Path", Color.green);

                        GameObject newDun = Instantiate(dungeonNode, nodeField.transform);
                        RectTransform rectDun = newPath.GetComponent<RectTransform>();
                        newDun.GetComponent<RectTransform>().localPosition = new Vector3(
                            rectDun.localPosition.x + xForward, rectDun.localPosition.y + yForward, rectDun.transform.localPosition.z);
                        newDun.SetActive(true);
                        newDun.transform.SetAsLastSibling();
                        ColorfulLogger.LogWithColor("Create Dungeon Node", Color.green);

                        focusNode = newDun;

                        unConnectNodeList.Enqueue(connectNode);
                        nodeRectPosition.Add(newDun.GetComponent<RectTransform>().localPosition);
                        runningIndex++;

                    }
                    
                }

                allConnectedNodeList.Add(startNode);
                
                if(unConnectNodeList.Count != 0)
                {
                    Debug.Log("Prepare in queue: " + unConnectNodeList.Count);
                    BaseMissionNode nextNode = unConnectNodeList.Dequeue();
                    StartCoroutine(SetupNodeTransitionMinimap(nextNode, nodeRectPosition[runningIndex]));
                }
                else
                {
                    ColorfulLogger.LogWithColor("No node has to find its connect", Color.red);
                }


            }
        }

        
    }
}
