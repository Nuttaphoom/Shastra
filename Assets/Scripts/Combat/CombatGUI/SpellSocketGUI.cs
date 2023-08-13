using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace Vanaring_DepaDemo
{
    interface ISocketGUI<T>
    {
        public void HandleGUI(T s);
    }
    public class SpellSocketGUI : MonoBehaviour, ISocketGUI<SpellAbilitySO>
    {

        public void HandleGUI(SpellAbilitySO spell)
        {

        }

        [Header("Main Skill Information")]
        [SerializeField]
        private Button _actionButton ;

        [SerializeField]
        private TextMeshProUGUI _textMeshProUGUI;

        [SerializeField]
        private Image _skillImage;

        [SerializeField]
        private Image _spellEnergyCostType;

        [SerializeField]
        private TextMeshProUGUI _spellCost;

        [SerializeField]
        private Sprite _lightImage;
        [SerializeField]
        private Sprite _darkImage;

        [Header("Description Window")]
        [SerializeField] private TextMeshProUGUI _spellNameTextDes;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _requireEnergyCost;
        [SerializeField] private TextMeshProUGUI _modifiedEnergyCost;
        [SerializeField] private Image _reqEnergyTypeDesImg;
        [SerializeField] private Image _modEnergyTypeDesImg;

        private SpellAbilitySO _spellSO;

        private CombatEntity _caster; 

        public void Init(SpellAbilitySO spell, CombatEntity combatEntity)
        { 
            _spellSO = spell;
            this._caster = combatEntity;
            _actionButton.onClick.AddListener(ChooseSpell);

            _textMeshProUGUI.text = spell.AbilityName.ToString();
            _spellNameTextDes.text = spell.AbilityName.ToString();
            _descriptionText.text = spell.Desscription.ToString();
            _spellCost.text = spell.RequiredEnergy.Amount.ToString();
            _requireEnergyCost.text = "> " + spell.RequiredEnergy.Amount.ToString();
            _modifiedEnergyCost.text = "+ " + spell.EnergyModifer.Amount.ToString();

            if (spell.RequiredEnergy.Side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                _spellEnergyCostType.sprite = _lightImage;
                _reqEnergyTypeDesImg.sprite = _lightImage;
            }
            else
            {
                _spellEnergyCostType.sprite = _darkImage;
                _reqEnergyTypeDesImg.sprite = _darkImage;
            }
            
            if(spell.EnergyModifer.Side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                _modEnergyTypeDesImg.sprite = _lightImage;
            }
            else
            {
                _modEnergyTypeDesImg.sprite = _darkImage;
            }
        }

        private void ChooseSpell()
        {
            SpellAbilityRuntime runtimeSpell = _spellSO.Factorize();
            if (_caster.SpellCaster.IsEnergySufficient(runtimeSpell))
            {
                _caster.SpellCaster.CastSpell(runtimeSpell)  ; 
            }
        }
    }
}
