using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
        private RuntimeMangicalEnergy _mangicalEnergy;

        private UnityAction<CombatEntity, RuntimeMangicalEnergy.EnergySide, int> OnModifyEnergy;

        private CombatEntity _combatEntity;

        private RuntimeMangicalEnergy _simulateMagicalEnergy = null;

        private void Awake()
        {
            _mangicalEnergy = new RuntimeMangicalEnergy(_mangicalEnergy) ;
            _combatEntity = GetComponent<CombatEntity>();
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
            return _mangicalEnergy.GetEnergy(side);
        }

        public void ModifyEnergy(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int value)
        {
            _mangicalEnergy.ModifyEnergy(value, side);
            OnModifyEnergy?.Invoke(caster, side, value);
        }

        public bool IsEnergyOverflow()
        {
            return _mangicalEnergy.IsOverheat();
        }

        public void ResetEnergy()
        {
            int modifiedAmout = 0;
            RuntimeMangicalEnergy.EnergySide modifiedSide = RuntimeMangicalEnergy.EnergySide.LightEnergy; 

            (modifiedAmout, modifiedSide) = _mangicalEnergy.ResetEnergy();

            OnModifyEnergy?.Invoke(null, modifiedSide, modifiedAmout) ;
        }

        #endregion

        #region Spell
        public void CastSpell(SpellActionSO spellSO)
        {
            SpellAbilityRuntime runtimeSpell = spellSO.Factorize(_combatEntity);

            StartCoroutine(TargetSelectionFlowControl.Instance.InitializeActionTargetSelectionScheme(_combatEntity, runtimeSpell));
        }

 
        #endregion

        #region Interface 
        public void Simulate(RuntimeMangicalEnergy.EnergySide argc, int argv, Null arga = null)
        {

            if (_simulateMagicalEnergy == null)
            {
                _simulateMagicalEnergy = new RuntimeMangicalEnergy(_mangicalEnergy);
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
    }

    [Serializable]
    public class RuntimeMangicalEnergy
    {
        [Header("Initial Values for Dark and Light Properties")]
        [SerializeField]
        private int _darkDefaultAmount = 0;
        [SerializeField]
        private int _lightDefaultAmount = 0;

        protected RuntimeStat _darkEnergy ;
        protected RuntimeStat _lightEnergy  ;

        public enum EnergySide
        {
            LightEnergy = 0,
            DarkEnergy = 1
        }

        #region Methods 

        public RuntimeMangicalEnergy(RuntimeMangicalEnergy copied)
        {
            _darkDefaultAmount =  copied._darkDefaultAmount ;
            _lightDefaultAmount = copied._lightDefaultAmount ;
            int peakVal = _darkDefaultAmount + _lightDefaultAmount; 
            _darkEnergy = new RuntimeStat(peakVal, _darkDefaultAmount);
            _lightEnergy = new RuntimeStat(peakVal, _lightDefaultAmount);
        }

        /// <summary>
        ///  Modify Runtime Energy of the user
        /// </summary>
        public void ModifyEnergy(int value, EnergySide side)
        {
            if (value < 0)
                throw new System.Exception("Value is negative, this can result in incorrect modification of energy");

            switch (side)
            {
                case EnergySide.LightEnergy:
                    _lightEnergy.ModifyValue(value, false,true);
                    _darkEnergy.ModifyValue(-value, false,true);
                    break;
                case EnergySide.DarkEnergy:
                    _lightEnergy.ModifyValue(-value, false,true);
                    _darkEnergy.ModifyValue(value, false, true);
                    break;
                default:
                    throw new System.Exception("Trying to access invalid side of energy");
            }
        }
        #endregion

        #region GETTER 
        public int GetEnergy(EnergySide side)
        {
            if (side == EnergySide.LightEnergy)
                return _lightEnergy.GetStatValue();
            else if (side == EnergySide.DarkEnergy)
                return _darkEnergy.GetStatValue();

            throw new System.Exception("Trying to access invalid side of energy");
        }
        public bool IsOverheat()
        {
            int peakVal = _darkDefaultAmount + _lightDefaultAmount;
            return (_darkEnergy.GetStatValue() >= peakVal) || (_lightEnergy.GetStatValue() >= peakVal)  ;
        }

        public (int, EnergySide) ResetEnergy()
        {
            EnergySide modifiedSide;
            int modifiedAmount = 0; 
            //Overheat dark
            if (_darkEnergy.GetStatValue() > _lightEnergy.GetStatValue())
            {
                modifiedSide = EnergySide.LightEnergy;
                modifiedAmount = _lightDefaultAmount ;
            }
            //Overheat light 
            else
            {
                modifiedSide = EnergySide.DarkEnergy;
                modifiedAmount = _darkDefaultAmount ;
            }

            int peakVal = _darkDefaultAmount + _lightDefaultAmount;

            _darkEnergy = new RuntimeStat(peakVal, _darkDefaultAmount);
            _lightEnergy = new RuntimeStat(peakVal, _lightDefaultAmount);

            return (modifiedAmount, modifiedSide); 
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