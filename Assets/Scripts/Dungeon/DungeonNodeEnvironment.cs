using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    [Serializable]
    public struct NodeRuntimeData
    {
        public bool IsVisited;
        public bool CurrentlyVisited; 
    }
    public class DungeonNodeEnvironment : MonoBehaviour, ISaveable  
    {
        
        [SerializeField]
        private List<BaseDungeonNode> _baseDungeonNode ;

        [Header("*** Use for debuging only ***")]
        ///Use for save/load system 
        [SerializeField]
        private List<NodeRuntimeData> _nodeRuntimeData ;

        [SerializeField]
        private SceneDataSO _TEST_Map_sceneData ;

        /// <summary>
        /// make sure to call this AFTER loading process.
        /// </summary>
        public BaseDungeonNode GetFirstNode
        {
            get
            {
                foreach (BaseDungeonNode node in _baseDungeonNode)
                {
                    if (node.IsCurrentlyVisiting)
                        return node; 
                }

                return _baseDungeonNode[0]; 
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                PersistentSceneLoader.Instance.LoadGeneralScene(_TEST_Map_sceneData); 
            }
        }
         
        public object CaptureState()
        {
            _nodeRuntimeData = new List<NodeRuntimeData>();
            for (int i = 0; i < _baseDungeonNode.Count; i++)
            {

                _nodeRuntimeData.Add(_baseDungeonNode[i].CaptureNodeData() ) ; 
                 
            }

            return _nodeRuntimeData; 

        }

        public void RestoreState(object state)
        {
            _nodeRuntimeData = (List<NodeRuntimeData>)state;

            for (int i = 0; i < _baseDungeonNode.Count; i++)
            {
                _baseDungeonNode[i].RestoreNodeData(_nodeRuntimeData[i]) ;
            }
        }

         
    }
}
