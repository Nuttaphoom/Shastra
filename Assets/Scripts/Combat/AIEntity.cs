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

            aiBehaviorHandler.SetEntity(this);
            //StartCoroutine(_botBehaviorHandler.CalculateNextBehavior());
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
            aiBehaviorHandler.CheckingCondition();
            aiBehaviorHandler.GetNextAction();
            //print("GetAction");
            //TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme();
            yield return null;
        }

        public override IEnumerator TakeControl()
        {
            //throw new NotImplementedException();
            yield return null;
        }

        public override IEnumerator TakeControlLeave()
        {
            aiBehaviorHandler.TakeControlLeave();
            yield return null;
        }

    }
}
