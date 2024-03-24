using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class DungeonManager : MonoBehaviour, ISceneLoaderWaitForSignal 
    {
        [SerializeField]
        private BaseDungeonNode _firstDungeonNode;

        [SerializeField]
        private BaseDungeonNode _currentDungeonNode;

        [SerializeField]
        private Transform _cameraPivot;

        private void Awake()
        {
            StartCoroutine(SetUpDungeon());
        } 

        private IEnumerator SetUpDungeon()
        {
            yield return VisiteNextNode(_firstDungeonNode);
        } 

        public IEnumerator VisiteNextNode(BaseDungeonNode nodeToVisit)
        {
            if (_currentDungeonNode != null)
            {

                //check if the next node is connected
                if (!_currentDungeonNode.IsConnectedNode(nodeToVisit))
                {

                    goto End;
                }
                yield return _currentDungeonNode.OnLeaveThisNode();
            }


            _currentDungeonNode = nodeToVisit;

            Vector3 prevCamPos = _cameraPivot.position;
            float progression = 0; 

            while (progression < 1)
            {

                _cameraPivot.transform.position = Vector3.Lerp(prevCamPos, nodeToVisit.transform.position, progression);
                progression += Time.deltaTime / 2 ;

                yield return null; 

            }

            _cameraPivot.transform.position = nodeToVisit.transform.position; 

            //yield return until transition visual is done 
             

            yield return _currentDungeonNode.OnVisiteThisNode() ;


        End:
            yield return null; 
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            yield return SetUpDungeon() ; 
        }
    }
}
