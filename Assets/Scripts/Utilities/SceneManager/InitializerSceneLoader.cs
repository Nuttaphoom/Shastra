using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vanaring
{
    

    public class InitializerSceneLoader : MonoBehaviour
    {
        [SerializeField]
        private SceneDataSO _firstSceneToLoad;

        [SerializeField]
        private SceneDataSO _persistantScene;

        private void Start()
        {
            Debug.LogAssertion("Start scene loader") ;
            SceneManager.LoadSceneAsync(_persistantScene.GetSceneName(), LoadSceneMode.Additive).completed += OnLoadAsync;
        }

        
        private void OnLoadAsync(AsyncOperation asy)
        {
            Debug.LogAssertion("OnLoadAsync called");
            if (_firstSceneToLoad.GetSceneType() == SceneDataSO.GameSceneType.Menu)
            {
                throw new System.Exception("still not implement loading menu functionality"); 
            }
            else if (_firstSceneToLoad.GetSceneType() == SceneDataSO.GameSceneType.Location)
            {
                PersistentSceneLoader.Instance.LoadLocation<int>(_firstSceneToLoad,0); 
            }
        }


    }

}
