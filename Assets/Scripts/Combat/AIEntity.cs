using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public class AIEntity : CombatEntity
    {
        [SerializeField]
        AIBehaviorHandler aiBehaviorHandler;
        protected override void Awake()
        {
            base.Awake();

            aiBehaviorHandler.SetEntity(this);
            //StartCoroutine(_botBehaviorHandler.CalculateNextBehavior());
        }

        public override IEnumerator TurnEnter()
        {
            yield return null;
        }

        public override IEnumerator TurnLeave()
        {
            //Calculate next action
            yield return null; 
        }


        public override IEnumerator GetAction()
        {
            print("GetAction");
            //TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme();
            throw new NotImplementedException();
        }

        public override IEnumerator TakeControl()
        {
            //throw new NotImplementedException();
            print("TakeControl");
            aiBehaviorHandler.CheckingCondition();
            //aiBehaviorHandler.GetNextAction();
            yield return aiBehaviorHandler.GetNextAction();
        }

        public override IEnumerator TakeControlLeave()
        {
            print("TakeControlLeave");
            throw new NotImplementedException();
        }

    }
}
