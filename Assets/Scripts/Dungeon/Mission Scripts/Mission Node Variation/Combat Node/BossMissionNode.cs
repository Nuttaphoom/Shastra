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

            Debug.Log("TODO : Combat should talk with this function so that we can know the result of the combat , shouldn't be assume player get back and visiste this node again mean complete this node") ;   

            Debug.Log("The dungeon is end");
        }

    }

     
}
