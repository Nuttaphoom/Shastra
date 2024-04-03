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
            yield return new WaitForSeconds(2.0f);
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
