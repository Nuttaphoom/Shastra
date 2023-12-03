using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vanaring
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

        [Header("Ailment Resistant")]
        [SerializeField]
        private AilmentResistantDataInfo _ailmentResistantDataInfo ; 

        [Header("Character Sprite")]
        [SerializeField]
        private Sprite _characterIconGUI;


        public Sprite GetCharacterIcon => _characterIconGUI;
        public int GetHP => _HP;
        public string CharacterName => _characterName;
        public int GetATK => _ATK;
        public AilmentResistantDataInfo ResistantData => _ailmentResistantDataInfo ;
    }

    

    public class RuntimeStat
    {
        private int _initialValue ;
        private int _peakValue;
        private int _increasedValue;

        public RuntimeStat(RuntimeStat copied)
        {
            _initialValue = copied._initialValue;
            _peakValue = copied._peakValue;
            _increasedValue = copied._increasedValue;

            //Debug.Log("increaesed : " + _defaultValue);
            //Debug.Log(" copied._defaultValue : " + copied._defaultValue);
            //Debug.Log(" copied.getvalue : " + copied.GetStatValue() ) ;
        }

        public RuntimeStat(int peakValue, int initialValue)
        {
            _initialValue = initialValue;
            _peakValue = peakValue;
            _increasedValue = 0;
        }

        #region Methods 
        public void ResetIncreasedValue()
        {
            _increasedValue = 0;
        }

        //value can be negative 
        public int ModifyValue(int value, bool temp = true, bool peakUpdate = false )
        {
            int ret = value;

            if (temp)
            {
                _increasedValue += value;
                if (_increasedValue + _initialValue > _peakValue)
                {
                    ret = _peakValue - (_increasedValue - value) ;

                    _increasedValue = _peakValue - _initialValue;
                }

                if (_increasedValue < 0) 
                    _increasedValue = 0;

            }
            else
            {
                _initialValue += value ;
                if (peakUpdate)
                    _peakValue += value;

                if (_initialValue < 0)
                    _initialValue = 0;

                else if (_initialValue > _peakValue)
                {
                    ret =  _peakValue - (_initialValue - value) ;
                    _initialValue = _peakValue;
                }
            }
            return ret; 
        } 

        public void ModifyPeakValue(int value)
        {
            _peakValue += value;
        }

        #endregion

        #region GETTER 
        public int GetStatValue()
        {
            return (_increasedValue + _initialValue);
        }

        public int GetPeakValue()
        {
            return (_peakValue); 
        }

        #endregion


    }
}
