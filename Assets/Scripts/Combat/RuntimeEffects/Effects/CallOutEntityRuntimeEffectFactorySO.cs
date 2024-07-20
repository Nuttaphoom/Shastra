using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
            //throw new NotImplementedException(); 
            List<IEnumerator> ie = new List<IEnumerator>();
            List<CombatEntity> prefabs = new List<CombatEntity>();
            ECompetatorSide side = _entitiesData[0].Side  ;

            foreach (var entityData in _entitiesData)
            {
                prefabs.Add(entityData.CombatEntity)  ;
            }

            ie.Add(CombatReferee.Instance.InstantiateCompetator(prefabs, side, true));

            yield return new WaitAll(caster, ie.ToArray());
        }










    }
}