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

        public void LoadLocalSaveForCharacters()
        {
            //loading unique key of action here 
            //List<string> load_spelUniqueKey_of_Asha_here = new List<string>();
            //GetRuntimeData("Asha").SetUpRuntimePartyMemberData(load_spelUniqueKey_of_Asha_here);
        }
    }
}
