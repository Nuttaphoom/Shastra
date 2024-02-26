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

        public void SetUpRuntimePartyMemberData(CharacterSheetSO sheet)
        {
            _characterSheetSO = sheet ; 
            _memberActionRegister = new PartyMemberActionRegister();
            //_memberActionRegister.LoadSpellFromDatabase(spellUniqueKeys);
            
        }

        public List<SpellActionSO> GetRegisteredSpellActionSO
        {
            get
            {
                if (_memberActionRegister == null)
                    throw new Exception("_memberActionRegister is null : RuntimePartyMemberData have never been properly set up" + _characterSheetSO.CharacterName);
               
                return _memberActionRegister.GetRegisteredSpell(); 
            }
        }

        public void UnlockSpellActionSO(SpellActionSO spellActionSO)
        {
            if (_memberActionRegister == null)
                throw new Exception("_memberActionRegister is null : RuntimePartyMemberData have never been properly set up in " + _characterSheetSO.CharacterName);

            _memberActionRegister.UnlockSpellAction(spellActionSO) ;
        }

        public bool IsThisPartyMember(string characterName)
        {
            return (characterName == GetMemberName ) ; 
        }


        public string GetMemberName {
            get {
                if (_characterSheetSO == null)
                    throw new Exception("CharacterSheetSO is null"); 

                return _characterSheetSO.CharacterName;
            }
        } 
        

        #region Save System
        public object CaptureState()
        {
            return _memberActionRegister.CaptureState();
        }

        public void RestoreState(object state)
        {
            var saveData = (List<string>)state;

            Debug.Log("restore state in RuntimePartyMemberData");
            _memberActionRegister.RestoreState(saveData);
        }

        #endregion

    }
}
