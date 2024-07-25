using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vanaring.Assets.Scripts.Utilities.StringConstant;

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
            _characterUEXPSystem = new CharacterUEXPSystem(currentLevel, currentEXP);
            _characterUEXPSystem.SubOnLevelUp(DebugOnLevelUpTest);
        }

        private void DebugOnLevelUpTest(int lvl)
        {
            //ColorfulLogger.LogWithColor("Upgrade Level to " + lvl, Color.red); 
        }

        public void ReceiveEXP(float exp)
        {
            _characterUEXPSystem.ReceiveEXP(exp) ; 
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
        public int GetVitality => _runtimeCombatMemberData.GetCharacterSheet.GetVitality + (_runtimeCombatMemberData.GetCharacterSheet.GetModVitality * (_characterUEXPSystem.GetCurrentLevel - 1 ) ) ;
        public int GetIntellect => _runtimeCombatMemberData.GetCharacterSheet.GetIntellect + (_runtimeCombatMemberData.GetCharacterSheet.GetModIntellect * (_characterUEXPSystem.GetCurrentLevel - 1));
        public int GetStrength=> _runtimeCombatMemberData.GetCharacterSheet.GetStrength + (_runtimeCombatMemberData.GetCharacterSheet.GetModStrength * (_characterUEXPSystem.GetCurrentLevel - 1));
        public int GetAgility => _runtimeCombatMemberData.GetCharacterSheet.GetAgility + (_runtimeCombatMemberData.GetCharacterSheet.GetModAgility * (_characterUEXPSystem.GetCurrentLevel - 1));
        
        #endregion

        #region Get Secondary Attribute 
        public int GetSecondaryAttribute_MaxHP => AttributeFormulaLocator.CalculateMaxHP(GetVitality); // 50 + (GetVitality * 14); 
        public int GetSecondaryAttribute_MaxMP => AttributeFormulaLocator.CalculateMaxMP(GetIntellect);  
        public int GetSecondaryAttribute_PhysicalATK => AttributeFormulaLocator.CalculatePhysicalATK(GetStrength); 
        public int GetSecondaryAttribute_MagicalATK => AttributeFormulaLocator.CalculateMagicalATK(GetIntellect); 
        public float GetSecondaryAttribute_Evasion => AttributeFormulaLocator.CalculateEvasion(GetAgility);  
        public float GetSecondaryAttribute_ACC => AttributeFormulaLocator.CalculateACC(GetAgility) ;

        #endregion


    }

}
