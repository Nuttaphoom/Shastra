using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Vanaring
{
    public class PersonalityTrait
    {
        private Dictionary<Trait.Trait_Type, int> traits = new Dictionary<Trait.Trait_Type, int>();
        public PersonalityTrait(PersonalityTraitSO personalityTraitSO)
        {
            foreach (Trait.Trait_Type type in Enum.GetValues(typeof(Trait.Trait_Type)))
            {
                traits[type] = personalityTraitSO.GetStat(type);
            }
        }

        public void SetStat(Trait.Trait_Type type, int value)
        {
            traits[type] = value;
        }

        public int GetStat(Trait.Trait_Type type)
        {
            return traits[type];
        }
    }
}
