using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{

   
    public abstract class BaseDungeonNode : MonoBehaviour
    {
        private bool _isVisited = false;

        [SerializeField]
        private List<BaseDungeonNode> _connectedNode ;

        #region GETTER 
        public List<BaseDungeonNode> ConnectedNode
        {
            get {
                if (_connectedNode == null)
                    throw new System.Exception("ConnectedNode of " + gameObject.name + " hasn't been assigned");  

                return _connectedNode; 
            }
        }

        #endregion
        public abstract IEnumerator OnVisiteThisNode();
    }
}
