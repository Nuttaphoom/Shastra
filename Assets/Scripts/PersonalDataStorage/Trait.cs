using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq.Expressions;

namespace Vanaring
{
    namespace Trait
    {
        [Serializable]
        public enum Trait_Type
        { 
            Kindness = 0,
            Charm,
            Knowledge,
            Proficiency
        }

        // Trait_Data only contain trait data such as level and currentexp
        [Serializable]
        public class Trait_Data
        {
            public int Getlevel() {
                return _personalityEXP.GetCurrentLevel ; 
            }
            
            private PersonalityTraitUEXPSystem _personalityEXP; 
            private Sprite _triatIcon;
            public float GetCurrentexp() { return _personalityEXP.GetCurrentEXP; }
            public float GetCurrentEXPCap() { return _personalityEXP.GetEXPCap() ; }
            
            public Sprite GetPersonalityTraitIcon
            {
                get {  
                    if (_triatIcon == null)
                    {
                        throw new Exception("_triatIcon is null"); 
                    }
                    return _triatIcon; 
                }
            }

            public Trait_Data(int level, float currentexp, Sprite traitIcon)
            {
                _personalityEXP = new PersonalityTraitUEXPSystem(level, currentexp);

                //this.level = level;
                //this. = currentexp;
                this._triatIcon = traitIcon;
            }

            public void SetTraitData(int level, float currentexp)
            {
                _personalityEXP = new PersonalityTraitUEXPSystem(level, currentexp);
            }
        }
    }
}
