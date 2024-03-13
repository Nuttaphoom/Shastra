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
 
        public List<SpellActionSO> GetSpellAction
        {
            get
            {
                //foreach (SpellActionSO spellaction in _spellActions)
                //{
                //    Debug.Log("spell in spell actions : " + spellaction.AbilityName);
                //}
                return _spellActions;
            }
        }
        public List<WeaponActionSO> GetWeaponAction => _equipedWeaponAction ;

        public void RegisterSpell(List<SpellActionSO> spells)
        { 
            if (_spellActions == null)
                _spellActions = new List<SpellActionSO>();

            foreach (SpellActionSO spellAction in spells)
            {
                bool pass = false; 
                foreach (var registeredSpell in _spellActions)
                {
                    if (registeredSpell.AbilityName == spellAction.AbilityName)
                    {
                        Debug.Log("Pass spell " + spellAction.AbilityName);
                        pass = true;
                        break; 
                    }
                }
                if (pass)
                    continue;


                Debug.Log("Add spell " + spellAction.AbilityName); 
                _spellActions.Add(spellAction);
            } 

            

        }
    }
}
