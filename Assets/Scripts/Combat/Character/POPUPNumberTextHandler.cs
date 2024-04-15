using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring 
{
    [Serializable]
    public class POPUPNumberTextHandler
    {
        private CombatEntity _entity;
 

        public POPUPNumberTextHandler(CombatEntity _owner)
        {
            _entity = _owner;
            _entity.SubOnDamageVisualEvent(OnDamaged_DisplayAccumulatedDMG);
            _entity.SubOnHealVisualEvent(OnHeal_DisplayAccumulatedHeal);
            _entity.SubOnOnAilmentAppliedEventChannel(OnAilmentAppliedAttemp);

        }


        ~POPUPNumberTextHandler()
        {
            _entity.UnSubOnDamageVisualEvent(OnDamaged_DisplayAccumulatedDMG);
            _entity.UnSubOnHealVisualEvent(OnHeal_DisplayAccumulatedHeal);
            _entity.UnSubOnOnAilmentAppliedEventChannel(OnAilmentAppliedAttemp);
        }

        private void OnAilmentAppliedAttemp(EntityAilmentApplierEffect data)
        {
            if (! data.SucessfullyAttach )
            {
                if (data.ResistantBlocked)
                {
                    POPUPNumberTextManager.Instance.DisplayGeneralPOPUPText(_entity, "RESIST");
                }else
                {
                    POPUPNumberTextManager.Instance.DisplayGeneralPOPUPText(_entity, "MISS") ;
                }
            }
        }

        private void OnDamaged_DisplayAccumulatedDMG(int finalDMG)
        {
            Debug.Log("final dmg : " + finalDMG);
            POPUPNumberTextManager.Instance.DisplayDamageText(finalDMG, _entity);
  
        }

        private void OnHeal_DisplayAccumulatedHeal(int finalhHP) {    
            POPUPNumberTextManager.Instance.DiisplayHealText(finalhHP, _entity);
        }

             

        
    }
}
