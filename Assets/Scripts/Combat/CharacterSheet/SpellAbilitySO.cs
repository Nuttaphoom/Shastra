using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring_DepaDemo
{ 
    [CreateAssetMenu(fileName = "Spell Ability", menuName = "ScriptableObject/Combat/SpellAbility")]
    public class SpellAbilitySO : CombatActionSO
    {
        [SerializeField]
        private RuntimeMangicalEnergy.EnergySide _energySide;

        [SerializeField]
        private int _energyRequire;
    }
}
