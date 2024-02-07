using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [Serializable]
    public class PartyMemberDataLocator  
    {
        [SerializeField] 
        private List<RuntimePartyMemberData> _partyMemberData;
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

        #region Save System
        public object CaptureState()
        {
            Dictionary<string, List<string>> saveData = new Dictionary<string, List<string>>();
            foreach (var member in _partyMemberData)
            {
                string characterName = member.GetMemberName();
                List<string> captureState = (List<string>)member.CaptureState();

                saveData.Add(characterName, captureState);
            }

            return saveData;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, List<string>> saveData = (Dictionary<string, List<string>>)state;

            foreach (KeyValuePair<string, List<string>> data in saveData) // loop through both
            {
                GetRuntimeData(data.Key).RestoreState(data.Value);
            }
        }

        #endregion
    }
}
