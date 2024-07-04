using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vanaring
{
    public class MainmenuManager : MonoBehaviour, ISceneLoaderWaitForSignal
    {

        [SerializeField]
        private Button _initialButton; 

        [SerializeField]
        private SceneDataSO _mapScene ;
        public void StartGame()
        {
            PersistentSceneLoader.Instance.LoadLocation<int>(_mapScene, 0);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return null; 
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            _initialButton.Select(); 
            yield return null; 
        }
    }
}
