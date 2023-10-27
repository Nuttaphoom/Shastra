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
    [CreateAssetMenu(fileName = "Physical Ability", menuName = "ScriptableObject/Combat/PhysicalActionSO")]
    public class PhysicalActionSO : ActorActionFactory 
    {
        public override ActorAction FactorizeRuntimeAction(CombatEntity caster)
        {
            return new PhysicalAbilityRuntime(caster, _targetSelector, _actionSignal);
        }
    }

    public class PhysicalAbilityRuntime : ActorAction
    {
        public PhysicalAbilityRuntime(CombatEntity caster, TargetSelector targetSelector, ActionSignal actionSignal)
        {
            _caster = caster;
            _targetSelector = targetSelector;
            _actionSignal = new ActionSignal(actionSignal);
        }

        public override IEnumerator PreActionPerform()
        {
            yield return null;
        }

        public override IEnumerator PostActionPerform()
        {
            yield return null;
        }
 
        public override IEnumerator Simulate(CombatEntity target)
        {
            throw new NotImplementedException();
            yield return null;
        }

        private void SetUpTimeLineActorSetting()
        {
            List<GameObject> actors = new List<GameObject>();

            actors.Add(_caster.gameObject);
            foreach (var entity in _targets)
            {
                actors.Add(entity.gameObject);
            }

            _actionSignal.SetUpActorsSetting(actors);
        }
    }


}
