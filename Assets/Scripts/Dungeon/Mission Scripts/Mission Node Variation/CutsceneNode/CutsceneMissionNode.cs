using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CutsceneMissionNode : BaseMissionNode
    {
        [SerializeField]
        private DungeonCutsceneContainerSO _cutsceneData;

        protected override IEnumerator OnVisiteThisNodeFirstTime()
        {
            yield return base.OnVisiteThisNodeFirstTime();
            yield return new WaitForSeconds(1.25f);

            PersistentSceneLoader.Instance.LoadGeneralScene(_cutsceneData.GetCutsceneSceneDataSO);
        }
    }

     
}
