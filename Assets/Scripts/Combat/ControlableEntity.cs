using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring
{
    public class ControlableEntity : CombatEntity
    {
        private CombatGraphicalHandler _combatGraphicalHandler;

        [Header("Right now we manually assign valid action, TODO : Load from Database")]
        [SerializeField]
        private ControlableEntityActionsRegistry _controlableEntityActionRegistry; 
        public override IEnumerator GetAction()
        {
            yield return null;
        }

        public override IEnumerator TakeControl()
        {
            if (_combatGraphicalHandler == null)
                _combatGraphicalHandler = GetComponent<CombatGraphicalHandler>();

            yield return _combatGraphicalHandler.TakeControl();
        }

        public override IEnumerator TakeControlLeave()
        {
            _combatGraphicalHandler.TakeControlLeave();
            yield return null;
        }

        public override IEnumerator TurnEnter()
        {
            yield return base.TurnEnter();
        }

        public override IEnumerator TurnLeave()
        {
            yield return base.TurnLeave();
        }

        public IEnumerator TakeControlSoftLeave()
        {
            _combatGraphicalHandler.DisableMenuElements();

            yield return null;
        }

        #region GETTER

        public ControlableEntityActionsRegistry GetControlableEntityActionRegistry => _controlableEntityActionRegistry;


        #endregion
    }
}