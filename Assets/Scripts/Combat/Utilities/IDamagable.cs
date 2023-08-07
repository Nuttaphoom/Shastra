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
        public IEnumerator Attack(List<CombatEntity> targets, EDamageScaling multiplier, ActionAnimationInfo animationinfo);

    }
    public interface IDamagable
    {

        public void LogicHurt(CombatEntity attacker, int inputdmg);
        public IEnumerator VisualHurt(CombatEntity attacker,string animationTrigger = "Hurt"); 
        
    }
}
