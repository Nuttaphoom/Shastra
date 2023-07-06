using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Vanaring_DepaDemo
{
    [RequireComponent(typeof(BaseEntityBrain))]
    public class CombatEntity : MonoBehaviour, IStatusEffectable
    {
        [Header("Right now we manually assign CharacterSheet, TO DO : Make it loaded from the main database")]
        [SerializeField]
        private CharacterSheetSO _characterSheet ;

        private BaseEntityBrain _baseEntityBrain ;

        [SerializeField]
        private SpellCasterHandler _spellCaster ;

        private StatusEffectHandler _statusEffectHandler;  
        
        private RuntimeCharacterStatsAccumulator _runtimeCharacterStatsAccumulator ;

        #region GETTER
        public RuntimeCharacterStatsAccumulator StatsAccumulator => _runtimeCharacterStatsAccumulator ;
        public SpellCasterHandler SpellCaster => _spellCaster;
        public StatusEffectHandler GetStatusEffectHandler()
        {
            return _statusEffectHandler; 
        }
        #endregion


        public void Init()
        {
            _runtimeCharacterStatsAccumulator = new RuntimeCharacterStatsAccumulator(_characterSheet) ;
            _statusEffectHandler = new StatusEffectHandler(this) ;
            
            if (! TryGetComponent(out _baseEntityBrain))
            {
                throw new Exception("BaseEntityBrain haven't been assigned"); 
            }
        }

      

    #region Turn Handler Methods 
        public IEnumerator TurnEnter()
        {
            if (_baseEntityBrain == null)
                throw new Exception("Base Entity Brain of " + gameObject.name + " hasn't been assgined") ;

            if (_statusEffectHandler == null)
                throw new Exception("Status Effect Handler hasn't never been init");

            yield return _statusEffectHandler.ExecuteStatusRuntimeEffectCoroutine();

            yield return _baseEntityBrain.TurnEnter() ; 
        }

        public  IEnumerator TurnLeave()
        {
            if (_baseEntityBrain == null)
                throw new Exception("Base Entity Brain of " + gameObject.name + " hasn't been assgined");

            yield return _baseEntityBrain.TurnLeave();
        }

        public IEnumerator GetAction( )
        {
            if (_baseEntityBrain == null)
                throw new Exception("Base Entity Brain of " + gameObject.name + " hasn't been assgined");

            IEnumerator coroutine = (_baseEntityBrain.GetAction());
            while (coroutine.MoveNext())
            {
                //No need to return null 
                if (coroutine.Current != null && coroutine.Current is RuntimeEffect)
                {
                    //Need to cast to RuntimeEffect before returning            
                    yield return (RuntimeEffect)coroutine.Current;
                }
            }
        }
        #endregion
    }
}
