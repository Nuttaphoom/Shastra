using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring_DepaDemo 
{
    public enum EDamageScaling
    {
        Low,
        Medium,
        High,  
         
    }

    public interface IAttackter
    {
        public IEnumerator Attack(List<CombatEntity> targets, float multiplier, ActionAnimationInfo animationinfo);

    }
    public interface IDamagable
    {
        public void LogicHurt(int inputdmg);
        public IEnumerator VisualHurt(string animationTrigger = "Hurt"); 
        
    }
}
