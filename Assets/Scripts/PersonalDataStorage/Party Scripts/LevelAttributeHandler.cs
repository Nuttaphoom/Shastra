using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring 
{
    public class LevelAttributeHandler
    {
        private CharacterUEXPSystem _characterUEXPSystem ;  

        private RuntimeCombatMemberData _runtimeCombatMemberData ;
        public LevelAttributeHandler(RuntimeCombatMemberData runtimeCombatData) {
            _runtimeCombatMemberData = runtimeCombatData;
            _characterUEXPSystem = new CharacterUEXPSystem(1,0) ;
        }

        public void RestoreLevelDataFromLocalSave( int currentLevel, float currentEXP)
        {
            _characterUEXPSystem = new CharacterUEXPSystem(currentLevel,currentEXP);
        }

        #region Getter 
        public CharacterUEXPSystem GetCharacterUEXPSystem
        {
            get
            {
                if (_characterUEXPSystem == null)
                    throw new Exception("_characterUEXPSystem never been restored"); 

                return _characterUEXPSystem;
            }
        }
        #endregion

        #region Get Primary Attribute   
        //Rightn now we use Base + Multiplier * Level
        // so we don't have abilities to choose which upgrade player want 
        public int GetVitality => _runtimeCombatMemberData.GetCharacterSheet.GetVitality + (_runtimeCombatMemberData.GetCharacterSheet.GetModVitality * _characterUEXPSystem.GetCurrentLevel) ;
        public int GetIntellect => _runtimeCombatMemberData.GetCharacterSheet.GetIntellect + (_runtimeCombatMemberData.GetCharacterSheet.GetModIntellect * _characterUEXPSystem.GetCurrentLevel);
        public int GetStrength=> _runtimeCombatMemberData.GetCharacterSheet.GetStrength + (_runtimeCombatMemberData.GetCharacterSheet.GetModStrength * _characterUEXPSystem.GetCurrentLevel);
        public int GetAgility => _runtimeCombatMemberData.GetCharacterSheet.GetAgility + (_runtimeCombatMemberData.GetCharacterSheet.GetModAgility * _characterUEXPSystem.GetCurrentLevel);
        
        #endregion

        #region Get Secondary Attribute 
        public int GetSecondaryAttribute_MaxHP => 50 + (GetVitality * 14);
        public int GetSecondaryAttribute_MaxMP => 70 + (GetIntellect * 4);
        public int GetSecondaryAttribute_PhysicalATK => 20 + (GetStrength * 3);
        public int GetSecondaryAttribute_MagicalATK => 15 + (GetIntellect * 3);
        public float GetSecondaryAttribute_Evasion => 100 + ((GetAgility));
        public float GetSecondaryAttribute_ACC => 190 + (GetAgility);

        #endregion


    }

}
