using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "PersonalityTraitSO", menuName = "ScriptableObject/PersonalityTraitSO")]
    public class PersonalityTraitSO : ScriptableObject
    {
        [Serializable]
        private struct TraitIndex
        {
            public Trait.Trait_Type type;
            public int value;
        }
        [SerializeField]
        private TraitIndex[] personalityTraitStat;

        [Tooltip("Exp point for the level eg. level 1 would have to use amount of exp in element1 to upgrade to level2.")]
        [SerializeField]
        private int[] trait_require_exp;

        public int GetStat(Trait.Trait_Type type)
        {
            int value = -1;
            foreach (TraitIndex data in personalityTraitStat)
            {
                if (data.type != type)
                {
                    continue;
                }
                // check in case of duplicate type in array
                if (value != -1)
                {
                    throw new Exception("Multiple Trait.Trait_Type: " + type + " in Array");
                }
                value = data.value;
            }
            // check in case of Cant find the type in array
            if (value == -1)
            {
                throw new Exception("Trait.Trait_Type: " + type + " not valid");
            }

            return value;

        }

        public int GetTraitRequireExpSize()
        {
            return trait_require_exp.Length;
        }

        public int GetTraitRequireExp(int level)
        {
            // check if current level is valid or not in the SO
            if (level >= trait_require_exp.Length || level < 0)
            {
                throw new Exception("trait_require_exp: on level " + level + " is not valid!");
            }
            return trait_require_exp[level];
        }
    }
}
