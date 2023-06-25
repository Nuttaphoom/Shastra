using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

 

namespace Vanaring_DepaDemo.Assets.Scripts.Combat.CharacterSheet
{

    public enum ECharacterStatType
    {
        HP,ATK
    }

    public class RuntimeCharacterStatsAccumulator  
    {

        [Header("Entity Base Stats")]
        private CharacterSheetSO _entityStatsSO;

        Dictionary<ECharacterStatType, RuntimeStat> _characterStats;

        private RuntimeMangicalEnergy _mangicalEnergy = new RuntimeMangicalEnergy();

        public RuntimeCharacterStatsAccumulator()
        {
            _mangicalEnergy = new RuntimeMangicalEnergy(); 

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

        #region Modify Energy  
       

        public int GetEnergyAmount(RuntimeMangicalEnergy.EnergySide side)
        {
            return _mangicalEnergy.GetEnergy(side) ;  
        }

        public void ModifyEnergy(int value, RuntimeMangicalEnergy.EnergySide side)
        {
            _mangicalEnergy.ModifyEnergy(value, side);
        }
        #endregion

        #region GETTER 

        #endregion
    }

}
