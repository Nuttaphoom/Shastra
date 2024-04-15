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
            return (int)(100 + (agility * 10 )); 
        }

     
        public static int CalculateACC(float agility)
        {
            return (int)(100 + ((agility + 9) * 10));
        }

        public static float CalculateHitChance(float attackerACC, float defenderEvasion)
        {
            float ret = attackerACC - defenderEvasion;
            if (ret < 10.0f)
                ret = 10.0f;
            else if (ret > 95)
                ret = 95.0f;

            return ret;
        }



    }
}
