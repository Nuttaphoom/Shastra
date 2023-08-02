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

            StartCoroutine(_botBehaviorHandler.CalculateNextBehavior()); 

        }
        public override IEnumerator GetAction()
        {
            yield return new WaitForSeconds(1.5f); 
            foreach (RuntimeEffectFactorySO eff in _botBehaviorHandler.GetBehaviorEffect())
            {
                yield return TargetSelectionFlowControl.Instance.InitializeTargetSelectionScheme(_combateEntity, eff, true) ;
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
            Debug.Log("calcualte next behavior for " + gameObject.name); 
            //calculate next behavior 
            yield return _botBehaviorHandler.CalculateNextBehavior(); 

            yield return null; 
        }

        public override IEnumerator TakeControl()
        {
            yield return null;
        }

        public override IEnumerator TakeControlLeave()
        {
            yield return null;
        }
    }
}
