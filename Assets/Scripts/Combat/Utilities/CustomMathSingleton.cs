
using System;

namespace Vanaring 
{
   public static class VanaringMathConst
    {
        public  const int InfinityValue = 2147483647 ; 

        public static int GetATKWithScaling(EDamageScaling scaling, float inputDmg)
        {
            float mul = 1;
            switch (scaling)
            {
                case (EDamageScaling.Low):
                    mul = 0.5f; break;
                case (EDamageScaling.Medium):
                    mul = 1.0f; break;
                case (EDamageScaling.High):
                    mul = 1.5f; break;
            }
            return (int) (inputDmg * mul );
        }

        /// <summary>
        /// TODO : finish Accurency system 
        /// </summary>
        /// <param name="accurency_percent"></param>
        /// <param name="dodge_percent"></param>
        /// <returns></returns>
        public static bool IsActionSucessfullyHit(float accurency_percent, float dodge_percent)
        {
            float hitDice = UnityEngine.Random.Range(0, accurency_percent);
            float dodgeDice = UnityEngine.Random.Range(0, dodge_percent);

            return (hitDice > dodgeDice);

        }
    }

   
}