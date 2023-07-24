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
    public class ItemSocketGUI : MonoBehaviour  
    {

        [SerializeField]
        private Button _actionButton ;

        [SerializeField]
        private TextMeshProUGUI _textMeshProUGUI ;
        [SerializeField]
        private TextMeshProUGUI _textMeshProNum ;

        private string _itemName;


        [SerializeField]
        private Image _skillImage;  

        private ItemAbilitySO _itemSO;

        private CombatEntity _caster;

        private int _itemAmount;

        public void Init(ItemAbilitySO item, CombatEntity combatEntity)
        { 
            _itemSO = item;
            this._caster = combatEntity;
            _actionButton.onClick.AddListener(ChooseItem) ;

            _textMeshProUGUI.text = item.AbilityName.ToString() ;
            _itemName = item.AbilityName.ToString();
            _itemAmount = 1;
        }

        public void AddItem(int amount)
        {
            _itemAmount += amount;
            _textMeshProNum.text = "x" + _itemAmount.ToString();
        }

        public bool IsSameItem(string amount)
        {
            return (_itemName == amount);
        }

        private void ChooseItem()
        {
            
            //SpellAbilityRuntime runtimeSpell = _itemSO.Factorize();
            //if (_caster.SpellCaster.IsEnergySufficient(runtimeSpell))
            //{
            //    StartCoroutine((TargetSelectionFlowControl.Instance.InitializeSpellTargetSelectionScheme
            //       (_caster, runtimeSpell)));
            //}
        }
    }
}
