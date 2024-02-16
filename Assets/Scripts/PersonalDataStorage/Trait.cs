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
            private int level;
            public int Getlevel() { return level; }
            private float currentexp;
            private Sprite _triatIcon;
            public float GetCurrentexp() { return currentexp; }
            
            
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
                this.level = level;
                this.currentexp = currentexp;
                this._triatIcon = traitIcon;
            }

            public void SetTraitData(int level, float currentexp)
            {
                this.level = level;
                this.currentexp = currentexp;
            }
        }
    }
}
