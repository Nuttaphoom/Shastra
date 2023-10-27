
namespace Vanaring 
{
   public static class VanaringMathConst
    {
        public  const int InfinityValue = 2147483647 ; 

        public static int GetATKWithScaling(EDamageScaling scaling, int inputDmg)
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
    }
}