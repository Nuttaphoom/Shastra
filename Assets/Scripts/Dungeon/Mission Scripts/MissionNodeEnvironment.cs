using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Vanaring.MissionSetupHandler;

namespace Vanaring
{
    
    public class MissionNodeEnvironment : MonoBehaviour    
    {
        [SerializeField]
        private List<BaseMissionNode> _baseDungeonNode ;


        [Header("*** Use for debuging only ***")]
        ///Use for save/load system 
        [SerializeField]
        private List<NodeRuntimeData> _nodeRuntimeData ;

        [SerializeField]
        private SceneDataSO _TEST_Map_sceneData ;

        /// <summary>
        /// make sure to call this AFTER loading process.
        /// </summary>
        public BaseMissionNode GetLastVisitedNode
        {
            get
            {
                foreach (BaseMissionNode node in _baseDungeonNode)
                {
                    //Debug.Log(node.gameObject.name + " is " + node.visistationState);
                    if (node.IsCurrentlyVisiting)
                        return node; 
                }

                return _baseDungeonNode[0]; 
            }
        }

        public BaseMissionNode GetFirstNode
        {
            get
            {
                return _baseDungeonNode[0];
            }
        }

        public List<BaseMissionNode> GetAllDungeonNodes
        {
            get
            {
                return _baseDungeonNode;
            }
        }
         
        public List<NodeRuntimeData> CaptureEnvironmentData()
        {
            _nodeRuntimeData = new List<NodeRuntimeData>();
            for (int i = 0; i < _baseDungeonNode.Count; i++)
                _nodeRuntimeData.Add(_baseDungeonNode[i].CaptureNodeData() ) ;   
             
            return _nodeRuntimeData; 
        }

        public void RestoreEnvironmentData(List<NodeRuntimeData> state)
        {
            _nodeRuntimeData =  state;

            for (int i = 0; i < _baseDungeonNode.Count; i++)
            {
                _baseDungeonNode[i].RestoreNodeData(_nodeRuntimeData[i]) ;
            }
        }

        public void OnMissionExit()
        {
            for (int i = 0; i < _baseDungeonNode.Count; i++)
            {
                _baseDungeonNode[i].OnExitMission_ClearNodeData();
            } 

            
        }



    }
}
