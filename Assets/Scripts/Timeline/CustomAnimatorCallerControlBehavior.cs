using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Vanaring
{
    [Serializable]
    public class CustomAnimatorCallerControlBehavior : PlayableBehaviour
    {
        [SerializeField]
        private string _triggerID;

        private bool firstTime = false;

        private CombatEntity _entity ;

        public override void ProcessFrame(Playable playable, FrameData ifo, object playerData)
        {
            if (!firstTime)
            {
                firstTime = true;
                _entity = (playerData as GameObject).GetComponent<CombatEntity>() ;

                if (_entity == null)
                {
                    throw new Exception("Entity is null"); 
                }

                _entity.StartCoroutine(_entity.GetComponent<CombatEntityAnimationHandler>().PlayTriggerAnimation(_triggerID)); 
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
        }
    }
}
