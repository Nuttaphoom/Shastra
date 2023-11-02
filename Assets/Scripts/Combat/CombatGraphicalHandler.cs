using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
 

namespace Vanaring 
{
    public class CombatGraphicalHandler : MonoBehaviour
    {
        [Header("Listen to ")]
        //[SerializeField]
        //private CombatEntityEventChannel OnTargetSelectionSchemeStart;
        //[SerializeField] 
        //private CombatEntityEventChannel OnTargetSelectionSchemeEnd;



        [Header("Panel and Canvas object (menu)")]
        [SerializeField]
        private SpellWindowManager _spellWindowManager;
        [SerializeField]
        private ItemWindowManager _itemWindowManager;
        [SerializeField]
        private MainWindowManager _mainWindowManager;

        private EntityWindowManager _entityWindowManager;

        [SerializeField]
        private Transform _quickHPBar; 

        [SerializeField]
        private CombatEntity _combatEntity;
 

        private void Awake()
        {
            if (_quickHPBar == null)
                throw new Exception("Quick HP bar Canvas hasn't been assigned");

            if (_combatEntity.GetComponent<CombatEntity>() == null)
                throw new Exception("Combat Entiy hasn't been assigned");

            _entityWindowManager = new EntityWindowManager(); 
        }

        private void OnEnable()
        {
            _combatEntity.SubOnDamageVisualEvent(OnUpdateEntityStats);
            _combatEntity.SubOnDamageVisualEventEnd(OnVisualHurtUpdateEnd); 
            //OnTargetSelectionSchemeStart.SubEvent(OnTargetSelectionStart_DisableUI) ;
            //OnTargetSelectionSchemeEnd.SubEvent(OnTargetSelectionEnd_EnableUI);
            _combatEntity.SpellCaster.SubOnModifyEnergy(OnEnergyUpdate);
        }

        private void OnDisable()
        {
            //OnTargetSelectionSchemeStart.UnSubEvent(OnTargetSelectionStart_DisableUI);
            //OnTargetSelectionSchemeEnd.UnSubEvent(OnTargetSelectionEnd_EnableUI); 
            _combatEntity.UnSubOnDamageVisualEvent(OnUpdateEntityStats);
            _combatEntity.UnSubOnDamageVisualEventEnd(OnVisualHurtUpdateEnd);
        }

        public void DisplayItemPanel()
        {
            _entityWindowManager.PushInNewWindow(_itemWindowManager);
        }

        public void DisplaySpellPanel()
        {
            _entityWindowManager.PushInNewWindow(_spellWindowManager);
        }

        public void DisplayWeaponPanel()
        {

        }

        public void DisableMenuElements()
        {
            _entityWindowManager.ClearStack();
            
            //_spellPanel.gameObject.SetActive(false);
            //_itemPanel.gameObject.SetActive(false);
            //_mainPanel.gameObject.SetActive(false);
        }

        public void DisplayMainMenu()
        {
            _entityWindowManager.ClearStack();
            _entityWindowManager.PushInNewWindow(_mainWindowManager);
        }

        public IEnumerator TakeControl()
        {
            EnableQuickMenuBar(true); 
            _entityWindowManager.SetNewEntity(this);
            DisplayMainMenu(); 

             _mainWindowManager.TakeInputControl();

            yield return null; 
        }

        public void TakeControlLeave()
        {
            EnableQuickMenuBar(false); 
            DisableMenuElements() ;
           
            _mainWindowManager.ReleaseInputControl();
        }

        #region EventListener
        private void OnTargetSelectionStart_DisableUI(CombatEntity _combatEntity)
        {
            DisableMenuElements();
            EnableQuickMenuBar(false);
            //_mainPanel.gameObject.SetActive(false);
            //_spellPanel.gameObject.SetActive(false);
            //_itemPanel.gameObject.SetActive(false);
        }

        private void OnTargetSelectionEnd_EnableUI(CombatEntity combatEntity)
        {
            if (combatEntity != _combatEntity)
                return;

            //if (!TargetSelectionFlowControl.Instance.PrepareAction())
            //{
            //    EnableQuickMenuBar(true);

            //    _entityWindowManager.PushInNewWindow(_mainWindowManager);
            //}

            //if (_combatEntity == combatEntity)
            //{
            //    _mainPanel.gameObject.SetActive(true);
            //} 
        }

        private void OnVisualHurtUpdateEnd(int i)
        {
            EnableQuickMenuBar(false); 
        }

        private void OnEnergyUpdate(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int amount)
        {
            OnUpdateEntityStats(0);
         }

        #endregion

        private void OnUpdateEntityStats(int i)
        {
            if (!_quickHPBar.gameObject.activeSelf)
                EnableQuickMenuBar(true); 
        }

        public void EnableQuickMenuBar(bool b)
        {
            
            _quickHPBar.gameObject.SetActive(b); 
        }



    }
}
 

