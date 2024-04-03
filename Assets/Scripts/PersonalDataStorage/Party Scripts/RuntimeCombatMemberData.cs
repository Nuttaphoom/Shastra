using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Vanaring
{
    [Serializable]
    public class RuntimeCombatMemberData
    {
        [SerializeField]
        private CharacterSheetSO _characterSheetSO;
        [SerializeField]
        private CombatMemberActionRegister _memberActionRegister ;
        [SerializeField]
        private LevelAttributeHandler _levelAttributeHandler; 

        public void SetUpRuntimePartyMemberData(CharacterSheetSO sheet)
        {
            _characterSheetSO = sheet ; 
            _memberActionRegister = new CombatMemberActionRegister();
            _levelAttributeHandler = new LevelAttributeHandler(this); 

            //_memberActionRegister.LoadSpellFromDatabase(spellUniqueKeys);
        }

        public LevelAttributeHandler LevelAttributeHandler
        {
            get
            {
                if (_levelAttributeHandler == null)
                    throw new Exception("_levelAttributeHandler is null");

                return _levelAttributeHandler;
            }
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

        public CombatCharacterSheetSO GetCharacterSheet
        {
            get
            {
                if (_characterSheetSO is not CombatCharacterSheetSO)
                    throw new Exception("Character sheet " + _characterSheetSO.CharacterName + " is not CombatCaracterSheetSO"); 

                return _characterSheetSO as CombatCharacterSheetSO ; 
            }
        }

        #region Save System
        public object CaptureState()
        {
            RuntimeCombatMemberDataSaveLoad capturedData = new RuntimeCombatMemberDataSaveLoad() ;
            capturedData.RegisteredAction = _memberActionRegister.CaptureState() as List<string> ;
            capturedData.CurrentLevel = _levelAttributeHandler.GetCharacterUEXPSystem.GetCurrentLevel;
            capturedData.CurrentEXP = _levelAttributeHandler.GetCharacterUEXPSystem.GetCurrentEXP ;

            return capturedData; //.CaptureState(); 
        }

        public void RestoreState(object state)
        {
            var saveData = (RuntimeCombatMemberDataSaveLoad) state;

            _memberActionRegister.RestoreState(saveData.RegisteredAction);

            _levelAttributeHandler.RestoreLevelDataFromLocalSave(saveData.CurrentLevel, saveData.CurrentEXP);
        }

        public struct RuntimeCombatMemberDataSaveLoad
        {
            public List<string> RegisteredAction;
            public int CurrentLevel;
            public float CurrentEXP;
        }

        #endregion

    }
}
