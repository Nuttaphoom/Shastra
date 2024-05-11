 
 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring
{

    
    [CreateAssetMenu(fileName = "Trigger Ability", menuName = "ScriptableObject/Combat/TriggerActionSO")]
    public class TriggerActionSO : ScriptableObject
    {
         

        [SerializeField]
        private ActorActionFactory triggerAction;

        [Header("Trigger Target Selector Rule")]
        [Header("Overflow entities will be the target")]
        [SerializeField]
        private bool _castOnBrokenEntities;

        [Header("Ally entities will be the target")]
        [SerializeField]
        private bool _castOnAlly; 
        

        public ActorAction FactorizeTriggerEffect(CombatEntity caster, List<CombatEntity> brokenEntities   )
        {
            var actorAction = triggerAction.FactorizeRuntimeAction(caster);
 
            actorAction.SetActionTarget(GetValidTarget(actorAction, caster, brokenEntities) );

            return actorAction;  // new TriggerActionAbilityRuntime(caster, this);
        }  

        private List<CombatEntity> GetValidTarget(ActorAction action, CombatEntity caster, List<CombatEntity> brokenEntities = null)
        {
            List<CombatEntity> validTarget = new List<CombatEntity>();

            if (_castOnBrokenEntities)
            {
                if (brokenEntities == null)
                    throw new Exception("target want to cast on broken entities but there is no given broken entites "); 

                validTarget = brokenEntities; 
            }
            else if (_castOnAlly)
            {

            }
            else
            {
                throw new Exception("Right now we only assign Trigger target to the broken entities only");
            }

            for (int i = 0; i < validTarget.Count; i++)
            {
                if (!action.GetTargetSelector().CorrectTarget(caster, validTarget[i]))
                {
                    validTarget.RemoveAt(i);
                    i--;
                }
            }

            return validTarget;  
        }
    }

    //public class TriggerActionAbilityRuntime : ActorAction
    //{
    //    public TriggerActionAbilityRuntime(CombatEntity caster, ActorActionFactory factory) : base(factory, caster)
    //    {

    //    }

    //    public override IEnumerator PreActionPerform()
    //    {
    //        yield return null;
    //    }

    //    public override IEnumerator PostActionPerform()
    //    {
    //        yield return null;
    //    }

    //    public override IEnumerator Simulate(CombatEntity target)
    //    {
    //        yield return null;
    //    }


    //}


}
