using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.EventSystems.EventTrigger;


namespace Vanaring
{
    [CreateAssetMenu(fileName = "CallOutEntityRuntimeEffectFactorySO", menuName = "ScriptableObject/RuntimeEffect/CallOutEntityRuntimeEffectFactorySO")]
    public class CallOutEntityRuntimeEffectFactorySO : RuntimeEffectFactorySO
    {
        [SerializeField]
        private ECompetatorSide _side;

        [SerializeField]
        private CombatEntity _prefab;
 

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            CallOutEntityRuntimeEffect retEffect = new CallOutEntityRuntimeEffect(_prefab, _side);
            if (targets != null)
            {
                foreach (CombatEntity target in targets)
                    retEffect.AssignTarget(target);
            }

            return retEffect;
        }

        public override void SimulateEnergyModifier(CombatEntity combatEntity)
        {
            //combatEntity.SpellCaster.Simulate(RuntimeMangicalEnergy.EnergySide.LightEnergy, 100, null); 
        }
    }

    public class CallOutEntityRuntimeEffect : RuntimeEffect
    {
        private CombatEntity _prefabTemplate;
        private ECompetatorSide _side;
        public CallOutEntityRuntimeEffect(CombatEntity prefabTemplate, ECompetatorSide side)
        {
            _prefabTemplate = prefabTemplate;
            _side = side;
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            if (_prefabTemplate == null)
                throw new NullReferenceException();

            CombatReferee.instance.InstantiateCompetator(_prefabTemplate,_side);

            yield return null;

        }










    }
}