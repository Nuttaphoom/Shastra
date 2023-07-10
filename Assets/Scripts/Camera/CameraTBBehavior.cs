using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    public class CameraTBBehavior : MonoBehaviour
    {
        public void OnEnterPlayPhase()
        {
            //set up camera and target
        }

        public void ResetCamera()
        {
            StopAllCoroutines();
        }

        public void OnEnterSelectTargetSKillPhase()
        {
            //set up camera and target
        }

        public void OnExitSelectTargetSKillPhase()
        {
            //set up camera and target
        }

        public void OnSkillUse()
        {

        }
    }
}
