using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Vanaring_DepaDemo
{
    [RequireComponent(typeof(BaseEntityBrain))]
    public class CombatEntity : MonoBehaviour
    {
        [Header("Right now we manually assign CharacterSheet, TO DO : Make it loaded from the main database")]
        [SerializeField]
        private CharacterSheetSO _characterSheet;

        private BaseEntityBrain _baseEntityBrain ;  

        private SpellCasterHandler _spellCaster ;  
        
        private RuntimeCharacterStatsAccumulator _runtimeCharacterStatsAccumulator ;

        #region GETTER
        public RuntimeCharacterStatsAccumulator StatsAccumulator => _runtimeCharacterStatsAccumulator ;
        public SpellCasterHandler SpellCaster => _spellCaster;
        #endregion


        public void Init()
        {
            _runtimeCharacterStatsAccumulator = new RuntimeCharacterStatsAccumulator(_characterSheet) ;

            if (! TryGetComponent(out _baseEntityBrain))
            {
                throw new Exception("BaseEntityBrain haven't been assigned"); 
            }
        }

        public bool IsTurnEnd ()
        {
            return false;
        }

    #region Turn Handler Methods 
        public IEnumerator TurnEnter()
        {
            if (_baseEntityBrain == null)
                Debug.Log("is null");
            yield return _baseEntityBrain.TurnEnter() ; 
        }

        public  IEnumerator TurnLeave()
        {
            yield return _baseEntityBrain.TurnLeave();
        }

        public IEnumerator GetAction( )
        {
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
