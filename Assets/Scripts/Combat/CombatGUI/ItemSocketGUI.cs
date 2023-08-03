﻿using System;
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

        private ItemAbilityRuntime _item ;

        private CombatEntity _caster;

        private int _itemAmount;

        public void Init(ItemAbilityRuntime item, CombatEntity combatEntity)
        {
            _item = item;
            this._caster = combatEntity;
            _actionButton.onClick.AddListener(ChooseItem) ;

            _textMeshProUGUI.text = item.ItemName.ToString() ;
            _itemName = item.ItemName.ToString();
            _itemAmount = 1;
        }

        public void AddItem(int amount)
        {
            _itemAmount += amount;
            _textMeshProNum.text = "x" + _itemAmount.ToString();
        }

        public void SetNumberOfItem(int amount)
        {
            _itemAmount = amount;
            _textMeshProNum.text = "x" + _itemAmount.ToString();
        }

        public bool IsSameItem(string name)
        {
            return (_itemName == name);
        }

        private void ChooseItem()
        {
            _caster.ItemUser.UseItem(_item);
        }

        public int ItemAmount => _itemAmount;
    }
}
