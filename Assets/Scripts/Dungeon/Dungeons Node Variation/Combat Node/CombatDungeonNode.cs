using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CombatDungeonNode : BaseDungeonNode
    {
        [SerializeField]
        private SceneDataSO _combatSceneData; 
        public override IEnumerator OnVisiteThisNode()
        {
            if (!_isVisited)
            {

                _isVisited = true;      
                PersistentSceneLoader.Instance.LoadGeneralScene(_combatSceneData);
            }



            yield return SetUpNodeTransitions();

        }
    }
}
