using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class CombatBacklogDisplayer : MonoBehaviour
    {
        public void DisplayPerformedActionBacklog(EntityActionPair entityActionPair)
        {
            CombatEntity entity = entityActionPair.Actor;
            ActorAction action = entityActionPair.PerformedAction;
            string comment = action.GetActionComment().GetComment(entity) ;
            if (comment == null || comment == "")
                return; 
            
           
            ColorfulLogger.LogWithColor(comment, Color.cyan) ;
        }
    }
}
