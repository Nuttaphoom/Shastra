using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Vanaring
{
    public class PersistentPlayerPersonalDataManager : PersistentInstantiatedObject<PersistentPlayerPersonalDataManager> , ISaveable
    {
        // TEMP : Manually Load data from SO for now 
        [SerializeField]
        private PersonalityTraitSO player_personalityTraitSO;

        [SerializeField]
        private PersonalityTrait player_personalityTrait;
        
        [SerializeField]
        private PartyMemberDataLocator _partyDataLocator;

        [SerializeField]
        private RelationshipHandler _relationshipHandler;

        /// <summary>
        /// TODO : Make this class none-serialized
        /// </summary>
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

        public PersonalityTrait GetPersonalityTrait {  get
            {
                return player_personalityTrait; 
            } }

        public RelationshipHandler RelationshipHandler { get { return _relationshipHandler; } }
        #endregion


        private void Awake()
        {
            // TO DO : 
            player_personalityTrait = new PersonalityTrait(player_personalityTraitSO);
            _relationshipHandler = new RelationshipHandler() ;
            _partyDataLocator = new PartyMemberDataLocator() ;  

            //TODO : Transfer this function into save/load system
            _relationshipHandler.LoadRelationStatusFromDatabase() ; 
            _partyDataLocator.InitializeRuntimeMemberData();

            //PersistentSaveLoadManager.Instance.Load("PlayerPersonalData");

        }

        #region Save System

        [Serializable]
        private struct SaveData
        {
            public Dictionary<string, List<string>> savePartyDataLocator;
            public List<string> saveBackpackData;
            public Dictionary<string, object> saveRelationshipHandler;
        }

        public object CaptureState()
        {
            Dictionary<string, List<string>> partyDataLocatorState = (Dictionary<string, List<string>>)_partyDataLocator.CaptureState();
            List<string> backpackState = (List<string>)_backpack.CaptureState();
            Dictionary<string, object> relationshipHandlerState = (Dictionary<string, object>)_relationshipHandler.CaptureState();
            return new SaveData
            {
                savePartyDataLocator = partyDataLocatorState,
                saveBackpackData = backpackState,
                saveRelationshipHandler = relationshipHandlerState

            };
        }

        public void RestoreState(object state)
        {
            SaveData saveData = (SaveData)state;

            _partyDataLocator.RestoreState(saveData.savePartyDataLocator);
            _backpack.RestoreState(saveData.saveBackpackData);
            _relationshipHandler.RestoreState(saveData.saveRelationshipHandler);
        }

        #endregion
    }
}
