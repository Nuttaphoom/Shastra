using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CombatDungeonNode : BaseDungeonNode
    {
        [SerializeField]
        private SceneDataSO _combatSceneData; 
         

        public override IEnumerator OnVisiteThisNodeFirstTime()
        {
            PersistentSceneLoader.Instance.LoadGeneralScene(_combatSceneData);

            yield return null; 
        }
    }
}
