using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

 

namespace Vanaring
{

    public enum ECharacterStatType
    {
        HP,ATK   
    }

    public class RuntimeCharacterStatsAccumulator  
    {
        private Dictionary<ECharacterStatType, RuntimeStat> _characterStats = new Dictionary<ECharacterStatType, RuntimeStat>() ;
        public RuntimeCharacterStatsAccumulator(CombatCharacterSheetSO _entityStatsSO)
        {
            _characterStats.Add(ECharacterStatType.HP, new RuntimeStat(_entityStatsSO.GetHP, _entityStatsSO.GetHP));
            _characterStats.Add(ECharacterStatType.ATK, new RuntimeStat(VanaringMathConst.InfinityValue ,_entityStatsSO.GetATK)) ;
 

        }

        #region ATKStatsManipulationMethod  
        public void ModifyATKAmount(int atk)
        {
            _characterStats[ECharacterStatType.ATK].ModifyValue(atk,true,true) ;

        }

        public void ModifyATKAmountByPercent(int percent)
        {


            int currentValue = _characterStats[ECharacterStatType.ATK].GetStatValue() ; 
            int increasedAmount = (currentValue * percent ) / 100 ;
 

            _characterStats[ECharacterStatType.ATK].ModifyValue(increasedAmount, true, true);


        }

        public int GetATKAmount()
        {
            return _characterStats[ECharacterStatType.ATK].GetStatValue();
        }
        #endregion

        #region HPStatsManipulationmethod
        public int ModifyHPStat(float amt)
        {
            return _characterStats[ECharacterStatType.HP].ModifyValue((int)amt,false);
        }

        public int GetHPAmount()
        {
            return _characterStats[ECharacterStatType.HP].GetStatValue();  
        }

        public int GetPeakHPAmount()
        {
            return _characterStats[ECharacterStatType.HP].GetPeakValue();
        }
        #endregion

        #region StuntManipulationMethod 

        

    

       

        public IEnumerator ResetTemporaryIncreasedValue()
        {
            foreach (ECharacterStatType type in _characterStats.Keys)
            {
                _characterStats[type].ResetIncreasedValue();
                //yield for some sec for removing status animation 
                yield return null; 
            }
        } 
        #endregion


        #region GETTER 

        #endregion
    }

}
