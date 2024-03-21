using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class PauseMenuWindowManager : MonoBehaviour, IInputReceiver
    {
        [SerializeField] private PauseMenuCharacterDetail _characterDetail;

        private WindowGUI _lastWindowOpen;

        private void Start()
        {
            //CombatReferee.Instance.SubOnCombatPreparation(LoadNewEntityIntoHUD);

            //TargetSelectionFlowControl.Instance.SubOnTargetSelectionEnter(OnTargetSelectionEnter);
            //TargetSelectionFlowControl.Instance.SubOnTargetSelectionEnd(OnTargetSelectionEnd);//.SubEvent<bool>(OnTargetSelectionEnd, "OnTargetSelectionEnd");

            //foreach (var window in GetAllValidWindows())
            //    window.Init(this);

        }

        private void OnDisable()
        {
            //CombatReferee.Instance.UnSubOnCombatPreparation(LoadNewEntityIntoHUD);
        }

        private List<WindowGUI> GetAllValidWindows()
        {
            List<WindowGUI> allWindows = new List<WindowGUI>();
            allWindows.Add(_characterDetail);
            //allWindows.Add(_itemWindow);
            //allWindows.Add(_weaponWindow);
            //allWindows.Add(_mainWindow);

            return allWindows;
        }

        //private void LoadNewEntityIntoHUD(Null n)
        //{
        //    foreach (var entity in CombatReferee.Instance.GetCompetatorsBySide(ECompetatorSide.Ally))
        //    {
        //        entity.SubOnTakeControlEvent(SetUpWindows);
        //        entity.SubOnTakeControlLeaveEvent(CloseWindow);
        //    }
        //}

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
        //public void OpenWindow(EWindowGUI newWindowType)
        //{
        //    WindowGUI windowToOpen = _mainWindow;

        //    if (newWindowType == EWindowGUI.Main)
        //    {
        //        windowToOpen = _mainWindow;
        //    }
        //    else if (newWindowType == EWindowGUI.Spell)
        //    {
        //        windowToOpen = _spellWindow;

        //    }
        //    else if (newWindowType == EWindowGUI.Item)
        //    {
        //        windowToOpen = _itemWindow;
        //    }
        //    else if (newWindowType == EWindowGUI.Weapon)
        //    {
        //        windowToOpen = _weaponWindow;
        //    }

        //    TryOpenWindow(windowToOpen);

        //}

        //public void SetUpWindows(CombatEntity entity)
        //{
        //    foreach (var window in GetAllValidWindows())
        //        window.ClearData();

        //    foreach (var window in GetAllValidWindows())
        //        window.LoadWindowData(entity);

        //    OpenWindow(EWindowGUI.Main);

        //    CentralInputReceiver.Instance().AddInputReceiverIntoStack(this);

        //}

        //public void CloseWindow(CombatEntity combatEntity = null)
        //{
        //    CentralInputReceiver.Instance().RemoveInputReceiverIntoStack(this);

        //    if (_lastWindowOpen != null)
        //        return;


        //    _lastWindowOpen.OnWindowDeActive();
        //    HideCurrentWindow();
        //    _lastWindowOpen = null;

        //}

        public void DisplayCurrentWindow()
        {
            if (!_lastWindowOpen.gameObject.activeSelf)
                _lastWindowOpen.gameObject.SetActive(true);
        }

        public void HideCurrentWindow()
        {
            if (_lastWindowOpen.gameObject.activeSelf)
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

        private void OnTargetSelectionEnd(bool sucessfullySelect)
        {
            //if (sucessfullySelect)
            //{
            //    CloseWindow();
            //}
            //else
            //{
            //    DisplayCurrentWindow();
            //}
        }

        #endregion
    }
}
