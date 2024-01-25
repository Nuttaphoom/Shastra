using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    [Serializable]
    public class PartyMemberDataLocator :  ISaveable
    {

        [SerializeField] 
        private List<RuntimePartyMemberData> _partyMemberData;

        public object CaptureState()
        {
            throw new System.NotImplementedException();
        }

        public void RestoreState(object state)
        {
            throw new System.NotImplementedException();
        }
    }
}
