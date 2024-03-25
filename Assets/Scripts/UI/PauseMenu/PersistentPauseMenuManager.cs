using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class PersistentPauseMenuManager : MonoBehaviour
    {
        [SerializeField] private PauseMenuWindowManager _pauseMenuWindowManager;
        [SerializeField] private GameObject pauseMenuGFX;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pauseMenuGFX.activeSelf)
                {
                    Debug.Log("Close Pause");
                    //_pauseMenuWindowManager.HideCurrentWindow();
                }
                else
                {
                    Debug.Log("Open Pause");
                    _pauseMenuWindowManager.OpenWindow(EPauseWindowGUI.Main);
                    _pauseMenuWindowManager.SetUpWindows();
                }
            }
        }
    }
}
