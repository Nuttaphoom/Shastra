using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class BossMissionNode : BaseMissionNode
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


        public override IEnumerator OnVisiteThisNode()
        {
            yield return base.OnVisiteThisNode();

            Debug.Log("The dungeon is end");
        }

    }

     
}
