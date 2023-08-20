using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
namespace Vanaring_DepaDemo
{
    [RequireComponent(typeof(BotBehaviorHandler))]

    public class AIEntityBrain : BaseEntityBrain
    {
        BotBehaviorHandler _botBehaviorHandler;

        [SerializeField] 
        private CombatEntity _combatEntity; 
        protected override void Awake()
        {
            base.Awake(); 
            if (! TryGetComponent(out _botBehaviorHandler))
                throw new System.Exception("BotBehaviorHandler hasn't been assigned");

            StartCoroutine(_botBehaviorHandler.CalculateNextBehavior()); 

        }
        public override IEnumerator GetAction()
        {
            BotBehaviorSO.ActionData actionData = _botBehaviorHandler.GetBehaviorEffect();
            if (_combateEntity == null)
                Debug.Log("combta entity is null"); 
 
            _combatEntity.SpellCaster.ModifyEnergy(_combateEntity, actionData.EnergyCost.Side, actionData.EnergyCost.Amount);

            yield return new WaitForSeconds(1.0f);

            foreach (RuntimeEffectFactorySO eff in actionData.Effects) {  

                yield return TargetSelectionFlowControl.Instance.InitializeTargetSelectionScheme(_combateEntity, eff, true) ;
                RuntimeEffectFactorySO factory ;
                List<CombatEntity> selectedTarget ;
   
                (factory, selectedTarget) = TargetSelectionFlowControl.Instance.GetLatestAction();
                     
                IEnumerator coroutine =   factory.Factorize(selectedTarget);

                while (coroutine.MoveNext())
                {
                    if (coroutine.Current != null && coroutine.Current.GetType().IsSubclassOf(typeof(RuntimeEffect)))
                    {
                        yield return coroutine.Current as RuntimeEffect;
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

        public override IEnumerator TakeControlSoftLeave()
        {
            //calculate next behavior 
            yield return _botBehaviorHandler.CalculateNextBehavior();
         
            
        }

        public override IEnumerator AfterGetAction()
        {
            yield return null; 
        }

        public override IEnumerator OnDeadVisualClear()
        {
            _botBehaviorHandler.DestroyTelegraphyVFX();

            yield return null;
        }
    }
}
