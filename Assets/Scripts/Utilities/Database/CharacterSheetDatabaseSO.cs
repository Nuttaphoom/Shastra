﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

namespace Vanaring
{
    [CreateAssetMenu(fileName = "CharacterSheetLocatorSO", menuName = "ScriptableObject/Database/CharacterSheetLocatorSO")]
    public class CharacterSheetDatabaseSO : BaseDatabaseSO<CharacterSheetSO>
    {
        public List<CharacterSheetSO> GetNormalCharacterSheets()
        {
            var ret = new List<CharacterSheetSO>(); 
            foreach (var record in _records) {
                ret.Add(record.GetRecorded()); 
            }
            return ret ;  
        }

        public List<CombatCharacterSheetSO> GetCombatCharacterShhets()
        {
            var ret = new List<CombatCharacterSheetSO>();
            foreach (var record in _records)
            {
                if ( record.GetRecorded() as CombatCharacterSheetSO != null)  
                    ret.Add(record.GetRecorded() as CombatCharacterSheetSO);
            }
       
             
            return ret;
        }
    }
}
