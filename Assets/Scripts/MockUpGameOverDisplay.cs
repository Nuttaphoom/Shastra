using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vanaring
{
    public class MockUpGameOverDisplay : MonoBehaviour
    {
        [SerializeField]
        private GameObject _gameWinScene;

        [SerializeField]
        private GameObject _gameOverScene;

        public static MockUpGameOverDisplay Instance;

        private void Awake()
        {
            Instance = this; 
        }

        public void GameWinDisplay()
        {
            _gameWinScene.SetActive(true);

            StartCoroutine(ReloadSceneAgain());
        }

        public void GameOverDisplay()
        {
            _gameOverScene.SetActive(true);

            StartCoroutine(ReloadSceneAgain()); 
        }

        public IEnumerator ReloadSceneAgain()
        {
            yield return new WaitForSeconds(3) ;

            SceneManager.LoadScene(0);
        }

    }
}
