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
        [Header("Listen to ")]
        [SerializeField]
        private CombatEntityEventChannel OnTargetSelectionSchemeStart;
        [SerializeField] 
        private CombatEntityEventChannel OnTargetSelectionSchemeEnd; 

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
        [SerializeField]
        private Transform _itemPanel;

        private CombatEntity _combatEntity; 

        private void Awake()
        {
            if (_itemButton == null || _spellButton == null || _weaponButton == null)
                throw new Exception("Buttons hasn't been correctly assigned");

            _combatEntity = GetComponent<CombatEntity>(); 

            _itemButton.onClick.AddListener(() => { DisplayItemPanel(); });
            _spellButton.onClick.AddListener(() => { DisplaySpellPanel(); });
            _weaponButton.onClick.AddListener(() => { DisplayWeaponPanel(); });
        }

        private void OnEnable()
        {
            OnTargetSelectionSchemeStart.SubEvent(OnTargetSelectionStart_DisableUI) ;
            OnTargetSelectionSchemeEnd.SubEvent(OnTargetSelectionEnd_EnableUI);
        }

        private void OnDisable()
        {
            OnTargetSelectionSchemeStart.UnSubEvent(OnTargetSelectionStart_DisableUI);
            OnTargetSelectionSchemeEnd.UnSubEvent(OnTargetSelectionEnd_EnableUI);
        }

        private void DisplayItemPanel()
        {
            _mainPanel.gameObject.SetActive(false);
            _itemPanel.gameObject.SetActive(true);
        }

        private void DisplaySpellPanel()
        {
            _mainPanel.gameObject.SetActive(false);
            _spellPanel.gameObject.SetActive(true) ;
            _itemPanel.gameObject.SetActive(false);

        }

        private void DisplayWeaponPanel()
        {

        }

        public void DisableGraphicalElements()
        {
            _mainPanel.gameObject.SetActive(false);
            _spellPanel.gameObject.SetActive(false);
            _itemPanel.gameObject.SetActive(false);
        }

        public void EnableGraphicalElements()
        {
            _mainPanel.gameObject.SetActive(true) ;
            _spellPanel.gameObject.SetActive(false);
            _itemPanel.gameObject.SetActive(false);
        }

        #region EventListener
        private void OnTargetSelectionStart_DisableUI(CombatEntity _combatEntity)
        {
            _mainPanel.gameObject.SetActive(false);
            _spellPanel.gameObject.SetActive(false);
            _itemPanel.gameObject.SetActive(false);
        }

        private void OnTargetSelectionEnd_EnableUI(CombatEntity combatEntity)
        {
            if (_combatEntity == combatEntity)
            {
                Debug.Log("yes enable this one");
                _mainPanel.gameObject.SetActive(true);
            }else
            {
                Debug.Log("no dont enable thsi one");
            }
        }

        #endregion


    }
}
 

