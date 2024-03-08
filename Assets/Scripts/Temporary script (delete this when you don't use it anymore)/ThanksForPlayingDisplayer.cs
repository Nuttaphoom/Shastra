using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Vanaring
{
    public class ThanksForPlayingDisplayer : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector _thankforPlayingTimeline;

        private void Awake()
        {
            if (_thankforPlayingTimeline == null)
                throw new System.Exception("_thankforPlayingTimeline is null");

            _thankforPlayingTimeline.gameObject.SetActive(false);
        }
        public void RestartCombatButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex) ;
        }

        public void ExitApplicationButton()
        {
            Application.Quit(); 
        }

        public void ShowThankForPlayingMenu()
        {
            if (_thankforPlayingTimeline == null)
                throw new System.Exception("_thankforPlayingTimeline is null") ;

            _thankforPlayingTimeline.gameObject.SetActive(true);

        }
    }
}
