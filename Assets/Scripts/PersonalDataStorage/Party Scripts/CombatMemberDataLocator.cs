using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Vanaring.RuntimeCombatMemberData;

namespace Vanaring
{
    [Serializable]
    public class CombatMemberDataLocator  
    {
        private CharacterSheetDatabaseSO m_characterSheetDatabase;

        private List<RuntimeCombatMemberData> _partyMemberData;
        public void InitializeRuntimeMemberData()
        {
            if (_partyMemberData != null)
                return;

            LoadCharacterDatabaseOP();

            _partyMemberData = new List<RuntimeCombatMemberData>();
            
            foreach (var partyMemberData in m_characterSheetDatabase.GetNormalCharacterSheets())
            {
                RuntimeCombatMemberData runtimePartyMemberData = new RuntimeCombatMemberData();
                runtimePartyMemberData.SetUpRuntimePartyMemberData(partyMemberData);
                _partyMemberData.Add(runtimePartyMemberData); 
            }
        }

        #region GETTER
        public RuntimeCombatMemberData GetRuntimeData(string memberName)
        {
            foreach (var member in _partyMemberData)
            {
                if (!member.IsThisPartyMember(memberName))
                    continue;

                return member; 
            }

            throw new System.Exception("member " + memberName + "couldn't be found within PartyMemberData"); 
        }

        public RuntimeCombatMemberData GetProtagonistRuntimeData
        {
            get
            {
                foreach (var member in _partyMemberData)
                {
                    if (!member.IsThisPartyMember("Asha"))
                        continue;

                    return member;
                }

                throw new System.Exception("The protagonist couldn't be found within PartyMemberData");
            }
        }
        

        public List<RuntimeCombatMemberData> GetRuntimeCombatMembers {
            get
            {
                return _partyMemberData ; 
            }
        }
        #endregion

        //public void LoadLocalSaveForCharacters()
        //{
        //    foreach (var member in _partyMemberData)
        //    {
        //        //member.SetUpRuntimePartyMemberData(new List<string>(),1) ; 
        //    }
        //    //loading unique key of action here 
        //    List<string> load_spelUniqueKey_of_Asha_here = new List<string>();
        //    //GetRuntimeData("Asha").SetUpRuntimePartyMemberData(load_spelUniqueKey_of_Asha_here);
        //}


        /// <summary>
        /// DO NOT CALL LOADING OPERATION IN CONSTRUCTOR 
        /// </summary>
        private void LoadCharacterDatabaseOP()
        {
            if (m_characterSheetDatabase != null)
                return;

            m_characterSheetDatabase = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<CharacterSheetDatabaseSO>(DatabaseAddressLocator.GetCharacterSheetDatabaseAddress);
        }


        #region Save System
        public object CaptureState()
        {
            Dictionary<string, RuntimeCombatMemberDataSaveLoad> saveData = new Dictionary<string, RuntimeCombatMemberDataSaveLoad>();
            foreach (var member in _partyMemberData)
            {
                string characterName = member.GetMemberName ;
                RuntimeCombatMemberDataSaveLoad captureState = (RuntimeCombatMemberDataSaveLoad)member.CaptureState();

                if (saveData.ContainsKey(characterName)) // temp skip for duplicate Asha?????
                    continue;

                saveData.Add(characterName, captureState);
            }

            return saveData;
        }

        public void RestoreState(object state)
        {
            InitializeRuntimeMemberData();
            Dictionary<string, RuntimeCombatMemberDataSaveLoad> saveData = (Dictionary<string, RuntimeCombatMemberDataSaveLoad>)state;
            
            foreach (KeyValuePair<string, RuntimeCombatMemberDataSaveLoad> data in saveData) // loop through both
            {
                GetRuntimeData(data.Key).RestoreState(data.Value);
            }
        }

        #endregion
    }
}
