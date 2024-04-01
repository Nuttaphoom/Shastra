using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class ExitMissionNode : BaseMissionNode
    {
        public override IEnumerator OnVisiteThisNodeFirstTime()
        {
            ColorfulLogger.LogWithColor("Dungeon is complete", Color.green) ; 
            yield return null;
        }

         
    }
}
