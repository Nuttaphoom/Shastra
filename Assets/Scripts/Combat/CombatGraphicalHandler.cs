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
    public class CombatGraphicalHandler : MonoBehaviour
    {
        [Header("Listen to ")]
        [SerializeField]
        private CombatEntityEventChannel OnTargetSelectionSchemeStart;
        [SerializeField] 
        private CombatEntityEventChannel OnTargetSelectionSchemeEnd;


 

        [Header("Panel and Canvas object (menu)")]
        [SerializeField]
        private Transform _mainPanel;
        [SerializeField]
        private Transform _spellPanel;
        [SerializeField]
        private Transform _itemPanel;

        [SerializeField]
        private GameObject _mainCanvas;

        [SerializeField]
        private CombatEntity _combatEntity;

        [SerializeField]
        private MainWindowManager _mainWindowManager; 

        private void Awake()
        {
  

            if (_mainCanvas == null)
                throw new Exception("Main Canvas hasn't been assigned");

            if (_combatEntity.GetComponent<CombatEntity>() == null)
                throw new Exception("Combat Entiy hasn't been assigned");
              
 
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

        public void DisplayItemPanel()
        {
            _mainPanel.gameObject.SetActive(false);
            _itemPanel.gameObject.SetActive(true);
        }

        public void DisplaySpellPanel()
        {
            _mainPanel.gameObject.SetActive(false);
            _spellPanel.gameObject.SetActive(true) ;
            _itemPanel.gameObject.SetActive(false);


        }

        public void DisplayWeaponPanel()
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

        public void TakeControl()
        {
            EnableGraphicalElements();
            _mainWindowManager.TakeInputControl(); 
        }

        public void TakeControlLeave()
        {
            DisableGraphicalElements() ;
            _mainWindowManager.ReleaseInputControl();
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

        private void OnEnergyUpdate(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int amount)
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
 

