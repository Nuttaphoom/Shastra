using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring_DepaDemo
{
    [CreateAssetMenu( fileName = "CharacterSheet",menuName = "ScriptableObject/Combat/Character/CharacterSheet" )]
    public class CharacterSheetSO : ScriptableObject
    {
        [Header("Peak HP of this entity")]
        [SerializeField]
        private int _HP;

        [Header("Default ATK of this entity")]
        [SerializeField]
        private int _ATK;

        public int GetHP => _HP;

        public int GetATK => _ATK;
    }

    public class RuntimeMangicalEnergy
    {
        private RuntimeStat _darkEnergy = new RuntimeStat(100,50) ;
        private RuntimeStat _lightEnergy = new RuntimeStat(100,50);

        public enum EnergySide
        {
            LightEnergy = 0,
            DarkEnergy = 1
        }

        #region GETTER 
        public int GetEnergy(EnergySide side)
        {
            if (side == EnergySide.LightEnergy)
                return _lightEnergy.GetStatValue();
            else if (side == EnergySide.DarkEnergy)
                return _darkEnergy.GetStatValue();
            
            throw new System.Exception("Trying to access invalid side of energy") ;
        }

        #endregion

        #region Methods 

        /// <summary>
        ///  Modify Runtime Energy of the user
        /// </summary>
        public void ModifyEnergy(int value, EnergySide side)
        {
            if (value < 0)
                throw new System.Exception("Value is negative, this can result in incorrect modification of energy");

            switch (side)
            {
                case EnergySide.LightEnergy :
                    _lightEnergy.ModifyValue(value); 
                    _darkEnergy.ModifyValue(-value);
                    break; 
                case EnergySide.DarkEnergy :
                    _lightEnergy.ModifyValue(-value);
                    _darkEnergy.ModifyValue(value);
                    break; 
                default:
                    throw new System.Exception("Trying to access invalid side of energy");   
            }
        }

        #endregion 
    }

    public class RuntimeStat
    {
        private int _defaultValue ;
        private int _peakValue;
        private int _increasedValue;

        public RuntimeStat(int peakValue, int defaultValue)
        {
            _defaultValue = defaultValue;
            _peakValue = peakValue;
            _increasedValue = 0;
        }

        #region Methods 
        public void ResetIncreasedValue()
        {
            _increasedValue = 0;
        }

        //value can be negative 
        public void ModifyValue(int value)
        {
            _increasedValue += value;
            if (_increasedValue + _defaultValue > _peakValue)
                _increasedValue = _peakValue - _defaultValue;
        }

        public void ModifyPeakValue(int value)
        {
            _peakValue += value;
        }

        #endregion

        #region GETTER 
        public int GetStatValue()
        {
            return (_increasedValue + _defaultValue);
        }
        #endregion


    }
}
