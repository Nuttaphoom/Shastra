using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring.Assets.Scripts.Utilities.StringConstant
{
    public class AttributeFormulaLocator
    {
        public static int CalculateMaxHP(float vitality)
        {
            return (int) (50 + (vitality * 14) ) ;
        }

        public static int CalculateMaxMP(float Intellect)
        {
            return (int)(70 + (Intellect * 4));
        }

        public static int CalculatePhysicalATK(float strength)
        {
            return (int)(20 + (strength * 3));
        }
        public static int CalculateMagicalATK(float intellect)
        {
            return (int)(15 + (intellect * 3));
        }
        public static int CalculateEvasion(float agility)
        {
            return (int)(100 + (agility)); 
        }

        public static int CalculateACC(float agility)
        {
            return (int)(190 + (agility));
        }

      
    }
}
