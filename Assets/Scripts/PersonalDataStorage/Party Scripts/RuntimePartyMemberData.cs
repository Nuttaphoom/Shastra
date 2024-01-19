using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    [Serializable]
    public class RuntimePartyMemberData
    {
        [SerializeField]
        private CharacterSheetSO _characterSheetSO;

        private SpellRegister _spellRegister; 

        public void SetUpRuntimePartyMemberData()
        {
            _spellRegister.LoadSpellFromDatabase(new List<string>() );
        }

        public bool IsThisPartyMember(string characterName)
        {
            return (characterName == GetMemberName()); 
        }
        public string GetMemberName() => _characterSheetSO.CharacterName; 

    }
}
