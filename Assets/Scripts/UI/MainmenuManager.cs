using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanaring
{
    public class MainmenuManager : MonoBehaviour
    {
        [SerializeField]
        private SceneDataSO _gameScene;
        public void StartGame()
        {
            PersistentSceneLoader.Instance.LoadLocation<int>(_gameScene, 0);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
