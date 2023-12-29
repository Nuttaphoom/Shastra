using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vanaring.Assets.Scripts.Utilities;

namespace Vanaring
{
    public class PersistentSceneLoader : PersistentInstantiatedObject<PersistentSceneLoader>
    {
        private bool _isLoading;

        private string _currentLoadedLocationScene ;
        private string _sceneToLoad;

        #region Load Location 
        public void LoadLocation<T>(SceneDataSO sceneSO, T transferedData) where T : struct
        {
            if (_isLoading)
                return ;

            _isLoading = true;
            _sceneToLoad = sceneSO.GetSceneName();
           
            GameObject newObject = new GameObject(""+sceneSO.name + " data container") ;

            newObject.AddComponent<LoaderDataContainer>() ; 

            var loaderDataUser = new LoaderDataUser<T>(transferedData) ;

            newObject.GetComponent<LoaderDataContainer>().SetDataUser(loaderDataUser);

            DontDestroyOnLoad(newObject); 

            if (_currentLoadedLocationScene != null)
            {
                UnloadLocation(); 
            }else
            {
                LoadNewScene(); 
            }



        }

        private void UnloadLocation()
        {
            if (_currentLoadedLocationScene != null)
                SceneManager.UnloadSceneAsync(_currentLoadedLocationScene);

            LoadNewScene() ;
        }

        private void OnCompleteLoadedNewLocationScene(AsyncOperation asy)
        {
            _currentLoadedLocationScene = _sceneToLoad;
            //Debug.Log(_currentLoadedLocationScene); 
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentLoadedLocationScene));
            _isLoading = false;
        }

        #endregion


        private void LoadNewScene()
        {
            if (_sceneToLoad == null)
                throw new System.Exception("_sceneToLoad is null");

            AsyncOperation asy = SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);
            asy.completed += OnCompleteLoadedNewLocationScene;

        }



    }
}
