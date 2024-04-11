using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class ExitMissionNode : BaseMissionNode
    {
        protected override IEnumerator OnVisiteThisNodeFirstTimeOnMission()
        {
            yield return base.OnVisiteThisNodeFirstTimeOnMission();

            MissionCompleteStatus status = new MissionCompleteStatus()
            {
                CompleteDungeon = true,
                ExitDungeon = true ,
            };

            DungeonManagerSingleton.Instance.ExitMission(status); 
            //MissionManagerSingleton.Instance.ExitDungeon(status);
            
        }

         
    }
}
