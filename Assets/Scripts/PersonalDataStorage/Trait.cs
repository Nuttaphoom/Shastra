using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        public struct Trait_Data
        {
            private int level;
            public int Getlevel() { return level; }
            private int currentexp;
            public int GetCurrentexp() { return currentexp; }

            public Trait_Data(int level, int currentexp)
            {
                this.level = level;
                this.currentexp = currentexp;
            }

            public void SetTraitData(int level, int currentexp)
            {
                this.level = level;
                this.currentexp = currentexp;
            }
        }
    }
}
