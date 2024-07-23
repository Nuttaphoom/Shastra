using CustomYieldInstructions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class MissionNodeManager : MonoBehaviour 
    {
        [SerializeField]
        private BaseMissionNode _currentDungeonNode;

        [SerializeField]
        private Transform _cameraPivot;

        [SerializeField]
        private MissionNodeTransitionManager _missionNodeTransitionManager;

        [SerializeField]
        private MissionPartyAnimationManager _missionPartyAnimation;

        [SerializeField]
        private GameObject allCompanionUI;

        #region GETTER

        public MissionNodeTransitionManager MissionNodeTransitionManager { get { return _missionNodeTransitionManager; } }
        #endregion
        private void Awake()
        {
            _missionNodeTransitionManager.Init(); 
        }
        public IEnumerator SetUpDungeonCoroutine(BaseMissionNode firstNodeToStart)
        {
            yield return VisiteNextNode(firstNodeToStart);
        }

       
        public IEnumerator VisiteNextNode(BaseMissionNode nodeToVisit )
        {

            //first time dungeon node is init 
            if (_currentDungeonNode != null)
            {

                //check if the next node is connected
                if (!_currentDungeonNode.IsConnectedNode(nodeToVisit))
                {
                    goto End;
                }

                yield return _currentDungeonNode.OnLeaveThisNode();

                //Clear up transition node object
                _missionNodeTransitionManager.ClearDungeonNodeTransition();


                List<IEnumerator> _allIEs = new List<IEnumerator>();

                _allIEs.Add(OnCameraMoveToNode(nodeToVisit));
                _allIEs.Add(_missionPartyAnimation.OnCharacterMoveToNode(nodeToVisit));

                yield return new WaitAll(this, _allIEs.ToArray());



            }

            _cameraPivot.transform.position = nodeToVisit.transform.position;
            allCompanionUI.transform.position = new Vector3(nodeToVisit.transform.position.x, 0f, nodeToVisit.transform.position.z);
            //


            _currentDungeonNode = nodeToVisit;

            //yield return until transition visual is done 

            yield return _currentDungeonNode.OnVisiteThisNode() ;

            //Set up transition 
            foreach (var node in _currentDungeonNode.ConnectedNode)
                yield return _missionNodeTransitionManager.SetUpDungeonNodeTransition(_currentDungeonNode,node );

            /// If VisiteNextNode is interrupted with Loading new scene in _currentDungeonNode.OnVisiteThisNode
            /// The rest of the code below will not be called, 



        End:
            yield return null; 
        }

        private IEnumerator OnCameraMoveToNode(BaseMissionNode nodeToVisit)
        {
            Vector3 prevCamPos = _cameraPivot.position;
            float progression = 0;

            while (progression < 1)
            {

                _cameraPivot.transform.position = Vector3.Lerp(prevCamPos, nodeToVisit.transform.position, progression);
                progression += Time.deltaTime / 2;

                yield return null;

            }
        }
    }

    
}
