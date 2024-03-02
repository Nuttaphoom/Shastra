using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Vanaring
{
    public class TransitionObject : MonoBehaviour
    {
        [Header("Scriptable Object")]
        [SerializeField]
        private TransitionSceneSO _transitionSO;

        [Header("Canvas Component")]
        [SerializeField]
        private GameObject transitionCanvas;
        [SerializeField]
        private TextMeshProUGUI _tiptext;
        [SerializeField]
        private Image gfxScreen;

        [Header("Director")]
        [SerializeField]
        private PlayableDirector fadeInDirector;
        [SerializeField]
        private PlayableDirector fadeOutDirector;

        [Header("Customize")]
        private float delayLoadingTime;

        public Image loadingBarFill;
        private TransitionSceneManager _tsm;
        private EventBroadcaster _eventBroadCaster;


        public void Init(TransitionSceneManager tsm)
        {
            _tsm = tsm;
            delayLoadingTime = _transitionSO.GetDelayLoadingTime;
            _tsm.SubOnSceneLoaderOperation(OnSceneProgressBarLoading);
            _tsm.SubOnSceneLoaderComplete(Startcou);
            transitionCanvas.SetActive(true);
            StartCoroutine(FadeInTransition());
            fadeInDirector.Play();
            //System should tell the timeline to play
        }

        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadCaster == null)
            {
                _eventBroadCaster = new EventBroadcaster();
                _eventBroadCaster.OpenChannel<Null>("OnSceneLoaderBegin");
            }
            return _eventBroadCaster;
        }

        private void OnSceneProgressBarLoading(float val)
        {
            if (!transitionCanvas.activeSelf)
            {
                transitionCanvas.SetActive(true);
            }
            loadingBarFill.fillAmount = val;
        }

        private IEnumerator FadeInTransition()
        {
            gfxScreen.fillAmount = 0;
            while (fadeInDirector.state == PlayState.Playing)
                yield return new WaitForEndOfFrame();
            //Debug.Log("finish fade-in");
            fadeInDirector.Stop();
            GetEventBroadcaster().InvokeEvent<Null>(null, "OnSceneLoaderBegin");
        }

        private IEnumerator FadeOutTransition()
        {
            Debug.Log("fade out transitio nstart");
            _tsm.UnSubOnSceneLoaderOperation(OnSceneProgressBarLoading);
            //yield return new WaitForSeconds(delayLoadingTime);
            //Debug.Log("Load scene finish");
            fadeOutDirector.Play();
            while (fadeOutDirector.state == PlayState.Playing)
                yield return new WaitForEndOfFrame();
            //Debug.Log("finish fade-out");
            fadeOutDirector.Stop();
            GetEventBroadcaster().InvokeEvent<Null>(null, "OnSceneLoaderBegin");
            transitionCanvas.SetActive(false);
            Destroy(gameObject);
            _tsm.UnSubOnSceneLoaderComplete(Startcou);
            yield return null;
        }

        private void Startcou(Null n)
        {
            StartCoroutine(FadeOutTransition());
        }

        public void SubOnSceneLoaderBegin(UnityAction<Null> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnSceneLoaderBegin");
        }
        public void UnSubOnSceneLoaderBegin(UnityAction<Null> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnSceneLoaderBegin");
        }
    }
}
