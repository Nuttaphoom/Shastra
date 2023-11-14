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

            ColorfulLogger.LogWithColor(entity.CharacterSheet.CharacterName + " Perform " + action.GetDescription().FieldName, Color.cyan);
        }
    }
}
