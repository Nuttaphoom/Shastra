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

        protected override IEnumerator OnVisiteThisNodeFirstTimeOnMission()
        {
            yield return base.OnVisiteThisNodeFirstTimeOnMission();
            yield return new WaitForSeconds(1.25f) ;

            PersistentSceneLoader.Instance.CreateLoaderDataUser<CombatDungeonNodeLoaderData>("CombatDungeonNodeDataUser", _combatDungeonNodeLoaderData); 
            PersistentSceneLoader.Instance.LoadGeneralScene(_combatSceneData);
        }
    }

    [Serializable]
    public struct CombatDungeonNodeLoaderData
    {
        [SerializeField]
        public EntityLoaderPoolSO EnemyLoaderPool ;

        [SerializeField]
        public List<EventRewardData> CombatRewards ;
 
    
    }
}
