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


        [Header("Button to bind call back") ] 
        [SerializeField]
        private Button _itemButton;
        [SerializeField]
        private Button _spellButton;
        [SerializeField]
        private Button _weaponButton;

        [Header("Panel and Canvas object (menu)")]
        [SerializeField]
        private Transform _mainPanel;
        [SerializeField]
        private Transform _spellPanel;
        [SerializeField]
        private Transform _itemPanel;

        [SerializeField]
        private GameObject _mainCanvas; 

        private CombatEntity _combatEntity; 

        private void Awake()
        {
            if (_itemButton == null || _spellButton == null || _weaponButton == null)
                throw new Exception("Buttons hasn't been correctly assigned");

            if (_mainCanvas == null)
                throw new Exception("Main Canvas hasn't been assigned"); 

            _combatEntity = GetComponent<CombatEntity>();


            _itemButton.onClick.AddListener(() => { DisplayItemPanel(); });
            _spellButton.onClick.AddListener(() => { DisplaySpellPanel(); });
            _weaponButton.onClick.AddListener(() => { DisplayWeaponPanel(); });
        }

        private void OnEnable()
        {
            _combatEntity.SubOnDamageVisualEvent(OnVisualHurtUpdate);
            _combatEntity.SubOnDamageVisualEventEnd(OnVisualHurtUpdateEnd); 
            OnTargetSelectionSchemeStart.SubEvent(OnTargetSelectionStart_DisableUI) ;
            OnTargetSelectionSchemeEnd.SubEvent(OnTargetSelectionEnd_EnableUI);
            _combatEntity.SpellCaster.SubOnModifyEnergy(OnEnergyUpdate);
        }

        private void OnDisable()
        {
            OnTargetSelectionSchemeStart.UnSubEvent(OnTargetSelectionStart_DisableUI);
            OnTargetSelectionSchemeEnd.UnSubEvent(OnTargetSelectionEnd_EnableUI); 
            _combatEntity.UnSubOnDamageVisualEvent(OnVisualHurtUpdate);
            _combatEntity.UnSubOnDamageVisualEventEnd(OnVisualHurtUpdateEnd);

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

        public void DisableMenuElements()
        {
            _spellPanel.gameObject.SetActive(false);
            _itemPanel.gameObject.SetActive(false);
            _mainPanel.gameObject.SetActive(false);


        }

        public void DisableGraphicalElements()
        {
            _mainPanel.gameObject.SetActive(false);
            _spellPanel.gameObject.SetActive(false);
            _itemPanel.gameObject.SetActive(false);
            _mainCanvas.gameObject.SetActive(false);
        }

        public void EnableGraphicalElements()
        {
            _mainCanvas.gameObject.SetActive(true); 
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
                _mainPanel.gameObject.SetActive(true);
            } 
        }

        private void OnVisualHurtUpdate(int i )
        {
            OnUpdateEntityStats(); 
        }

        private void OnVisualHurtUpdateEnd(int i)
        {
            if (_mainCanvas.activeSelf)
                _mainCanvas.gameObject.SetActive(false);
        }

        private void OnEnergyUpdate(RuntimeMangicalEnergy.EnergySide side, int amount)
        {
            OnUpdateEntityStats();
        }

        #endregion

        private void OnUpdateEntityStats()
        {
            if(!_mainCanvas.activeSelf)
                _mainCanvas.gameObject.SetActive(true);
        }



    }
}
 

