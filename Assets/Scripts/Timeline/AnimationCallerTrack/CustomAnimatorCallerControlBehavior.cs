using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

                if (playerData is Transform)
                    _entity = (playerData as Transform).gameObject.GetComponent<CombatEntity>();

                else if (playerData is GameObject)
                    _entity = (playerData as GameObject).gameObject.GetComponent<CombatEntity>();
                else
                {
                    throw new Exception("_entity can not be assigned");
                }


                if (_entity != null)
                    _entity.StartCoroutine(_entity.GetComponent<CombatEntityAnimationHandler>().PlayTriggerAnimation(_triggerID));
                else if ((playerData as GameObject).TryGetComponent(out Animator animator)){
                    animator.SetTrigger(_triggerID); 
                }else
                {
                    throw new Exception("" + playerData.ToString() + "didn't has animator attached to it") ;
                }
                    
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
        }
    }
}
