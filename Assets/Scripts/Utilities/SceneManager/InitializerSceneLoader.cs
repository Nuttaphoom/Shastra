using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
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
            SceneManager.LoadSceneAsync(_persistantScene.GetSceneName(), LoadSceneMode.Additive).completed += OnLoadAsync;
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.F1))
            //{
            //    PersistentActiveDayDatabase.Instance.GetActiveDayData.GetAvailableLocation()[0].LoadThisLocation();
            //}
            //if (Input.GetKeyDown(KeyCode.F2))
            //{
            //    PersistentActiveDayDatabase.Instance.GetActiveDayData.GetAvailableLocation()[1].LoadThisLocation();
            //}
            //if (Input.GetKeyDown(KeyCode.F3))
            //{
            //    PersistentActiveDayDatabase.Instance.GetActiveDayData.GetAvailableLocation()[2].LoadThisLocation();
            //}
        }
        private void OnLoadAsync(AsyncOperation asy)
        {
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
