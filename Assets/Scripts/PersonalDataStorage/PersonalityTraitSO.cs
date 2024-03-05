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
            public Sprite personalityTraitSprite;
        }
        [SerializeField]
        private TraitIndex[] personalityTraitStat;

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

        public Sprite GetTraitImage(Trait.Trait_Type type)
        {
            Sprite value = null ;
            foreach (TraitIndex data in personalityTraitStat)
            {
                if (data.type != type)
                {
                    continue;
                }
                // check in case of duplicate type in array
                if (value != null)
                {
                    throw new Exception("Multiple Trait.Trait_Type: " + type + " in Array");
                }
                value = data.personalityTraitSprite ;
            }
            // check in case of Cant find the type in array
            if (value == null)
            {
                Debug.LogWarning("image for " + type + " not valid");
                //throw new Exception("Trait.Trait_Type: " + type + " not valid");
            }

            return value;
        }

         

      
    }
}
