using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CombatMissionNode : BaseMissionNode
    {
        [SerializeField]
        private SceneDataSO _combatSceneData;

        [SerializeField]
        private CombatDungeonNodeLoaderData _combatDungeonNodeLoaderData;
         
        public override IEnumerator OnVisiteThisNodeFirstTime()
        {
            PersistentSceneLoader.Instance.CreateLoaderDataUser<CombatDungeonNodeLoaderData>("CombatDungeonNodeDataUser", _combatDungeonNodeLoaderData); 
            PersistentSceneLoader.Instance.LoadGeneralScene(_combatSceneData);

            yield return null; 
        }
    }

    [Serializable]
    public struct CombatDungeonNodeLoaderData
    {
        [SerializeField]
        public EntityLoaderPoolSO EnemyLoaderPool ; 
    }
}
