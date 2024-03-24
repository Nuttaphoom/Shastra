using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class PauseMenuWindowManager : MonoBehaviour, IInputReceiver
    {
        [SerializeField] private PauseMenuCharacterDetail _characterDetail;
        [SerializeField] private PauseMenuMainPanel _mainPause;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button characterDetailButton;

        private PauseMenuWindowGUI _lastWindowOpen;

        private void Start()
        {
            characterDetailButton.onClick.AddListener(()=>OpenWindow(EPauseWindowGUI.Party));

            foreach (var window in GetAllValidWindows())
                window.Init(this);

        }

        private List<PauseMenuWindowGUI> GetAllValidWindows()
        {
            List<PauseMenuWindowGUI> allWindows = new List<PauseMenuWindowGUI>();
            allWindows.Add(_characterDetail);

            return allWindows;
        }

        #region PrivateMethod
        private void TryOpenWindow(PauseMenuWindowGUI newWindow)
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
        public void OpenWindow(EPauseWindowGUI newWindowType)
        {
            PauseMenuWindowGUI windowToOpen = _mainPause;

            if(newWindowType == EPauseWindowGUI.Party)
            {
                windowToOpen = _characterDetail;
            }
            else if (newWindowType == EPauseWindowGUI.Main)
            {
                windowToOpen = _mainPause;
            }

            TryOpenWindow(windowToOpen);

        }



        public void SetUpWindows()
        {
            foreach (var window in GetAllValidWindows())
                window.ClearData();

            //foreach (var window in GetAllValidWindows())
            //    window.LoadWindowData(entity);

            //OpenWindow(EWindowGUI.Main);

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
            if (sucessfullySelect)
            {
                CloseWindow();
            }
            else
            {
                DisplayCurrentWindow();
            }
        }

        #endregion
    }
}
