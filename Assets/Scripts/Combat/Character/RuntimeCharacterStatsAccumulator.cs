using Kryz.CharacterStats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

 

namespace Vanaring
{ 
    public enum ECharacterSecondaryAttributes
    {
        HP,
        MP,
        PhysicalATK, 
        MagicalATK,
        Accuracy, 
        Evasion
    }

    public class RuntimeCharacterStatsAccumulator  
    {
        //private Dictionary<ECharacterPrimaryAttributes, CharacterStat> _characterPrimaryAttributes = new Dictionary<ECharacterPrimaryAttributes, CharacterStat>();
        private Dictionary<ECharacterSecondaryAttributes, CharacterStat> _characterSecondaryAttributes = new Dictionary<ECharacterSecondaryAttributes, CharacterStat>();
       
        /// <summary>
        /// Use to initialize Party member entity
        /// this function will update HP and other status of the member depending on the runtime value of that Party member
        /// </summary>
        /// <param name="runtimePartyMember"></param>
        public RuntimeCharacterStatsAccumulator(RuntimePartyMember runtimePartyMember)
        {
            LevelAttributeHandler levelAttributeHandler = runtimePartyMember.GetRuntimeCombatMemberData.LevelAttributeHandler; //.GetBaseSecondaryAttribute_MaxHP;

            //Setup Secondary Attributes 
            //Mostly formula that transfer Primary stats into Secondary stats
            int MaxHP = levelAttributeHandler.GetSecondaryAttribute_MaxHP;
            int PhysicalATK = levelAttributeHandler.GetSecondaryAttribute_PhysicalATK;
            int MagicalATK = levelAttributeHandler.GetSecondaryAttribute_MagicalATK;
            float ACC = levelAttributeHandler.GetSecondaryAttribute_ACC ; 
            float Evasion = levelAttributeHandler.GetSecondaryAttribute_Evasion;

            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.HP, new CharacterStat(runtimePartyMember.GetCurrentPartyMemberHP, MaxHP) ) ;
            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.PhysicalATK, new CharacterStat(PhysicalATK, PhysicalATK));
            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.MagicalATK, new CharacterStat(MagicalATK, MagicalATK));
            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.Accuracy, new CharacterStat(ACC, ACC))  ;
            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.Evasion, new CharacterStat(Evasion, Evasion)) ;


        }

        /// <summary>
        /// Use to initialize Combat entity with given CombatCharacterSheetSO
        /// </summary>
        /// <param name="combatCharacterSheetSO"></param>
        public RuntimeCharacterStatsAccumulator(CombatCharacterSheetSO combatCharacterSheetSO)
        {
            //Mostly formula that transfer Primary stats into Secondary stats
            int MaxHP = combatCharacterSheetSO.Get_Base_SecondaryAttribute_MaxHP;
            int PhysicalATK = combatCharacterSheetSO.Get_Base_SecondaryAttribute_PhysicalATK;
            int MagicalATK = combatCharacterSheetSO.Get_Base_SecondaryAttribute_MagicalATK;
            float ACC = combatCharacterSheetSO.Get_Base_SecondaryAttribute_ACC;
            float Evasion = combatCharacterSheetSO.Get_Base_SecondaryAttribute_Evasion;


            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.HP, new CharacterStat(MaxHP, MaxHP));
            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.PhysicalATK, new CharacterStat(PhysicalATK, PhysicalATK));
            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.MagicalATK, new CharacterStat(MagicalATK, MagicalATK));
            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.Accuracy, new CharacterStat(ACC, ACC));
            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.Evasion, new CharacterStat(Evasion, Evasion)); 

        }



        #region ATKStatsManipulationMethod  
        public void ModifyPhysicalATKAmount(StatModifier mod)
        {
            //_characterStats[ECharacterStatType.ATK].ModifyValue(atk,true,true) ;
            _characterSecondaryAttributes[ECharacterSecondaryAttributes.PhysicalATK].AddModifier(mod); 
        }

        public void RemoveModifyPhysicalATK(StatModifier mod)
        {
            _characterSecondaryAttributes[ECharacterSecondaryAttributes.PhysicalATK].RemoveModifier(mod);
        }

        public void ModifyMagicalATKAmount(StatModifier mod)
        {
            //_characterStats[ECharacterStatType.ATK].ModifyValue(atk,true,true) ;
            _characterSecondaryAttributes[ECharacterSecondaryAttributes.MagicalATK].AddModifier(mod);
        }

        public void RemoveMagicalPhysicalATK(StatModifier mod)
        {
            _characterSecondaryAttributes[ECharacterSecondaryAttributes.MagicalATK].RemoveModifier(mod);

        }

        //public void ModifyATKAmountByPercent(int percent)
        //{
        //    int currentValue = _characterStats[ECharacterStatType.ATK].GetStatValue() ; 
        //    int increasedAmount = (currentValue * percent ) / 100 ;


        //    _characterStats[ECharacterStatType.ATK].ModifyValue(increasedAmount, true, true);


        //}

        public float GetPhysicalATKAmount()
        {
            float ret = _characterSecondaryAttributes[ECharacterSecondaryAttributes.PhysicalATK].Value ; 
            return ret ;
        }
        #endregion

        #region HPStatsManipulationmethod
        public void ModifyHPStat(StatModifier mod)
        {
            _characterSecondaryAttributes[ECharacterSecondaryAttributes.HP].AddModifier(mod) ;
        }

        public float GetHPAmount()
        {
            return _characterSecondaryAttributes[ECharacterSecondaryAttributes.HP].Value ;  
        }

        public float GetPeakHPAmount()
        {
            return _characterSecondaryAttributes[ECharacterSecondaryAttributes.HP].GetPeakValue ;
        }
        #endregion

        #region StuntManipulationMethod 
        //public IEnumerator ResetTemporaryIncreasedValue()
        //{
        //    foreach (ECharacterStatType type in _characterStats.Keys)
        //    {
        //        _characterStats[type].ResetIncreasedValue();
        //        //yield for some sec for removing status animation 
        //        yield return null; 
        //    }
        //} 
        #endregion

        #region Accuracy and Dodge Manipulation Method 
        public float GetAccuracyAmount()
        {
            return _characterSecondaryAttributes[ECharacterSecondaryAttributes.Accuracy].Value ; 
        }
        public float GetEvasionAmount()
        {
            return _characterSecondaryAttributes[ECharacterSecondaryAttributes.Evasion].Value ;
        }

        #endregion


        #region GETTER 

        #endregion
    }

}
