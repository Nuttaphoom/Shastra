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

        [SerializeField]
        private Button _actionButton ;

        [SerializeField]
        private TextMeshProUGUI _textMeshProUGUI;

        [SerializeField]
        private Image _skillImage;

        [SerializeField]
        private TextMeshProUGUI _descriptionText;

        [SerializeField]
        private Image _spellEnergyCostType;

        [SerializeField]
        private TextMeshProUGUI _spellCost;

        [SerializeField]
        private Sprite _lightImage;
        [SerializeField]
        private Sprite _darkImage;

        private SpellAbilitySO _spellSO;

        private CombatEntity _caster; 

        public void Init(SpellAbilitySO spell, CombatEntity combatEntity)
        { 
            _spellSO = spell;
            this._caster = combatEntity;
            _actionButton.onClick.AddListener(ChooseSpell ) ;

            _textMeshProUGUI.text = spell.AbilityName.ToString() ;
            _descriptionText.text = spell.Desscription.ToString();
            _spellCost.text = spell.RequiredEnergy.Amount.ToString();
            if(spell.RequiredEnergy.Side == RuntimeMangicalEnergy.EnergySide.LightEnergy)
            {
                _spellEnergyCostType.sprite = _lightImage;
            }
            else
            {
                _spellEnergyCostType.sprite = _darkImage;
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
