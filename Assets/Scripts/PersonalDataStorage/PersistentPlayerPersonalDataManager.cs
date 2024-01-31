using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Vanaring
{
    public class PersistentPlayerPersonalDataManager : PersistentInstantiatedObject<PersistentPlayerPersonalDataManager> 
    {
        // TEMP : Manually Load data from SO for now 
        [SerializeField]
        private PersonalityTraitSO player_personalityTraitSO;

        [HideInInspector]
        public PersonalityTrait player_personalityTrait;

        [SerializeField]
        private PartyMemberDataLocator _partyDataLocator;

        [SerializeField]
        private Backpack _backpack;

        #region GETTER 
        public PartyMemberDataLocator PartyMemberDataLocator
        {
            get
            {
                if (_partyDataLocator == null)
                    throw new Exception("PartyDataLocator is null"); 

                return _partyDataLocator;
            }
        }

        #endregion

        private void Awake()
        {
            // TO DO : 
            player_personalityTrait = new PersonalityTrait(player_personalityTraitSO);

            _partyDataLocator.LoadLocalSaveForCharacters(); 
        }

         
    }
}
