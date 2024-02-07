using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Vanaring.Assets.Scripts.Utilities.StringConstant;

namespace Vanaring 
{
    public class RelationshipHandler
    {
        private CharacterSheetDatabaseSO m_characterSheetDatabase;
        private List<RuntimeCharacterRelationshipStatus> characterRelationshipStatuses = new List<RuntimeCharacterRelationshipStatus>(); 
        
        public RelationshipHandler()
        {

        }

        public void LoadRelationStatusFromDatabase()
        {
            LoadCharacterDatabaseOP();

            if (characterRelationshipStatuses != null)
                throw new System.Exception("Try to laod spell from data base multiple time.This isn't allowed. " +
                    "The system should be loaded only 1 time when the save is loaded, and modified the SpellAction thoughtout the lifetime of application, " +
                    "and save the uniqueID when the game is saved");

            characterRelationshipStatuses = new List<RuntimeCharacterRelationshipStatus>();

            foreach ( var characterSheet in m_characterSheetDatabase.GetNormalCharacterShhets()) {
                characterRelationshipStatuses.Add(new RuntimeCharacterRelationshipStatus(characterSheet));
            }
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

    public class RuntimeCharacterRelationshipStatus
    {
        private RelationshipUEXPSystem _expSystem;

        private CharacterSheetSO _characterSheetSO;
        public RuntimeCharacterRelationshipStatus(CharacterSheetSO cs)
        {
            _characterSheetSO = cs;

            _expSystem = new RelationshipUEXPSystem();

            _expSystem.SubOnLevelUp(OnlevelUp);  

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


        ~RuntimeCharacterRelationshipStatus()
        {
            _expSystem.UnSubOnLevelUp(OnlevelUp);

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
