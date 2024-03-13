using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Vanaring.Assets.Scripts.Utilities.StringConstant;
using UnityEngine; 

namespace Vanaring 
{
    [Serializable]
    public class RelationshipHandler
    {
        [SerializeField]
        private List<RuntimeCharacterRelationshipStatus> characterRelationshipStatuses = new List<RuntimeCharacterRelationshipStatus>();

        private CharacterSheetDatabaseSO m_characterSheetDatabase ;

        #region Public Methods
        public RelationshipHandler()
        {

        }

        public void LoadRelationStatusFromDatabase()
        {
            LoadCharacterDatabaseOP();

            //if (characterRelationshipStatuses != null)
            //    throw new System.Exception("Try to laod spell from data base multiple time.This isn't allowed. " +
            //        "The system should be loaded only 1 time when the save is loaded, and modified the SpellAction thoughtout the lifetime of application, " +
            //        "and save the uniqueID when the game is saved");

            characterRelationshipStatuses = new List<RuntimeCharacterRelationshipStatus>();

            foreach (var characterSheet in m_characterSheetDatabase.GetNormalCharacterSheets())
            {
                characterRelationshipStatuses.Add(new RuntimeCharacterRelationshipStatus(characterSheet));
            }
        }

        public void ProgressRelationship(string characterName, float exp = 1)
        {
            foreach (var status in characterRelationshipStatuses)
            {
                if ( status.IsTheSameCharacter(characterName))
                {
                    status.ProgressRelationship(exp);
              
                    return;
                }
            }

            return;
        }

        public int GetCurrentRelationshipEXP(string characterName)
        {
            foreach (var runtimeStatus in characterRelationshipStatuses)
            {
                if (runtimeStatus.IsTheSameCharacter(characterName))
                {
                    return (int) runtimeStatus.GetCurrentEXP;
                }
            }

            throw new Exception("Given character name " + characterName + " couldn't be found in the runtime relationship status");
        }

        public int GetCurrentBondLevel(string characterName)
        {
            foreach (var runtimeStatus in characterRelationshipStatuses)
            {
                if (runtimeStatus.IsTheSameCharacter(characterName))
                {
                    return runtimeStatus.GetCurrentLevel ;
                }
            }

            throw new Exception("Given character name " + characterName + " couldn't be found in the runtime relationship status");
        }

        public bool IsReadyForHangout(string characterName)
        {
            foreach (var runtimeStatus in characterRelationshipStatuses)
            {
                if (runtimeStatus.IsTheSameCharacter(characterName))
                {
                    Debug.Log("Current EXP " + runtimeStatus.GetCurrentEXP);

                    Debug.Log("runtimeStatus.GetEXPCap  " + runtimeStatus.GetEXPCap);
                    return runtimeStatus.GetCurrentEXP == (runtimeStatus.GetEXPCap - 1 ) ;
                }
            }

            throw new Exception(characterName + " couldn't be found in characterRelationshipStatuses"); 
        }
        
        public int GetRelationshipCapEXP(String characterName)
        {
            foreach (var runtimeStatus in characterRelationshipStatuses)
            {
                if (runtimeStatus.IsTheSameCharacter(characterName))
                {
                    return (int) runtimeStatus.GetEXPCap ;
                }
            }

            throw new Exception(characterName + " couldn't be found in characterRelationshipStatuses");
        }
        #endregion


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
        public Dictionary<string, object> CaptureState()
        {
            Dictionary<string, object> savedData = new Dictionary<string, object>();
            foreach (RuntimeCharacterRelationshipStatus runtime in characterRelationshipStatuses)
            {
                // temp fix for multiple asha character
                if (savedData.ContainsKey(runtime.GetCharacterName))
                {
                    continue;
                }

                savedData.Add(runtime.GetCharacterName, runtime.CaptureState());
            }

            return savedData;
        }

        public void RestoreState(Dictionary<string, object> states)
        {
            foreach (RuntimeCharacterRelationshipStatus runtime in characterRelationshipStatuses)
            {
                runtime.RestoreState(states[runtime.GetCharacterName]);
            }
        }

        #endregion

    }

    [Serializable]
    public class RuntimeCharacterRelationshipStatus
    {
        [SerializeField]
        private RelationshipUEXPSystem _expSystem;

        [SerializeField] 
        private CharacterSheetSO _characterSheetSO;
        public RuntimeCharacterRelationshipStatus(CharacterSheetSO cs)
        {
            _characterSheetSO = cs;

            #region ForTestingOnly 
            if (cs.CharacterName == "Pear")
                _expSystem = new RelationshipUEXPSystem(1,4);
            else 
                _expSystem = new RelationshipUEXPSystem();
            #endregion
            _expSystem.SubOnLevelUp(OnlevelUp);

        } 
        
        ~RuntimeCharacterRelationshipStatus()
        {
            _expSystem.UnSubOnLevelUp(OnlevelUp);

        }

        #region GETTER
        public float GetCurrentEXP
        {
            get
            {
                if (_expSystem == null)
                    throw new Exception("_expSystem is null"); 

                return _expSystem.GetCurrentEXP ; 
            }
        }
        public float GetEXPCap
        {
            get
            {
                if (_expSystem == null)
                    throw new Exception("_expSystem is null");

                return _expSystem.GetEXPCap();
            }
        }

        public int GetCurrentLevel
        {
            get
            {
                if (_expSystem == null)
                    throw new Exception("_expSystem is null"); 

                return _expSystem.GetCurrentLevel ;
            }
        }

        public string GetCharacterName
        {
            get
            {
                if (_characterSheetSO.CharacterName == null)
                    throw new Exception("_characterSheetSO.CharacterName is null");

                return _characterSheetSO.CharacterName;
            }
        }

        #endregion


        public bool IsTheSameCharacter(string characterName)
        {
            return characterName == _characterSheetSO.CharacterName; 
        }

        public void ProgressRelationship(float exp)
        {
            _expSystem.ReceiveEXP(exp);
        }

        private void OnlevelUp(int curLevel)
        {
            Debug.Log("Level Up to " + curLevel) ; 
        }

        #region Save System
        public object CaptureState()
        {
            return _expSystem.CaptureState();
        }

        public void RestoreState(object state)
        {
            _expSystem.RestoreState(state);
        }

        #endregion

    }
}
