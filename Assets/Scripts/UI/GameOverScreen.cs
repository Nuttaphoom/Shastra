using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Vanaring 
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        bool flipflop = true;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                ActiveGameOverScreen(!flipflop);
                flipflop = !flipflop;
            }
        }

        public void ActiveGameOverScreen(bool win)
        {
            if (win)
            {
                _text.text = "You Win";
            }
            else
            {
                _text.text = "Game Over";
            }
            gameObject.SetActive(true);
        }

        public void RestartButton()
        {
            Debug.Log("Restart");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void MainMenuButton()
        {
            Debug.Log("MainMenu");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
