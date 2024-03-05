using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace Vanaring
{
    [Serializable]
    public class PersonalityTrait
    {
        [SerializeField]
        private Dictionary<Trait.Trait_Type, Trait.Trait_Data> traits = new Dictionary<Trait.Trait_Type, Trait.Trait_Data>();

  
        public PersonalityTrait(PersonalityTraitSO personalityTraitSO)
        {
            foreach (Trait.Trait_Type type in Enum.GetValues(typeof(Trait.Trait_Type)))
            {
                // TEMP : Setting start exp with 0
                traits[type] = new Trait.Trait_Data(personalityTraitSO.GetStat(type), 0, personalityTraitSO.GetTraitImage(type));
            }
        }


        public void SetStat(Trait.Trait_Type type, int level, float exp)
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

        public float GetCurrentTraitRequireEXP(Trait.Trait_Type type)
        {
            // Warning of forgot adding new type to Base SO
            if (!traits.ContainsKey(type))
            {
                throw new Exception("Trait.Trait_Type don't have this type: " + type);
            }
            return traits[type].GetCurrentEXPCap(); //  trait_require_exp[traits[type].Getlevel()];
        }

        public float GetCurrentTraitEXP(Trait.Trait_Type type)
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
