using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace Vanaring
{
    public class TransitionObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject transitionCanvas;
        [SerializeField]
        private TransitionSceneSO _transitionSO;
        [SerializeField]
        private TextMeshProUGUI _tiptext;
        [SerializeField]
        private Image loadingScreenImage;
        public Image loadingBarFill;
        private TransitionSceneManager _tsm;
        private EventBroadcaster _eventBroadCaster;
        public void Init(TransitionSceneManager tsm)
        {
            _tsm = tsm;
            _tsm.SubOnSceneLoaderOperation(OnSceneProgressBarLoading);
            _tsm.SubOnSceneLoaderComplete(Startcou);
            transitionCanvas.SetActive(true);
            StartCoroutine(FadeInBackground());
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

        private IEnumerator FadeInBackground()
        {
            loadingScreenImage.fillAmount = 0;
            while (loadingScreenImage.fillAmount < 1)
            {
                loadingScreenImage.fillAmount += Time.deltaTime * 5;
                yield return new WaitForSeconds(0.007f);
            }
            //Debug.Log("Fade-in obj complete");
            GetEventBroadcaster().InvokeEvent<Null>(null, "OnSceneLoaderBegin");
        }

        private IEnumerator DestroyAfterTransition()
        {
            _tsm.UnSubOnSceneLoaderOperation(OnSceneProgressBarLoading);
            yield return new WaitForSeconds(1.0f);

            while (loadingScreenImage.fillAmount > 0)
            {
                loadingScreenImage.fillAmount -= Time.deltaTime * 5;
                yield return new WaitForSeconds(0.007f);
            }
            transitionCanvas.SetActive(false);
            Destroy(gameObject);
            _tsm.UnSubOnSceneLoaderComplete(Startcou);
            //Debug.Log("Destroy transition");
        }

        private void Startcou(Null n)
        {
            StartCoroutine(DestroyAfterTransition());
        }

        public void SubOnSceneLoaderBegin(UnityAction<Null> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnSceneLoaderBegin");
        }
        public void UnSubOnSceneLoaderBegin(UnityAction<Null> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnSceneLoaderBegin");
        }

        //private IEnumerator RandomTip()
        //{
        //    while (true)
        //    {
        //        int ranText = Random.Range(0, _transitionSO.TipText.Count - 1);
        //        _tiptext.text = _transitionSO.TipText[ranText];
        //        yield return new WaitForSeconds(3.0f);
        //    }
        //}

        //private IEnumerator RandomBG()
        //{
        //    while (true)
        //    {
        //        int ranText = Random.Range(0, _transitionSO.ScreenImage.Count - 1);
        //        loadingScreenImage.sprite = _transitionSO.ScreenImage[ranText];
        //        yield return new WaitForSeconds(10.0f);
        //    }
        //}
    }
}
