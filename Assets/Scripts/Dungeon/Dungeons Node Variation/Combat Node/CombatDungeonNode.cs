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
            yield return base.OnVisiteThisNode() ;

            PersistentSceneLoader.Instance.LoadGeneralScene(_combatSceneData);
        }
    }
}
