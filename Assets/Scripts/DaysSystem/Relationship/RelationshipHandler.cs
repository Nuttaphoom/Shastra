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

            foreach ( var characterSheet in m_characterSheetDatabase.GetNormalCharacterShhets()) {
                characterRelationshipStatuses.Add(new RuntimeCharacterRelationshipStatus(characterSheet));
            }
        }

        public void ProgressRelationship(string characterName, int exp = 1)
        {
            foreach (var status in characterRelationshipStatuses)
            {
                if (!status.IsTheSameCharacter(characterName))
                {
                    status.ProgressRelationship(exp);
                    return;
                }
            }

            return;
        }

        /// <summary>
        /// DO NOT CALL LOADING OPERATION IN CONSTRUCTOR 
        /// </summary>
        private void LoadCharacterDatabaseOP()
        {
            if (m_characterSheetDatabase != null)
                return;

            m_characterSheetDatabase = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<CharacterSheetDatabaseSO>(DatabaseAddressLocator.GetCharacterSheetDatabaseAddress);
        }


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

            _expSystem = new RelationshipUEXPSystem();

            _expSystem.SubOnLevelUp(OnlevelUp);

        }
        ~RuntimeCharacterRelationshipStatus()
        {
            _expSystem.UnSubOnLevelUp(OnlevelUp);

        }

        #region GETTER

        public int GetCurrentEXP
        {
            get
            {
                if (_expSystem == null)
                    throw new Exception("_expSystem is null"); 

                return _expSystem.GetCurrentLevel; 
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

        }
        
    }
}
