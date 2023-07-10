using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Vanaring_DepaDemo;

namespace Vanaring_DepaDemo
{
    [RequireComponent(typeof(CombatEntity))]
    public class CombatGraphicalHandler : MonoBehaviour
    {
        [SerializeField]
        private Button _itemButton;
        [SerializeField]
        private Button _spellButton;
        [SerializeField]
        private Button _weaponButton;

        
        [SerializeField]
        private Transform _mainPanel;

        [SerializeField]
        private Transform _spellPanel;

        private void Awake()
        {
            if (_itemButton == null || _spellButton == null || _weaponButton == null)
                throw new Exception("Buttons hasn't been correctly assigned");

            _itemButton.onClick.AddListener(() => { DisplayItemPanel(); });
            _spellButton.onClick.AddListener(() => { DisplaySpellPanel(); });
            _weaponButton.onClick.AddListener(() => { DisplayWeaponPanel(); });

        }

        private void DisplayItemPanel()
        {

        }

        private void DisplaySpellPanel()
        {
            _mainPanel.gameObject.SetActive(false);
            _spellPanel.gameObject.SetActive(true) ; 
        }

        private void DisplayWeaponPanel()
        {

        }

        public void DisableGraphicalElements()
        {
            _mainPanel.gameObject.SetActive(false);
            _spellPanel.gameObject.SetActive(false);
        }

        public void EnableGraphicalElements()
        {
            _mainPanel.gameObject.SetActive(true) ;
            _spellPanel.gameObject.SetActive(false);
        } 


    }
}
 

