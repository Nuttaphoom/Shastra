using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Vanaring
{
    [RequireComponent(typeof(EnemyBehaviorHandler))]

    public class EnemyEntityBrain : BaseEntityBrain
    {
        EnemyBehaviorHandler _enemyBehaviorHandler;

        [SerializeField]
        private CombatEntity _combatEntity;
        protected override void Awake()
        {
            base.Awake();
            if (!TryGetComponent(out _enemyBehaviorHandler))
                throw new System.Exception("BotBehaviorHandler hasn't been assigned");

            StartCoroutine(_enemyBehaviorHandler.CalculateNextBehavior());

        }
        public override IEnumerator GetAction()
        {
            BotBehaviorSO.ActionData actionData = _enemyBehaviorHandler.GetBehaviorEffect();
            if (_combateEntity == null)
                Debug.Log("combta entity is null");

            _combatEntity.SpellCaster.ModifyEnergy(_combateEntity, actionData.EnergyCost.Side, actionData.EnergyCost.Amount);

            yield return new WaitForSeconds(1.0f);

            foreach (RuntimeEffectFactorySO eff in actionData.Effects)
            {

                yield return TargetSelectionFlowControl.Instance.InitializeTargetSelectionScheme(_combateEntity, eff, true);
                RuntimeEffectFactorySO factory;
                List<CombatEntity> selectedTarget;

                (factory, selectedTarget) = TargetSelectionFlowControl.Instance.GetLatestAction();

                IEnumerator coroutine = factory.Factorize(selectedTarget);

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
            yield return _enemyBehaviorHandler.CalculateNextBehavior();
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
            yield return null;


        }

        public override IEnumerator AfterGetAction()
        {
            yield return null;
        }

        public override IEnumerator OnDeadVisualClear()
        {
            _enemyBehaviorHandler.DestroyTelegraphyVFX();

            yield return null;
        }
    }
}
