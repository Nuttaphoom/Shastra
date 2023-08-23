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
        [Header("Button")]
        [SerializeField]
        private Button _actionButton ;
        [SerializeField]
        private TextMeshProUGUI _textMeshProUGUI ;
        [SerializeField]
        private TextMeshProUGUI _itemDescription;
        [SerializeField]
        private Image _itemIcon;
        [SerializeField]
        private TextMeshProUGUI _textMeshProNum ;

        [Header("Button")]
        [SerializeField]
        private Sprite _highlightButtonImg;
        [SerializeField]
        private Sprite _defaultButtonImg;

        private string _itemName;


        [SerializeField]
        private Image _skillImage;  

        private ItemAbilityRuntime _item ;

        private CombatEntity _caster;

        private int _itemAmount;

        private Color _hightlightedColor = Color.yellow;
        private Color _defaultColor;

        private bool _init = false; 
        public void Init(ItemAbilityRuntime item, CombatEntity combatEntity)
        {
            _init  = true; 
            _item = item;
            this._caster = combatEntity;
            _actionButton.onClick.AddListener(ChooseItem) ;

            _textMeshProUGUI.text = item.ItemName.ToString() ;
            _itemDescription.text = item.ItemDescrption.ToString();
            _itemIcon.sprite = item.ItemSprite;
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
            if (_init)
            {
                Debug.Log("is  init");
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
            _actionButton.GetComponent<Image>().sprite = _highlightButtonImg;
        }

        public void UnHighlightedButton()
        {
            _actionButton.GetComponent<Image>().sprite = _defaultButtonImg; 
        }
    }
}
