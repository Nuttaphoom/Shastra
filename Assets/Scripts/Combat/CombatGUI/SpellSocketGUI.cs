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
    
    public class SpellSocketGUI : MonoBehaviour
    {
        

        [SerializeField]
        private Button _actionButton ;

        [SerializeField]
        private TextMeshProUGUI _textMeshProUGUI ;

        [SerializeField]
        private Image _skillImage;  

        private SpellAbilitySO _spellSO;

        private CombatEntity _caster; 

        public void Init(SpellAbilitySO spell, CombatEntity combatEntity)
        { 
            _spellSO = spell;
            this._caster = combatEntity;
            _actionButton.onClick.AddListener(ChooseSpell ) ;
            _textMeshProUGUI.text = spell.AbilityName.ToString() ; 
        }

        private void ChooseSpell()
        {
            SpellAbilityRuntime runtimeSpell = _spellSO.Factorize();
            if (_caster.SpellCaster.IsEnergySufficient(runtimeSpell))
            {
                StartCoroutine((TargetSelectionFlowControl.Instance.InitializeSpellTargetSelectionScheme
                   (_caster, runtimeSpell)));
            }
        }
    }
}
