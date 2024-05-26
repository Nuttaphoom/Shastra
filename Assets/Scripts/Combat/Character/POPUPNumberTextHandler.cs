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
            _entity.SubOnDodgeAttackVisualEvent(OnDodge_DisplayAttackDodge); 
        }


        ~POPUPNumberTextHandler()
        {
            _entity.UnSubOnDamageVisualEvent(OnDamaged_DisplayAccumulatedDMG);
            _entity.UnSubOnHealVisualEvent(OnHeal_DisplayAccumulatedHeal);
            _entity.UnSubOnOnAilmentAppliedEventChannel(OnAilmentAppliedAttemp);
            _entity.SubOnDodgeAttackVisualEvent(OnDodge_DisplayAttackDodge);

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

        /// <summary>
        /// Call in Defender 
        /// </summary>
        /// <param name="n"></param>
        private void OnDodge_DisplayAttackDodge(Null n)
        {
            POPUPNumberTextManager.Instance.DisplayDodgeText(_entity) ;
        }

        private void OnDamaged_DisplayAccumulatedDMG(int finalDMG)
        {
            POPUPNumberTextManager.Instance.DisplayDamageText(finalDMG, _entity);
  
        }

        private void OnHeal_DisplayAccumulatedHeal(int finalhHP) {    
            POPUPNumberTextManager.Instance.DisplayHealText(finalhHP, _entity);
        }

             

        
    }
}
