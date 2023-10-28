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
        [SerializeField]    private SpellWindowGUI _spellWindow;
        [SerializeField]    private ItemWindowGUI _itemWindow;
        [SerializeField]    private WeaponWindowGUI _weaponWindow;
        [SerializeField]    private MainWindowGUI _mainWindow; 

        private WindowGUI _lastWindowOpen;

    
        private void Start()
        {

            foreach (var entity in CombatReferee.instance.GetCompetatorsBySide(ECompetatorSide.Ally))
            {
                entity.EventBroadcaster.SubEvent<CombatEntity>(SetUpWindows, "OnTakeControl");
                entity.EventBroadcaster.SubEvent<CombatEntity>(CloseWindow, "OnTakeControlLeave");
            }

            TargetSelectionFlowControl.Instance.GetEventBroadcaster().SubEvent<bool>(OnTargetSelectionEnd, "OnTargetSelectionEnd");
            TargetSelectionFlowControl.Instance.GetEventBroadcaster().SubEvent<CombatEntity>(OnTargetSelectionEnter, "OnTargetSelectionEnter");

            _spellWindow.Init(this);
            _itemWindow.Init(this);
            _weaponWindow.Init(this);
            _mainWindow.Init(this);

            //_spellWindow.gameObject.SetActive(false);
            //_itemWindow.gameObject.SetActive(false);
            //_weaponWindow.gameObject.SetActive(false);
            //_mainWindow.gameObject.SetActive(false);

        }

        #region PrivateMethod
        private void TryOpenWindow(WindowGUI newWindow)
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
            WindowGUI windowToOpen = _mainWindow;

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
            _spellWindow.LoadWindowData(entity); 
            _itemWindow.LoadWindowData(entity);
            _weaponWindow.LoadWindowData(entity);
            _mainWindow.LoadWindowData(entity);

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
