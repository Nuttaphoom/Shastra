using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Vanaring
{
    public class TransitionManager : MonoBehaviour
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

        public void LoadScene(Scene scene)
        {
            StartCoroutine(LoadSceneAsync(scene.buildIndex));
            StartCoroutine(RandomTip());
            StartCoroutine(RandomBG());
        }
        
        private IEnumerator LoadSceneAsync(int sceneId)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

            transitionCanvas.SetActive(true);

            while (!operation.isDone)
            {
                float progressVal = Mathf.Clamp01(operation.progress / 0.9f);

                loadingBarFill.fillAmount = progressVal;

                yield return null;
            }
        }

        private IEnumerator RandomTip()
        {
            while (true)
            {
                int ranText = Random.Range(0, _transitionSO.TipText.Count-1);
                _tiptext.text = _transitionSO.TipText[ranText];
                yield return new WaitForSeconds(3.0f);
            }
        }

        private IEnumerator RandomBG()
        {
            while (true)
            {
                int ranText = Random.Range(0, _transitionSO.ScreenImage.Count - 1);
                loadingScreenImage.sprite = _transitionSO.ScreenImage[ranText];
                yield return new WaitForSeconds(10.0f);
            }
        }
    }
}
