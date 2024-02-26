using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring 
{
    [Serializable]
    public class ControlableEntityActionsRegistry 
    {
        [SerializeField] 
        private List<SpellActionSO> _spellActions;

        [SerializeField]
        private List<WeaponActionSO> _equipedWeaponAction ;
 
        public List<SpellActionSO> GetSpellAction => _spellActions ;  
        public List<WeaponActionSO> GetWeaponAction => _equipedWeaponAction ;

        public void RegisterSpell(List<SpellActionSO> spells)
        {
            _spellActions = new List<SpellActionSO>(spells);

            foreach (SpellActionSO spellAction in _spellActions)
            {
                _spellActions.Add(spellAction);
            } 


        }
    }
}
