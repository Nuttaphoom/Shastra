using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vanaring.Assets.Scripts.Utilities.StringConstant;

namespace Vanaring
{
    [Serializable]
    public class PartyMemberDataLocator  
    {
        private CharacterSheetDatabaseSO m_characterSheetDatabase;


        [SerializeField] 
        private List<RuntimePartyMemberData> _partyMemberData;
        public void InitializeRuntimeMemberData()
        {
            if (_partyMemberData != null)
                return;

            LoadCharacterDatabaseOP();

            _partyMemberData = new List<RuntimePartyMemberData>();
            
            foreach (var partyMemberData in m_characterSheetDatabase.GetNormalCharacterSheets())
            {
                RuntimePartyMemberData runtimePartyMemberData = new RuntimePartyMemberData();
                runtimePartyMemberData.SetUpRuntimePartyMemberData(partyMemberData);
                _partyMemberData.Add(runtimePartyMemberData); 
            }

        }

        #region GETTER
        public RuntimePartyMemberData GetRuntimeData(string memberName)
        {
            foreach (var member in _partyMemberData)
            {
                if (!member.IsThisPartyMember(memberName))
                    continue;

                return member; 
            }

            throw new System.Exception("member " + memberName + "couldn't be found within PartyMemberData"); 
        }

        public RuntimePartyMemberData GetProtagonistRuntimeData
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
            Dictionary<string, List<string>> saveData = new Dictionary<string, List<string>>();
            foreach (var member in _partyMemberData)
            {
                string characterName = member.GetMemberName();
                List<string> captureState = (List<string>)member.CaptureState();

                if (!saveData.ContainsKey(characterName)) // temp skip for duplicate Asha?????
                    continue;

                saveData.Add(characterName, captureState);
            }

            return saveData;
        }

        public void RestoreState(object state)
        {
            InitializeRuntimeMemberData(); 
             
            Dictionary<string, List<string>> saveData = (Dictionary<string, List<string>>)state;

            foreach (KeyValuePair<string, List<string>> data in saveData) // loop through both
            {
                GetRuntimeData(data.Key).RestoreState(data.Value);
            }
        }

        #endregion
    }
}
