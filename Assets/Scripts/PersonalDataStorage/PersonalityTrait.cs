using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Vanaring
{
    public class PersonalityTrait
    {
        private Dictionary<Trait.Trait_Type, Trait.Trait_Data> traits = new Dictionary<Trait.Trait_Type, Trait.Trait_Data>();
        private List<int> trait_require_exp = new List<int>();
        public PersonalityTrait(PersonalityTraitSO personalityTraitSO)
        {
            foreach (Trait.Trait_Type type in Enum.GetValues(typeof(Trait.Trait_Type)))
            {
                // TEMP : Setting start exp with 0
                traits[type] = new Trait.Trait_Data(personalityTraitSO.GetStat(type), 0);
            }

            if (trait_require_exp.Count > 0)
            {
                throw new Exception("trait_require_exp is already has data");
            }

            for (int i = 0; i < personalityTraitSO.GetTraitRequireExpSize(); i++)
            {
                Debug.Log( i + " : " + personalityTraitSO.GetTraitRequireExp(i));
                trait_require_exp.Add(personalityTraitSO.GetTraitRequireExp(i));
            }
        }

        public void SetStat(Trait.Trait_Type type, int level, int exp)
        {
            traits[type].SetTraitData(level, exp);
        }

        public Trait.Trait_Data GetStat(Trait.Trait_Type type)
        {
            // Warning of forgot adding new type to Base SO
            if (!traits.ContainsKey(type))
            {
                throw new Exception("Trait.Trait_Type don't have this type: " + type);
            }
            return traits[type];
        }

        public int GetCurrentTraitRequireEXP(Trait.Trait_Type type)
        {
            // Warning of forgot adding new type to Base SO
            if (!traits.ContainsKey(type))
            {
                throw new Exception("Trait.Trait_Type don't have this type: " + type);
            }
            return trait_require_exp[traits[type].Getlevel()];
        }

        public int GetCurrentTraitEXP(Trait.Trait_Type type)
        {
            // Warning of forgot adding new type to Base SO
            if (!traits.ContainsKey(type))
            {
                throw new Exception("Trait.Trait_Type don't have this type: " + type);
            }
            return traits[type].GetCurrentexp();
        }
    }
}
