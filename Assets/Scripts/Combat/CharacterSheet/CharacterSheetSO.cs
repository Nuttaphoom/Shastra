using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring_DepaDemo
{
    [CreateAssetMenu( fileName = "CharacterSheet",menuName = "ScriptableObject/Combat/Character/CharacterSheet" )]
    public class CharacterSheetSO : ScriptableObject
    {
        [SerializeField]
        private string _characterName = "No name"; 

        [Header("Peak HP of this entity")]
        [SerializeField]
        private int _HP;

        [Header("Default ATK of this entity")]
        [SerializeField]
        private int _ATK;

        public int GetHP => _HP;
        public string CharacterName => _characterName;
        public int GetATK => _ATK;
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
        public void ModifyValue(int value, bool temp = true, bool peakUpdate = false )
        {
            if (temp)
            {
                _increasedValue += value;
                if (_increasedValue + _defaultValue > _peakValue)
                    _increasedValue = _peakValue - _defaultValue;
                if (_increasedValue < 0)
                    _increasedValue = 0;
            }
            else
            {
                _defaultValue += value ;
                if (peakUpdate)
                    _peakValue += value;

                if (_defaultValue < 0)
                    _defaultValue = 0;
                else if (_defaultValue > _peakValue)
                    _defaultValue = _peakValue;
            }
               
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
