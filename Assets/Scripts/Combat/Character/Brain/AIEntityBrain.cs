using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
namespace Vanaring_DepaDemo
{
    [RequireComponent(typeof(BotBehaviorHandler))]

    public class AIEntityBrain : BaseEntityBrain
    {
        BotBehaviorHandler _botBehaviorHandler;

        protected override void Awake()
        {
            base.Awake(); 
            if (! TryGetComponent(out _botBehaviorHandler))
                throw new System.Exception("BotBehaviorHandler hasn't been assigned");

            _botBehaviorHandler.CalculateNextBehavior(); 

        }
        public override IEnumerator GetAction()
        {
            //Debug.Log("GetAction!!!!!!");
            yield return new    WaitForSeconds(3.0f); 
            foreach (RuntimeEffectFactorySO eff in _botBehaviorHandler.GetBehaviorEffect())
            {
                yield return TargetSelectionFlowControl.Instance.InitializeTargetSelectionScheme(_combateEntity, eff, true);
                RuntimeEffectFactorySO factory ;
                List<CombatEntity> selectedTarget ;
                (factory, selectedTarget) = TargetSelectionFlowControl.Instance.GetLatestAction();
                 
                IEnumerator coroutine =   factory.Factorize(selectedTarget);

                while (coroutine.MoveNext())
                {
                    if (coroutine.Current != null && coroutine.Current.GetType().IsSubclassOf(typeof(RuntimeEffect)))
                    {
                        yield return  coroutine.Current as RuntimeEffect;
                    }
                }
            }
        }

        public override IEnumerator TurnEnter()
        {
            yield return null; 
        }

        public override IEnumerator TurnLeave()
        {
            //calculate next behavior 
            Debug.Log("TurnLeave!");
            yield return _botBehaviorHandler.CalculateNextBehavior(); 

            yield return null; 
        }

        public override IEnumerator TakeControl()
        {
            //Debug.Log("TakeControl!!!!!!");
            yield return null;
        }

        public override IEnumerator TakeControlLeave()
        {
            yield return null;
        }
    }
}
