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
        [SerializeField]
        private PartyMemberActionRegister _memberActionRegister ;

        private int _level = 100 ;

        public void SetUpRuntimePartyMemberData(List<string> spellUniqueKeys, int level)
        {
            Debug.Log("set up character for member name : " + _characterSheetSO.CharacterName);
            _memberActionRegister = new PartyMemberActionRegister();
            _memberActionRegister.LoadSpellFromDatabase(spellUniqueKeys);
            _level = level;
            
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

        public void UnlockSpellActionSO(SpellActionSO spellActionSO)
        {
            if (_memberActionRegister == null)
                throw new Exception("_memberActionRegister is null : RuntimePartyMemberData have never been properly set up");

            _memberActionRegister.UnlockSpellAction(spellActionSO) ;
        }

        public bool IsThisPartyMember(string characterName)
        {
            return (characterName == GetMemberName()); 
        }

      
        public string GetMemberName() => _characterSheetSO.CharacterName;

        #region Save System
        public object CaptureState()
        {
            return _memberActionRegister.CaptureState();
        }

        public void RestoreState(object state)
        {
            var saveData = (List<string>)state;

            _memberActionRegister.RestoreState(saveData);
        }

        #endregion

    }
}
