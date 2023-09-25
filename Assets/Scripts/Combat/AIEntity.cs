using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring 
{
    public class AIEntity : CombatEntity
    {
        protected override void Awake()
        {
            base.Awake();

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
            throw new NotImplementedException();
        }

        public override IEnumerator TakeControl()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator TakeControlLeave()
        {
            throw new NotImplementedException();
        }

    }
}
