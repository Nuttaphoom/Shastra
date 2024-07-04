using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Vanaring
{
    [Serializable]
    public class CustomEntityMeshHiddenControlBehavior : PlayableBehaviour
    {
        private bool firstTime = false;
        private CombatEntity _entity;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (!firstTime)
            {
                firstTime = true;

                if (playerData is Transform)
                    _entity = (playerData as Transform).gameObject.GetComponent<CombatEntity>();
                else if (playerData is GameObject)
                    _entity = (playerData as GameObject).GetComponent<CombatEntity>();
                else
                {
                    throw new Exception("_entity cannot be assigned, playerData is " + playerData);
                }

                if (_entity != null)
                    _entity.GetComponent<CombatEntityAnimationHandler>().HideVisualMesh();
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);

            if (_entity != null)
                _entity.GetComponent<CombatEntityAnimationHandler>().ShowVisualMesh();
        }
    }
}