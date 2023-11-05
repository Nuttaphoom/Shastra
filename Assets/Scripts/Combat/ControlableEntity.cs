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
 
        [Header("Right now we manually assign valid action, TODO : Load from Database")]
        [SerializeField]
        private ControlableEntityActionsRegistry _controlableEntityActionRegistry;
 
        
        public override IEnumerator GetAction()
        {
            yield return null;
        }

        public override IEnumerator TakeControl()
        {
            ColorfulLogger.LogWithColor("Player remain Light : " +  _spellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.LightEnergy) + " Dark : " + _spellCaster.GetEnergyAmount(RuntimeMangicalEnergy.EnergySide.DarkEnergy), Color.yellow);

            yield return base.TakeControl(); 

            //if (_combatGraphicalHandler == null)
            //    _combatGraphicalHandler = GetComponent<CombatGraphicalHandler>();

            //yield return _combatGraphicalHandler.TakeControl();
        }

        public override IEnumerator TakeControlLeave()
        {
            yield return base.TakeControlLeave(); 

            //_combatGraphicalHandler.TakeControlLeave();
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

     

        #region GETTER

        public ControlableEntityActionsRegistry GetControlableEntityActionRegistry => _controlableEntityActionRegistry;


        #endregion
    }
}