using Kryz.CharacterStats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Vanaring
{
    public enum EDamageScaling
    {
        Low,
        Medium,
        High,  
         
    }

    public interface IAttackter
    {
        public IEnumerator LogicAttack(List<CombatEntity> targets, EDamageScaling multiplier);
    }
    public interface IDamagable
    {
        public void LogicHurt(CombatEntity attacker, StatModifier statsModm       );
        public IEnumerator VisualHurt(string animationTrigger = "Hurt");



    }
}
