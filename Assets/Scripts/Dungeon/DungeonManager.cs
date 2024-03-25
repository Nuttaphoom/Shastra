using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class DungeonManager : MonoBehaviour 
    {
        [SerializeField]
        private BaseDungeonNode _currentDungeonNode;

        [SerializeField]
        private Transform _cameraPivot;

    

        public IEnumerator SetUpDungeonCoroutine(BaseDungeonNode firstNodeToStart)
        {
            yield return VisiteNextNode(firstNodeToStart);
        } 

        public IEnumerator VisiteNextNode(BaseDungeonNode nodeToVisit )
        {
            if (_currentDungeonNode != null)
            {
                //check if the next node is connected
                if (!_currentDungeonNode.IsConnectedNode(nodeToVisit))
                {
                    goto End;
                }
                yield return _currentDungeonNode.OnLeaveThisNode();

                Vector3 prevCamPos = _cameraPivot.position;
                float progression = 0;

                while (progression < 1)
                {

                    _cameraPivot.transform.position = Vector3.Lerp(prevCamPos, nodeToVisit.transform.position, progression);
                    progression += Time.deltaTime / 2;

                    yield return null;

                }

            }

            _cameraPivot.transform.position = nodeToVisit.transform.position;


            _currentDungeonNode = nodeToVisit;

            //yield return until transition visual is done 

            yield return _currentDungeonNode.OnVisiteThisNode() ;


        End:
            yield return null; 
        }

        
    }
}
