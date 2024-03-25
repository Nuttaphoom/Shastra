using Kryz.CharacterStats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

 

namespace Vanaring
{ 

    public enum ECharacterStatType
    {
        HP,ATK   
    }

    public class RuntimeCharacterStatsAccumulator  
    {
        private Dictionary<ECharacterStatType, CharacterStat> _characterStats = new Dictionary<ECharacterStatType, CharacterStat>() ;
        public RuntimeCharacterStatsAccumulator(CombatCharacterSheetSO _entityStatsSO)
        {
            _characterStats.Add(ECharacterStatType.HP, new CharacterStat(_entityStatsSO.GetHP, _entityStatsSO.GetHP) );
            _characterStats.Add(ECharacterStatType.ATK, new CharacterStat( _entityStatsSO.GetATK) ) ;

        }

        #region ATKStatsManipulationMethod  
        public void ModifyATKAmount(StatModifier mod)
        {
            //_characterStats[ECharacterStatType.ATK].ModifyValue(atk,true,true) ;
            _characterStats[ECharacterStatType.ATK].AddModifier(mod); 
        }

        public void RemoveModifyATK(StatModifier mod)
        {
            _characterStats[ECharacterStatType.ATK].RemoveModifier(mod);

        }

        //public void ModifyATKAmountByPercent(int percent)
        //{
        //    int currentValue = _characterStats[ECharacterStatType.ATK].GetStatValue() ; 
        //    int increasedAmount = (currentValue * percent ) / 100 ;


        //    _characterStats[ECharacterStatType.ATK].ModifyValue(increasedAmount, true, true);


        //}

        public float GetATKAmount()
        {
            float ret = _characterStats[ECharacterStatType.ATK].Value ; 
            return ret ;
        }
        #endregion

        #region HPStatsManipulationmethod
        public void ModifyHPStat(StatModifier mod)
        {
            _characterStats[ECharacterStatType.HP].AddModifier(mod) ;
        }

        public float GetHPAmount()
        {
            return _characterStats[ECharacterStatType.HP].Value ;  
        }

        public float GetPeakHPAmount()
        {
            return _characterStats[ECharacterStatType.HP].GetPeakValue ;
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
