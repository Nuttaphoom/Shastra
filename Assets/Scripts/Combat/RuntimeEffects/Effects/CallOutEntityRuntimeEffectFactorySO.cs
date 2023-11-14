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
        [Serializable]
        public struct CallOutEntityDataStruct
        {
            public ECompetatorSide Side ;
            public CombatEntity CombatEntity; 
        }

        [SerializeField]
        private List<CallOutEntityDataStruct> _entitiesData;
 

        public override RuntimeEffect Factorize(List<CombatEntity> targets)
        {
            return SetUpRuntimeEffect(new CallOutEntityRuntimeEffect(_entitiesData), targets);
        }

        public override void SimulateEnergyModifier(CombatEntity combatEntity)
        {
            //combatEntity.SpellCaster.Simulate(RuntimeMangicalEnergy.EnergySide.LightEnergy, 100, null); 
        }
    }

    public class CallOutEntityRuntimeEffect : RuntimeEffect
    {
        private List<CallOutEntityRuntimeEffectFactorySO.CallOutEntityDataStruct> _entitiesData;
        public CallOutEntityRuntimeEffect(List<CallOutEntityRuntimeEffectFactorySO.CallOutEntityDataStruct> entitiesData )
        {
            _entitiesData = new List<CallOutEntityRuntimeEffectFactorySO.CallOutEntityDataStruct>();
            foreach (var entity in entitiesData )
                _entitiesData.Add( entity );
        }

        public override IEnumerator ExecuteRuntimeCoroutine(CombatEntity caster)
        {
            List<IEnumerator> ie = new List<IEnumerator>();
            foreach (var entityData in _entitiesData)
            {
                var prefabTemplate = entityData.CombatEntity;
                var side = entityData.Side; 

                ie.Add(CombatReferee.Instance.InstantiateCompetator( prefabTemplate,  side)) ;
            }

            yield return new WaitAll(caster,ie.ToArray() );
        }










    }
}