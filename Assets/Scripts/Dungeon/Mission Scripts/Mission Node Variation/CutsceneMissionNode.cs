using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vanaring.Assets.Scripts.Utilities.Cutscene_System;

namespace Vanaring
{
    public class CutsceneMissionNode : BaseMissionNode
    {
        [SerializeField]
        private CutsceneContainerSO _cutsceneData;
          
        protected override IEnumerator OnVisiteThisNodeFirstTime()
        {
            yield return base.OnVisiteThisNodeFirstTimeOnMission();
            yield return new WaitForSeconds(1.25f);

            PersistentSceneLoader.Instance.LoadGeneralScene(_cutsceneData.GetCutsceneSceneDataSO) ;
        }
    }

     
}
