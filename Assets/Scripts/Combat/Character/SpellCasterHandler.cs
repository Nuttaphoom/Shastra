﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
 
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// Used in CombatEntity class to handle casting, modify energy, and stuffs about spell 
/// </summary>
/// 

namespace Vanaring
{
    [Serializable]
    public class SpellCasterHandler : MonoBehaviour, ISimulationApplier<RuntimeMangicalEnergy.EnergySide, int, Null>
    {
        [SerializeField]
        private RuntimeMangicalEnergy _magicalEnergy;

        private UnityAction<CombatEntity, RuntimeMangicalEnergy.EnergySide, int> OnModifyEnergy;

        private CombatEntity _combatEntity;

        private RuntimeMangicalEnergy _simulateMagicalEnergy = null;

        [Header("Balanced Style will synchonize")]
        [SerializeField]
        private bool _balancedEnergyStyle = false; 

        private void Awake()
        {
            _combatEntity = GetComponent<CombatEntity>();
            _magicalEnergy.Init(this); 
        }

        #region Event Sub
        public void SubOnModifyEnergy(UnityAction<CombatEntity, RuntimeMangicalEnergy.EnergySide, int> argc)
        {
            OnModifyEnergy += argc;
        }

        public void UnSubOnModifyEnergy(UnityAction<CombatEntity, RuntimeMangicalEnergy.EnergySide, int> argc)
        {
            OnModifyEnergy -= argc;
        }
        #endregion EndSub 

        #region Modify Energy  
        public bool IsEnergySufficient(SpellActionSO spell)
        {
            return GetEnergyAmount(spell.RequiredEnergy.Side) >= spell.RequiredEnergy.Amount;
        }
        public int GetEnergyAmount(RuntimeMangicalEnergy.EnergySide side)
        {
            return _magicalEnergy.GetEnergy(side);
        }

        public int GetPeakEnergyAmout(RuntimeMangicalEnergy.EnergySide side)
        {
            return _magicalEnergy.GetEnergyRuntimeStat(side).GetPeakValue();
        }

        public void ModifyEnergy(RuntimeMangicalEnergy.EnergySide side, int value)
        {
            
            int modValue = _magicalEnergy.ModifyEnergy(value, side);

            if (_balancedEnergyStyle)
            {
                RuntimeMangicalEnergy.EnergySide oppositeSide = (RuntimeMangicalEnergy.EnergySide)((int)(side + 1) % 2);
                _magicalEnergy.ModifyEnergy(-modValue, oppositeSide);
                OnModifyEnergy?.Invoke(_combatEntity, oppositeSide, -modValue);
            }


            OnModifyEnergy?.Invoke(_combatEntity, side, modValue);


        }

        public bool IsEnergyOverflow()
        {
            return _magicalEnergy.IsOverheat();
        }

        public void ResetEnergy()
        {
            Debug.Log("Reset energy in " +  gameObject);

            //RuntimeMangicalEnergy.EnergySide modifiedSide = RuntimeMangicalEnergy.EnergySide.LightEnergy;
            int lightModifiedAmout = _magicalEnergy.ResetEnergy(RuntimeMangicalEnergy.EnergySide.LightEnergy) ;
            int darkAmodifiedAmout = _magicalEnergy.ResetEnergy(RuntimeMangicalEnergy.EnergySide.DarkEnergy) ;

            Debug.Log("LightMod Amout " + lightModifiedAmout + " Dark amout " + darkAmodifiedAmout);



            if (_balancedEnergyStyle)
            {
                if (lightModifiedAmout != 0)
                    ModifyEnergy(RuntimeMangicalEnergy.EnergySide.LightEnergy, lightModifiedAmout);
                else if (darkAmodifiedAmout != 0)
                    ModifyEnergy(RuntimeMangicalEnergy.EnergySide.DarkEnergy, darkAmodifiedAmout);

                return; 
            }

            if (! _balancedEnergyStyle)
            {
                if (lightModifiedAmout != 0)
                    ModifyEnergy(RuntimeMangicalEnergy.EnergySide.LightEnergy, lightModifiedAmout);
                if (darkAmodifiedAmout != 0)
                    ModifyEnergy(RuntimeMangicalEnergy.EnergySide.DarkEnergy, darkAmodifiedAmout);
            }
            //(modifiedAmout, modifiedSide) = _mangicalEnergy.ResetEnergy();

            //OnModifyEnergy?.Invoke(null, modifiedSide, modifiedAmout) ;
        }

        #endregion

        #region Spell
        public void CastSpell(SpellActionSO spellSO)
        {
            SpellAbilityRuntime runtimeSpell = spellSO.FactorizeRuntimeAction(_combatEntity) as SpellAbilityRuntime;
            
            StartCoroutine(TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme(_combatEntity, runtimeSpell));
        }

 
        #endregion

        #region Interface 
        public void Simulate(RuntimeMangicalEnergy.EnergySide argc, int argv, Null arga = null)
        {

            if (_simulateMagicalEnergy == null)
            {
                _simulateMagicalEnergy = new RuntimeMangicalEnergy(_magicalEnergy);
            }


            _simulateMagicalEnergy.ModifyEnergy(argv, argc);
        }

        public bool CheckSimulation()
        {
            if (_simulateMagicalEnergy == null)
                return false; 

            bool ret = _simulateMagicalEnergy.IsOverheat() ; 
            
            _simulateMagicalEnergy = null ; 

            return ret ;  
        }
        #endregion

        #region GETTER
        public bool IsBalanceStyle => _balancedEnergyStyle; 

        #endregion
    }

    [Serializable]
    public class RuntimeMangicalEnergy
    {
        [Header("Initial Values for Dark and Light Properties")]
        [SerializeField]
        private int _darkDefaultAmount = 0;
        [SerializeField]
        private int _lightDefaultAmount = 0;

        //protected RuntimeStat _darkEnergy ;
        //protected RuntimeStat _lightEnergy;

        private SpellCasterHandler _spellCasterHandler; 

        private Dictionary<EnergySide, RuntimeStat> _energy = new Dictionary<EnergySide, RuntimeStat>() ; 
        public enum EnergySide
        {
            LightEnergy = 0,
            DarkEnergy = 1
        }

        #region Methods 

        public void Init(SpellCasterHandler spellCasterHandler) {
            _spellCasterHandler = spellCasterHandler;

            int peak = _darkDefaultAmount + _lightDefaultAmount;
            
            RuntimeStat darkEnergy;
            RuntimeStat lightEnergy; 

            if (_spellCasterHandler.IsBalanceStyle)
            {
                darkEnergy = new RuntimeStat(peak, _darkDefaultAmount);
                lightEnergy = new RuntimeStat(peak, _lightDefaultAmount);
            }else
            {
                darkEnergy = new RuntimeStat(_darkDefaultAmount, _darkDefaultAmount);
                lightEnergy = new RuntimeStat(_lightDefaultAmount, _lightDefaultAmount);
            }

            _energy = new Dictionary<EnergySide, RuntimeStat>();

            _energy.Add(EnergySide.DarkEnergy, darkEnergy);
            _energy.Add(EnergySide.LightEnergy, lightEnergy);
        }
        public RuntimeMangicalEnergy( RuntimeMangicalEnergy copied )
        {
            _spellCasterHandler = copied._spellCasterHandler ;

            _darkDefaultAmount = copied._darkDefaultAmount;
            _lightDefaultAmount = copied._lightDefaultAmount; 

            var darkEnergy = new RuntimeStat(copied.GetEnergyRuntimeStat(EnergySide.DarkEnergy)) ;
            var lightEnergy = new RuntimeStat(copied.GetEnergyRuntimeStat(EnergySide.LightEnergy)) ;

            _energy = new Dictionary<EnergySide, RuntimeStat>();

            _energy.Add(EnergySide.DarkEnergy, darkEnergy);
            _energy.Add(EnergySide.LightEnergy, lightEnergy);

        }

        /// <summary>
        ///  Modify Runtime Energy of the user
        /// </summary>
        public int ModifyEnergy(int value, EnergySide side)
        {
            //if (value < 0)
            //    throw new System.Exception("Value is negative, this can result in incorrect modification of energy");

            int v = value; 
            
            if (v + _energy[side].GetStatValue() < 0)  
                v = -_energy[side].GetStatValue();

            else if (v + _energy[side].GetStatValue() > _darkDefaultAmount + _lightDefaultAmount)
                v = (int) MathF.Abs(_darkDefaultAmount + _lightDefaultAmount  - _energy[side].GetStatValue() ) ;

            _energy[side].ModifyValue(v,false,false);

            return v;
            //switch (side)
            //{
            //    case EnergySide.LightEnergy:
            //        _lightEnergy.ModifyValue(value, false,true);
            //        _darkEnergy.ModifyValue(-value, false,true);
            //        break;
            //    case EnergySide.DarkEnergy:
            //        _lightEnergy.ModifyValue(-value, false,true);
            //        _darkEnergy.ModifyValue(value, false, true);
            //        break;
            //    default:
            //        throw new System.Exception("Trying to access invalid side of energy");
            //}
        }
        #endregion

        #region GETTER 
        public int GetEnergy(EnergySide side)
        {
            return _energy[side].GetStatValue() ;

            //if (side == EnergySide.LightEnergy)
            //    return _lightEnergy.GetStatValue();
            //else if (side == EnergySide.DarkEnergy)
            //    return _darkEnergy.GetStatValue();

            //throw new System.Exception("Trying to access invalid side of energy");
        }

        public RuntimeStat GetEnergyRuntimeStat(EnergySide side)
        {
            return _energy[side]; 
        }
        public bool IsOverheat()
        {
            bool noLightEnergyRemaining = (GetEnergy(EnergySide.LightEnergy) == 0) && (_lightDefaultAmount > 0);
            bool noDarkEnergyRemaining = (GetEnergy(EnergySide.DarkEnergy) == 0) && (_darkDefaultAmount > 0);

 
            return noLightEnergyRemaining || noDarkEnergyRemaining;

            //return (_darkEnergy.GetStatValue() >= peakVal) || (_lightEnergy.GetStatValue() >= peakVal)  ;
        }

        public int ResetEnergy(EnergySide side)
        {
            int defaultAmout = _darkDefaultAmount; 
            if (side == EnergySide.LightEnergy)
                defaultAmout = _lightDefaultAmount; 

            int diff =  _energy[side].GetStatValue() - defaultAmout ;
            return -1* diff; 

          

            //ModifyEnergy(darkDiff,EnergySide.DarkEnergy) ;
            //ModifyEnergy(lightDiff, EnergySide.LightEnergy);

            //EnergySide modifiedSide;
            //int modifiedAmount = 0; 
            ////Overheat dark
            //if (_darkEnergy.GetStatValue() > _lightEnergy.GetStatValue())
            //{
            //    modifiedSide = EnergySide.LightEnergy;
            //    modifiedAmount = _lightDefaultAmount ;
            //}
            ////Overheat light 
            //else
            //{
            //    modifiedSide = EnergySide.DarkEnergy;
            //    modifiedAmount = _darkDefaultAmount ;
            //}

            //int peakVal = _darkDefaultAmount + _lightDefaultAmount;

            //_darkEnergy = new RuntimeStat(peakVal, _darkDefaultAmount);
            //_lightEnergy = new RuntimeStat(peakVal, _lightDefaultAmount);

            //return (modifiedAmount, modifiedSide); 
        }

        #endregion
    }

    [Serializable]
    public struct EnergyModifierData
    {
        [SerializeField]
        private RuntimeMangicalEnergy.EnergySide _side;

        [SerializeField]
        private int _amount;

        public RuntimeMangicalEnergy.EnergySide Side { get { return _side; } }
        public int Amount { get { return _amount; } }
    }
}