using System;
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
        HP,ATK
    }

    public class RuntimeCharacterStatsAccumulator  
    {
        private Dictionary<ECharacterStatType, RuntimeStat> _characterStats = new Dictionary<ECharacterStatType, RuntimeStat>() ;
        public RuntimeCharacterStatsAccumulator(CharacterSheetSO _entityStatsSO)
        {
            _characterStats.Add(ECharacterStatType.HP, new RuntimeStat(_entityStatsSO.GetHP, _entityStatsSO.GetHP));
            _characterStats.Add(ECharacterStatType.ATK, new RuntimeStat(_entityStatsSO.GetATK, _entityStatsSO.GetATK)) ; 
        }

        #region ATKStatsManipulationMethod  
        public void ModifyATKAmount(int atk)
        {
            _characterStats[ECharacterStatType.ATK].ModifyValue(atk);
        }

        public int GetATKAmount()
        {
            return _characterStats[ECharacterStatType.ATK].GetStatValue();
        }
        #endregion

        #region HPStatsManipulationmethod
        public void ModifyHPStat(float amt)
        {
            _characterStats[ECharacterStatType.HP].ModifyValue((int)amt);
        }

        public int GetHPAmount()
        {
            return _characterStats[ECharacterStatType.HP].GetStatValue();  
        }
        #endregion



        #region GETTER 

        #endregion
    }

}
