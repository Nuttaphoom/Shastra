using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UnityEngine;

namespace Vanaring_DepaDemo
{

    public class ControlableEntityBrain : BaseEntityBrain 
    {
        private CombatGraphicalHandler _combatGraphicalHandler;

        RuntimeEffect _action;

        #region Turn Callback Methods 
        public override IEnumerator TurnEnter()
        {
            yield return null;
        }

        public override IEnumerator TurnLeave()
        {
            yield return TakeControlLeave() ; 
        }

        public override IEnumerator GetAction(  ) {
            SpellAbilityRuntime latestSpell = null;
            ItemAbilityRuntime latestItem = null; 
            if (TargetSelectionFlowControl.Instance.PrepareAction() )
            {
                latestSpell = TargetSelectionFlowControl.Instance.IsLatedActionSpell();
                latestItem = TargetSelectionFlowControl.Instance.IsLatedActionItem(); 

                var latestAction = TargetSelectionFlowControl.Instance.GetLatestAction();
                InitializeAction(latestAction.Item1, latestAction.Item2);
            }

            yield return _action ;

            if (_action != null)
            {
                //After return action, we check if we need to modify energy or not 
                //TODO - Properly check if we should modify the energy 
                if (latestSpell != null)
                {
                    Debug.LogWarning("TODO : Properly check if we should modify the energy or not in a separate class or scheme");
                    _combateEntity.SpellCaster.ModifyEnergy(latestSpell.ModifiedEnergySide,latestSpell.ModifiedEnergyAmount) ;
                }if (latestItem != null)
                {
                    //_combateEntity.ItemUser.RemoveItem(latestItem); 
                }

                latestSpell = null; 
                _action = null;
            }

            
        }
        #endregion

        private void InitializeAction(RuntimeEffectFactorySO _factory, List<CombatEntity> _targets )
        {
            IEnumerator coroutine =  _factory.Factorize(_targets) ; 

            while (coroutine.MoveNext())
            {
                if (coroutine.Current != null && coroutine.Current is RuntimeEffect)
                {
                    _action = (RuntimeEffect)coroutine.Current;
                } 
            }
        }

        public override IEnumerator TakeControl()
        {
            if (_combatGraphicalHandler == null)
                _combatGraphicalHandler = GetComponent<CombatGraphicalHandler>();

            _combatGraphicalHandler.EnableGraphicalElements();
            Debug.Log(gameObject.name+ "take control");
            yield return null;
        }

        public override IEnumerator TakeControlLeave()
        {
            _combatGraphicalHandler.DisableGraphicalElements();
            yield return null;
        }
    }
}
