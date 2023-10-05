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
 
        public List<SpellActionSO> GetSpellAction => _spellActions ;  
    }
}
