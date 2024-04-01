using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    public class MissionMinimapManager : MonoBehaviour
    {
        
        private BaseMissionNode firstNode;
        private List<BaseMissionNode> nodeList = new List<BaseMissionNode>();
        private List<BaseMissionNode> allConnectedNodeList = new List<BaseMissionNode>();
        private Queue<BaseMissionNode> unConnectNodeList = new Queue<BaseMissionNode>();

        [SerializeField] private MissionNodeEnvironment mission;
        [SerializeField] private MissionNodeTransitionManager missionNodeTransitionManager;
        [SerializeField] private MissionSetupHandler setUpHandler;
        [SerializeField] private GameObject nodeField;
        [SerializeField] private GameObject curNode;
        [SerializeField] private GameObject dungeonNode;
        [SerializeField] private GameObject pathNode_x;
        [SerializeField] private GameObject pathNode_z;
        private GameObject focusNode;

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
            nodeList = mission.GetAllDungeonNodes;
            firstNode = mission.GetFirstNode;
            if(firstNode != null)
            {
                curNode.SetActive(true);
            }
            focusNode = curNode;
            StartCoroutine(SetupNodeTransitionMinimap(firstNode));
        }

        private void Update()
        {
            if(mission == null)
            {
                mission = FindAnyObjectByType<MissionNodeEnvironment>();
            }
        }

        private IEnumerator SetupNodeTransitionMinimap(BaseMissionNode startNode)
        {
            if(startNode.ConnectedNode != null)
            {
                int xForward = 0;
                int yForward = 0;
                List<BaseMissionNode> connectNodeList = startNode.ConnectedNode;
                foreach (BaseMissionNode connectNode in connectNodeList)
                {
                    if (!allConnectedNodeList.Contains(connectNode))
                    {
                        unConnectNodeList.Enqueue(connectNode);
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

                        //rect.localPosition = new Vector3(
                        //    newPath.GetComponent<RectTransform>().localPosition.x, newPath.GetComponent<RectTransform>().localPosition.y, newPath.GetComponent<RectTransform>().localPosition.z);
                        //rectDun.localPosition = new Vector3(
                        //    newDun.GetComponent<RectTransform>().localPosition.x, newPath.GetComponent<RectTransform>().localPosition.y, newPath.GetComponent<RectTransform>().localPosition.z);

                    }
                    
                }
                allConnectedNodeList.Add(startNode);
                if(unConnectNodeList.Count != 0)
                {
                    Debug.Log(unConnectNodeList.Count);
                    BaseMissionNode nextNode = unConnectNodeList.Dequeue();
                    StartCoroutine(SetupNodeTransitionMinimap(nextNode));
                }
                else
                {
                    ColorfulLogger.LogWithColor("No node has to find its connect", Color.red);
                }
                
            }
            
            yield return new WaitForSeconds(1.0f);
            
        }
    }
}
