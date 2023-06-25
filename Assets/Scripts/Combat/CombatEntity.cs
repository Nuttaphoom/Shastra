using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Vanaring_DepaDemo.Assets.Scripts.Combat.CharacterSheet;

namespace Vanaring_DepaDemo
{
    public class CombatEntity : MonoBehaviour
    {
        [Header("Right now we manually assign CharacterSheet, TO DO : Make it loaded from the main database")]
        [SerializeField]
        private CharacterSheetSO _characterSheet;

        [SerializeField] 
        private List<SpellAbilitySO> _spellAbilities ; 
        
        private RuntimeCharacterStatsAccumulator _runtimeCharacterStatsAccumulator ;
        
        public RuntimeCharacterStatsAccumulator StatsAccumulator => _runtimeCharacterStatsAccumulator ;  

        public IEnumerator Init()
        {
            _runtimeCharacterStatsAccumulator = new RuntimeCharacterStatsAccumulator() ;  
            yield return null; 
        }

        public bool IsTurnEnd ()
        {
            return false;
        } 

        public IEnumerator TurnEnter()
        {
            throw new NotImplementedException ();
        }

        public  IEnumerator TurnLeave()
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetAction()
        {
            throw new NotImplementedException();

        }
    }
}
