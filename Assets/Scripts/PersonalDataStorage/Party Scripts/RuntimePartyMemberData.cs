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

        private PartyMemberActionRegister _memberActionRegister ;

        private int _level = 1 ;

        

        public void SetUpRuntimePartyMemberData(List<string> spellUniqueKeys, int level)
        {
            _memberActionRegister = new PartyMemberActionRegister(); 
            _memberActionRegister.LoadSpellFromDatabase(spellUniqueKeys);
        }

        public List<SpellActionSO> GetRegisteredSpellActionSO
        {
            get
            {
                if (_memberActionRegister == null)
                    throw new Exception("_memberActionRegister is null : RuntimePartyMemberData have never been properly set up");
               
                return _memberActionRegister.GetRegisteredSpell(); 
            }
        }

        public bool IsThisPartyMember(string characterName)
        {
            return (characterName == GetMemberName()); 
        }
        public string GetMemberName() => _characterSheetSO.CharacterName; 

    }
}
