using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Vanaring 
{
    
    public class PlayerWindowManager : MonoBehaviour, IInputReceiver
    {
        [SerializeField] private SpellWindowGUI _spellWindow;
        [SerializeField] private ItemWindowGUI _itemWindow;
        [SerializeField] private WeaponWindowGUI _weaponWindow;
        [SerializeField] private MainWindowGUI _mainWindow;

        private CombatWindowGUI _lastWindowOpen;

        private void Start()
        {
            CombatReferee.Instance.SubOnCombatPreparation(LoadNewEntityIntoHUD);

            TargetSelectionFlowControl.Instance.SubOnTargetSelectionEnter(OnTargetSelectionEnter);
            TargetSelectionFlowControl.Instance.SubOnTargetSelectionEnd(OnTargetSelectionEnd);//.SubEvent<bool>(OnTargetSelectionEnd, "OnTargetSelectionEnd");

            foreach (var window in GetAllValidWindows())
                window.Init(this);

        }

        private void OnDisable()
        {
            CombatReferee.Instance.UnSubOnCombatPreparation(LoadNewEntityIntoHUD);
        }

        private List<CombatWindowGUI> GetAllValidWindows()
        {
            List<CombatWindowGUI> allWindows = new List<CombatWindowGUI>() ;
            allWindows.Add(_spellWindow);
            allWindows.Add(_itemWindow);
            allWindows.Add(_weaponWindow);
            allWindows.Add(_mainWindow);

            return allWindows; 
        }

        private void LoadNewEntityIntoHUD(Null n)
        {
            foreach (var entity in CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Ally))
            {
                entity.SubOnTakeControlEvent(SetUpWindows);
                entity.SubOnTakeControlLeaveEvent(CloseWindow);
            }
        }

        #region PrivateMethod
        private void TryOpenWindow(CombatWindowGUI newWindow)
        {
            if (newWindow.gameObject.activeSelf)
                return;

            if (_lastWindowOpen != null)
            {
                _lastWindowOpen.OnWindowDeActive();
                HideCurrentWindow(); 
            }

            _lastWindowOpen = newWindow;

            DisplayCurrentWindow(); 
            _lastWindowOpen.OnWindowActive();
            
        }
        #endregion

        #region PublicMethod
        public void OpenWindow(EWindowGUI newWindowType)
        {
            CombatWindowGUI windowToOpen = _mainWindow;

            if (newWindowType == EWindowGUI.Main)
            {
                windowToOpen = _mainWindow;
            }
            else if (newWindowType == EWindowGUI.Spell)
            {
                windowToOpen = _spellWindow;

            }
            else if (newWindowType == EWindowGUI.Item)
            {
                windowToOpen = _itemWindow;
            }
            else if (newWindowType == EWindowGUI.Weapon)
            {
                windowToOpen = _weaponWindow;
            }

            TryOpenWindow(windowToOpen);

        }

        public void SetUpWindows(CombatEntity entity)
        {
            foreach (var window in GetAllValidWindows())
                window.ClearData( );

            foreach (var window in GetAllValidWindows())
                window.LoadWindowData(entity);

            OpenWindow(EWindowGUI.Main);

            CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);

        }

        public void CloseWindow(CombatEntity combatEntity = null)
        {
            CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);

            if (_lastWindowOpen != null)
                return;
                
            
            _lastWindowOpen.OnWindowDeActive();
            HideCurrentWindow();
            _lastWindowOpen = null;

        }

        public void DisplayCurrentWindow()
        {
            if (! _lastWindowOpen.gameObject.activeSelf)
                _lastWindowOpen.gameObject.SetActive(true); 
        }

        public void HideCurrentWindow()
        {
            if (_lastWindowOpen.gameObject.activeSelf )
                _lastWindowOpen.gameObject.SetActive(false);
        }

        public void ReceiveKeys(KeyCode key)
        {
            if (_lastWindowOpen != null) 
                _lastWindowOpen.ReceiveKeysFromWindowManager(key);
        }

        #endregion

        #region EventListener 
        private void OnTargetSelectionEnter(CombatEntity actor)
        {
            HideCurrentWindow(); 
        }

        private void OnTargetSelectionEnd(bool sucessfullySelect )
        {
            if ( sucessfullySelect)
            {
                CloseWindow(); 
            }else
            {
                DisplayCurrentWindow(); 
            }
        }

        #endregion
    }
}
