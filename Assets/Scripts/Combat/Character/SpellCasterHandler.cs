using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vanaring_DepaDemo;

/// <summary>
/// Used in CombatEntity class to handle casting, modify energy, and stuffs about spell 
/// </summary>
/// 
public class SpellCasterHandler
{
    [SerializeField]
    private List<SpellAbilitySO> _spellAbilities;

    public List<SpellAbilitySO> SpellAbilities => _spellAbilities;

    private RuntimeMangicalEnergy _mangicalEnergy; 
    public SpellCasterHandler()
    {
        _mangicalEnergy = new RuntimeMangicalEnergy();
    }

    public bool IsEnergySufficient(SpellAbilitySO spell)
    {
        return GetEnergyAmount(spell.EnergySide) >= spell.EnergyRequire  ; 
    }
    #region Modify Energy  

    public int GetEnergyAmount(RuntimeMangicalEnergy.EnergySide side)
    {
        return _mangicalEnergy.GetEnergy(side);
    }

    public void ModifyEnergy(int value, RuntimeMangicalEnergy.EnergySide side)
    {
        _mangicalEnergy.ModifyEnergy(value, side);
    }
    #endregion

}

public class RuntimeMangicalEnergy
{
    private RuntimeStat _darkEnergy = new RuntimeStat(100, 50);
    private RuntimeStat _lightEnergy = new RuntimeStat(100, 50);

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

        throw new System.Exception("Trying to access invalid side of energy");
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
            case EnergySide.LightEnergy:
                _lightEnergy.ModifyValue(value);
                _darkEnergy.ModifyValue(-value);
                break;
            case EnergySide.DarkEnergy:
                _lightEnergy.ModifyValue(-value);
                _darkEnergy.ModifyValue(value);
                break;
            default:
                throw new System.Exception("Trying to access invalid side of energy");
        }
    }

    #endregion
}