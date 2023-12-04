using System;
using TMPro;
using UnityEngine;

namespace Vanaring 
{
    [Serializable]
    public class POPUPNumberTextHandler
    {
        private CombatEntity _entity;

        private int _accumulatedDMG = 0;
        private int _accumulatedHP = 0; 

        public POPUPNumberTextHandler(CombatEntity _owner)
        {
            
            _entity = _owner;
            _entity.SubOnDamageVisualEvent(OnHPATKVisualUpdate);
            _entity.SubOnHealVisualEvent(OnHPATKVisualUpdate);
            _entity.SubOnOnAilmentAppliedEventChannel(OnAilmentAppliedAttemp);

        }


        ~POPUPNumberTextHandler()
        {
            _entity.UnSubOnDamageVisualEvent(OnHPATKVisualUpdate);
            _entity.UnSubOnHealVisualEvent(OnHPATKVisualUpdate);
            _entity.UnSubOnOnAilmentAppliedEventChannel(OnAilmentAppliedAttemp);
        }

        private void OnAilmentAppliedAttemp(EntityAilmentApplierEffect data)
        {
            if (! data.SucessfullyAttach )
            {
                if (data.ResistantBlocked)
                {
                    POPUPNumberTextManager.Instance.DisplayPOPUPText(_entity, "RESIST");
                }else
                {
                    POPUPNumberTextManager.Instance.DisplayPOPUPText(_entity, "MISS") ;
                }
            }
        }

        private void OnHPATKVisualUpdate(int DONTUSE)
        {
            POPUPNumberTextManager.Instance.DisplayDPOPUPDamageHealText(_accumulatedDMG, _accumulatedHP, _entity);
            _accumulatedDMG = 0;
            _accumulatedHP = 0;
        }

        public void AccumulateDMG(int dmg)
        {
            _accumulatedDMG += dmg; 
        }

        public void AccumulateHP(int hp)
        {
            _accumulatedHP += hp; 
        }
    }
}
