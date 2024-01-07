using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

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

        private void Awake()
        {
        }
        private void OnDisable()
        {
            
        }

        public void Init(TransitionSceneManager tsm)
        {
            _tsm = tsm;
            _tsm.SubOnSceneLoaderOperation(OnSceneProgressBarLoading);
            _tsm.SubOnSceneLoaderComplete(Startcou);
            transitionCanvas.SetActive(true);
        }

        private void OnSceneProgressBarLoading(float val)
        {
            if (!transitionCanvas.activeSelf)
            {
                transitionCanvas.SetActive(true);
            }
            loadingBarFill.fillAmount = val;
        }

        private IEnumerator DestroyAfterTransition()
        {
            transitionCanvas.SetActive(false);
            _tsm.UnSubOnSceneLoaderOperation(OnSceneProgressBarLoading);
            yield return new WaitForSeconds(2.0f);
            _tsm.UnSubOnSceneLoaderComplete(Startcou);
            Debug.Log("Destroy transition");
        }

        private void Startcou(Null n)
        {
            StartCoroutine(DestroyAfterTransition());
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
