using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class ExitMissionNode : BaseMissionNode
    {
        public override IEnumerator OnVisiteThisNodeFirstTime()
        {
            yield return base.OnVisiteThisNodeFirstTime();

            MissionCompleteStatus status = new MissionCompleteStatus()
            {
                CompleteDungeon = true,
                ExitDungeon = true ,
            }; 
            MissionManagerSingleton.Instance.ExitDungeon(status);
            
        }

         
    }
}
