﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace Vanaring 
{
    public class ItemSocketGUI : MonoBehaviour  
    {
        [Header("Button")]
        [SerializeField]
        private Button _actionButton ;
        [SerializeField]
        private TextMeshProUGUI _itemNameText ;
        [SerializeField]
        private TextMeshProUGUI _itemDescription;
        [SerializeField]
        private Image _itemIcon;
        [SerializeField]
        private TextMeshProUGUI _textMeshProNum ;

        [Header("Button")]
        [SerializeField]
        private Image _highlightButtonImg;

        private string _itemName;


        [SerializeField]
        private Image _skillImage;  

        private ItemAbilityRuntime _item ;

        private CombatEntity _caster;

        private int _itemAmount;

        private Color _hightlightedColor = Color.yellow;
        private Color _defaultColor;

        private bool _init = false; 
        public void Init(ItemAbilityRuntime item, CombatEntity combatEntity, int amount)
        {
            
            _init  = true; 
            _item = item;
            this._caster = combatEntity;
            _actionButton.onClick.AddListener(ChooseItem) ;
            _itemNameText.text = item.ItemName.ToString();
            _itemDescription.text = item.ItemDescrption.ToString();
            //_itemIcon.sprite = item.ItemSprite;
            _itemName = item.ItemName.ToString();
            _itemAmount = amount;
            _textMeshProNum.text = "x" + _itemAmount.ToString();
        }

        public string GetItemDescription()
        {
            return _item.ItemDescrption.ToString();
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
            if (_init)
            {
                Debug.Log("is init");
            }
            if (_caster == null)
            {
                Debug.Log("cast is null");
            } if ( _caster.ItemUser == null)
            {
                Debug.Log("caster itemuser is null"); 
            }if (_item == null)
            {
                Debug.Log("item is null");
            }
            _caster.ItemUser.UseItem(_item);
        }

        public void CallButtonCallback()
        {
            _actionButton.onClick?.Invoke();
        }
        public int ItemAmount => _itemAmount;

        public void HightlightedButton()
        {
            //_actionButton.GetComponent<Image>().sprite = _highlightButtonImg;
            _highlightButtonImg.gameObject.SetActive(true);
        }

        public void UnHighlightedButton()
        {
            _highlightButtonImg.gameObject.SetActive(false);
            //_actionButton.GetComponent<Image>().sprite = _defaultButtonImg;

        }
    }
}
