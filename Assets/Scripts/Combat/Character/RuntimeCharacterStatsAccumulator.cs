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

 

namespace Vanaring_DepaDemo 
{

    public enum ECharacterStatType
    {
        HP,ATK, Stunt 
    }

    public class RuntimeCharacterStatsAccumulator  
    {
        private Dictionary<ECharacterStatType, RuntimeStat> _characterStats = new Dictionary<ECharacterStatType, RuntimeStat>() ;
        public RuntimeCharacterStatsAccumulator(CharacterSheetSO _entityStatsSO)
        {
            _characterStats.Add(ECharacterStatType.HP, new RuntimeStat(_entityStatsSO.GetHP, _entityStatsSO.GetHP));
            _characterStats.Add(ECharacterStatType.ATK, new RuntimeStat(VanaringMathConst.InfinityValue ,_entityStatsSO.GetATK)) ;
            _characterStats.Add(ECharacterStatType.Stunt, new RuntimeStat(1,0));

        }

        #region ATKStatsManipulationMethod  
        public void ModifyATKAmount(int atk)
        {
            _characterStats[ECharacterStatType.ATK].ModifyValue(atk,true,true) ;

        }

        public void ModifyATKAmountByPercent(int percent)
        {
            Debug.Log("increased ATK by percent " + percent);


            int currentValue = _characterStats[ECharacterStatType.ATK].GetStatValue() ; 
            int increasedAmount = (currentValue * percent ) / 100 ;
            Debug.Log("increased ATK by   " + increasedAmount);
            Debug.Log("before increase " + _characterStats[ECharacterStatType.ATK].GetStatValue());

            _characterStats[ECharacterStatType.ATK].ModifyValue(increasedAmount, true, true);

            Debug.Log("after increase " + _characterStats[ECharacterStatType.ATK].GetStatValue());

        }

        public int GetATKAmount()
        {
            return _characterStats[ECharacterStatType.ATK].GetStatValue();
        }
        #endregion

        #region HPStatsManipulationmethod
        public void ModifyHPStat(float amt)
        {
            _characterStats[ECharacterStatType.HP].ModifyValue((int)amt,false);
        }

        public int GetHPAmount()
        {
            return _characterStats[ECharacterStatType.HP].GetStatValue();  
        }
        #endregion

        #region StuntManipulationMethod 

        public void ApplyStunt()
        {
            _characterStats[ECharacterStatType.Stunt].ResetIncreasedValue() ; 
            _characterStats[ECharacterStatType.Stunt].ModifyValue(1) ; 
        } 

        public void RemoveStunt()
        {
            _characterStats[ECharacterStatType.Stunt].ResetIncreasedValue();
        }

        public bool IsStunt()
        {
            return  _characterStats[ECharacterStatType.Stunt].GetStatValue() == 1; 
        }

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
