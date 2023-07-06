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
        [SerializeField]
        private RuntimeEffectFactorySO _testeFFECT;  

        RuntimeEffect _action;  
        public override IEnumerator TurnEnter()
        {
            yield return null; 
        }

        public override IEnumerator TurnLeave()
        {
            yield return null; 
        }

        public override IEnumerator GetAction(  ) {

            if (TargetSelectionFlowControl.Instance.PrepareAction() )
            {
                var latestAction = TargetSelectionFlowControl.Instance.GetLatestAction();
                InitializeAction(latestAction.Item1, latestAction.Item2);
            }
            yield return _action ;

            if (_action != null)
            {
                _action = null;
            }

            yield return null; 
        }

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
    }
}
