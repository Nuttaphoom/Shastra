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
            yield return base.OnVisiteThisNodeFirstTime();

            PersistentSceneLoader.Instance.CreateLoaderDataUser<CombatDungeonNodeLoaderData>("CombatDungeonNodeDataUser", _combatDungeonNodeLoaderData); 
            PersistentSceneLoader.Instance.LoadGeneralScene(_combatSceneData);

        }
    }

    [Serializable]
    public struct CombatDungeonNodeLoaderData
    {
        [SerializeField]
        public EntityLoaderPoolSO EnemyLoaderPool ; 
    }
}
