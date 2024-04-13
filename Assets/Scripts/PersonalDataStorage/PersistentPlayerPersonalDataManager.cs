using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Vanaring
{
    public class PersistentPlayerPersonalDataManager : PersistentInstantiatedObject<PersistentPlayerPersonalDataManager> , ISaveable
    {
        // TEMP : Manually Load data from SO for now 
        [Header("These parameter should not be set in inspector")] 
        [SerializeField]
        private PersonalityTraitSO player_personalityTraitSO;

        [SerializeField]
        private PersonalityTrait player_personalityTrait;
        
        [SerializeField]
        private CombatMemberDataLocator _partyDataLocator;

        [SerializeField]
        private RelationshipHandler _relationshipHandler;

        /// <summary>
        /// TODO : Make this class none-serialized
        /// </summary>
        [SerializeField]
        private Backpack _backpack;

        #region GETTER 
        public CombatMemberDataLocator CombatMemberDataLocator
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

        public Backpack GetBackpack
        {
            get
            {
                if (_backpack == null)
                    throw new Exception("_backpack is null");
                return _backpack;
            }
        }
        #endregion


        private void Awake()
        {
            // TO DO : 
            player_personalityTrait = new PersonalityTrait(player_personalityTraitSO);
            _relationshipHandler = new RelationshipHandler() ;
            _partyDataLocator = new CombatMemberDataLocator() ;  

            //TODO : Transfer this function into save/load system
            _relationshipHandler.LoadRelationStatusFromDatabase() ; 
            _partyDataLocator.InitializeRuntimeMemberData();

            //PersistentSaveLoadManager.Instance.Load("PlayerPersonalData");

        }

        #region Save System

        [Serializable]
        private struct SaveData
        {
            public Dictionary<string, RuntimeCombatMemberData.RuntimeCombatMemberDataSaveLoad> savePartyDataLocator;
            public Backpack.BackpackSaveData saveBackpackData;
            public Dictionary<string, object> saveRelationshipHandler;
        }

        public object CaptureState()
        {
            Dictionary<string, RuntimeCombatMemberData.RuntimeCombatMemberDataSaveLoad> partyDataLocatorState = (Dictionary<string, RuntimeCombatMemberData.RuntimeCombatMemberDataSaveLoad>)_partyDataLocator.CaptureState();

            Backpack.BackpackSaveData backpackState = (Backpack.BackpackSaveData)_backpack.CaptureBackpackState(); 

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
            _backpack.RestoreBackpackState(saveData.saveBackpackData);
            _relationshipHandler.RestoreState(saveData.saveRelationshipHandler);
        }

        #endregion
    }
}
