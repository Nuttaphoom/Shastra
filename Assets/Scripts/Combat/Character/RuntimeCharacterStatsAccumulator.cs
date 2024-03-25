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

    //public enum ECharacterPrimaryAttributes 
    //{
    //    Strength, 
    //    Vitality, 
    //    Intellect, 
    //    Agility, 
    //    Luck
    //}

    public enum ECharacterSecondaryAttributes
    {
        HP,
        MP,
        PhysicalATK, 
        MagicalATK
    }

    public class RuntimeCharacterStatsAccumulator  
    {
        //private Dictionary<ECharacterPrimaryAttributes, CharacterStat> _characterPrimaryAttributes = new Dictionary<ECharacterPrimaryAttributes, CharacterStat>();
        private Dictionary<ECharacterSecondaryAttributes, CharacterStat> _characterSecondaryAttributes = new Dictionary<ECharacterSecondaryAttributes, CharacterStat>();
        public RuntimeCharacterStatsAccumulator(CombatCharacterSheetSO _entityStatsSO)
        {
            //Setup Primary Attributes 
            //_characterPrimaryAttributes.Add(ECharacterPrimaryAttributes.Strength, new CharacterStat(_entityStatsSO.GetStrength ) );
            //_characterPrimaryAttributes.Add(ECharacterPrimaryAttributes.Vitality, new CharacterStat(_entityStatsSO.GetVitality));
            //_characterPrimaryAttributes.Add(ECharacterPrimaryAttributes.Intellect, new CharacterStat(_entityStatsSO.GetIntellect));
            //_characterPrimaryAttributes.Add(ECharacterPrimaryAttributes.Agility, new CharacterStat(_entityStatsSO.GetAgility));
            //_characterPrimaryAttributes.Add(ECharacterPrimaryAttributes.Luck, new CharacterStat(_entityStatsSO.GetLuck));

            //Setup Secondary Attributes 
            //Mostly formula that transfer Primary stats into Secondary stats
            int MaxHP = _entityStatsSO.GetSecondaryAttribute_MaxHP;
            int MaxMP = _entityStatsSO.GetSecondaryAttribute_MaxMP;
            int PhysicalATK = _entityStatsSO.GetSecondaryAttribute_PhysicalATK;
            int MagicalATK = _entityStatsSO.GetSecondaryAttribute_PhysicalATK;

            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.HP, new CharacterStat(MaxHP, MaxHP) ) ;
            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.PhysicalATK, new CharacterStat(PhysicalATK, PhysicalATK));
            _characterSecondaryAttributes.Add(ECharacterSecondaryAttributes.MagicalATK, new CharacterStat(MagicalATK, MagicalATK));

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


        #region GETTER 

        #endregion
    }

}
