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
        }


        ~POPUPNumberTextHandler()
        {
            _entity.UnSubOnDamageVisualEvent(OnHPATKVisualUpdate);
            _entity.UnSubOnHealVisualEvent(OnHPATKVisualUpdate);
        }


        private void OnHPATKVisualUpdate(int DONTUSE)
        {
            POPUPNumberTextManager.Instance.DisplayDPOPUPText(_accumulatedDMG, _accumulatedHP, _entity);
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
