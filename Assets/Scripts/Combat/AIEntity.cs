using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    [RequireComponent(typeof(AIBehaviorHandler))]
    [RequireComponent (typeof(CombatEntityAnimationHandler))]
    public class AIEntity : CombatEntity
    {
        AIBehaviorHandler aiBehaviorHandler;

        
        protected override void Awake()
        {
            base.Awake();
            aiBehaviorHandler = GetComponent<AIBehaviorHandler>();

            //StartCoroutine(_botBehaviorHandler.CalculateNextBehavior());
        }

        public override IEnumerator InitializeEntityIntoCombat()
        {
            CombatEntityAnimationHandler.InstantlyHideVisualMesh();
            yield return CombatEntityAnimationHandler.PlaySpawnVisualEffectCoroutine(); 

        }
        public override IEnumerator TurnEnter()
        {
            yield return base.TurnEnter();
        }

        public override IEnumerator TurnLeave()
        {
            //Calculate next action
            yield return base.TurnLeave();
        }


        public override IEnumerator GetAction()
        {
            if (_ailmentHandler.DoesAilmentOccur())
            {
                yield return _ailmentHandler.AlimentControlGetAction();
            }
            else
            {
                yield return aiBehaviorHandler.GetNextAction();
            }
        }

        public override IEnumerator TakeControl()
        {
            yield return base.TakeControl(); 
        
        }

        public override IEnumerator TakeControlLeave()
        {
            yield return base.TakeControlLeave();  ;
        }
         
        
        public override void ApplyOverflow()
        {
            base.ApplyOverflow();

            aiBehaviorHandler.OnBehaviorOwnerStun(); 
        }
    }
}
