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

        private void OnEnable()
        {
            _tsm.SubOnSceneLoaderOperation(OnSceneProgressBarLoading);
            _tsm.SubOnSceneLoaderComplete((Null n) => { });
        }
        private void OnDisable()
        {
            _tsm.UnSubOnSceneLoaderOperation(OnSceneProgressBarLoading);
        }

        public void Init()
        {
            _tsm.SubOnSceneLoaderOperation(OnSceneProgressBarLoading);
        }

        private void OnSceneProgressBarLoading(float val)
        {
            if (!transitionCanvas.activeSelf)
            {
                transitionCanvas.SetActive(true);
            }
            loadingBarFill.fillAmount = val;
        }

        private void DestoyOnFinish(Null n)
        {
            Destroy(this);
        }

        //private IEnumerator RandomTip()
        //{
        //    while (true)
        //    {
        //        int ranText = Random.Range(0, _transitionSO.TipText.Count-1);
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
