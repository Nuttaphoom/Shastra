using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Vanaring; 
using Vanaring_Utility_Tool;

namespace Vanaring
{
    public class PersistentSceneLoader : PersistentInstantiatedObject<PersistentSceneLoader>
    {
        private bool _isLoading;

        private string _currentLoadedLocationScene ;
        private string _sceneToLoad;
        private TransitionObject transitionScreenObj;
        [SerializeField]
        private TransitionSceneManager transitionManager;

        private SceneDataSO _last_visisted_location;
        private LoaderDataUser loaderDataUser;

        #region Load Location 

        /// <summary>
        /// Functionality to load and unload previous scene 
        /// </summary>
        /// <param name="sceneSO"></param>

        public void LoadLastVisitedLocation()
        {
            LoadLocation<Null>(_last_visisted_location,
                 null );
        }
        public LoaderDataUser<T> ExtractLastSavedData<T>()
        {
            return loaderDataUser as LoaderDataUser<T>;
        }

        public void LoadLocation<T>(SceneDataSO sceneSO, T transferedData  )  
        {

            if (transferedData != null)
            {
                 loaderDataUser = new LoaderDataUser<T>(transferedData);
            }

            _last_visisted_location =  (sceneSO); 

            LoadGeneralScene(sceneSO) ;


        }
        public void LoadGeneralScene(SceneDataSO sceneSO)
        {
            if (_isLoading)
                return;

            _isLoading = true;

            _sceneToLoad = sceneSO.GetSceneName();

            if (_currentLoadedLocationScene != null)
            {
                UnloadLocation();
            }
            else
            {
                BeginLoadScene();
            }
        }
        private void UnloadLocation()
        {
            if (_currentLoadedLocationScene != null)
                SceneManager.UnloadSceneAsync(_currentLoadedLocationScene);

            BeginLoadScene() ;
        }

        public void OnCompleteLoadedNewLocationScene(AsyncOperation asy)
        {
            _currentLoadedLocationScene = _sceneToLoad;
            //Debug.Log(_currentLoadedLocationScene); 
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentLoadedLocationScene));
            _isLoading = false;
            
        }

        public void CreateTransitionScene()
        {
            if(transitionScreenObj == null)
            {
                transitionScreenObj = Instantiate(transitionManager.TransitionObj, gameObject.transform);
                transitionScreenObj.name = "-- TransitionScreen ----------";
                transitionManager.SetTransitionObj(transitionScreenObj);
                transitionScreenObj.Init(transitionManager);
            }
            
        }

        public IEnumerator CreateTransitionSceneObj()
        {
            transitionManager.SetTransitionObj(Instantiate(transitionManager.TransitionObj, gameObject.transform)); 
            yield return new WaitForEndOfFrame();
        }

        #endregion


        private void BeginLoadScene()
        {
            if (_sceneToLoad == null)
                throw new System.Exception("_sceneToLoad is null");

            CreateTransitionScene();
            transitionManager.TransitionObj.SubOnSceneLoaderBegin(LoadNewScene);
            StartCoroutine(LoadDelayTimer());
        }

        private IEnumerator LoadDelayTimer() {
            yield return new WaitForSeconds(1.0f);
            LoadNewScene(null);
        }

        private void LoadNewScene(Null n)
        {
            //Debug.Log("Fade in complete");
            transitionManager.TransitionObj.UnSubOnSceneLoaderBegin(LoadNewScene);
            StartCoroutine(transitionManager.LoadSceneAsync(_sceneToLoad));
        }
    }

    [System.Serializable]
    public class TransitionSceneManager
    {
        [SerializeField]
        private TransitionSceneSO _transitionSceneSO;

        [SerializeField]
        private TransitionObject transitionObjTemplate;
        private TransitionObject transitionObj;

        private EventBroadcaster _eventBroadCaster;

        public TransitionSceneSO TransitionSO => _transitionSceneSO;
        public TransitionObject TransitionObj => transitionObjTemplate;
        //public TransitionObject  transitionObjTemplate;
        public void SetTransitionObj(TransitionObject t)
        {
            transitionObj = t;
        }
        public IEnumerator LoadSceneAsync(string sceneToLoad)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

            //PersistentSceneLoader.Instance.CreateTransitionScene();


            while (!operation.isDone)
            {
                float progressVal = Mathf.Clamp01(operation.progress / 0.9f);

                GetEventBroadcaster().InvokeEvent<float>(progressVal, "OnSceneLoaderProgress");

                yield return null;
            }
            GetEventBroadcaster().InvokeEvent<Null>(null, "OnSceneLoaderComplete");
            PersistentSceneLoader.Instance.OnCompleteLoadedNewLocationScene(operation);
            yield return new WaitForEndOfFrame();
        }

        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadCaster == null)
            {
                _eventBroadCaster = new EventBroadcaster();
                _eventBroadCaster.OpenChannel<float>("OnSceneLoaderProgress");
                _eventBroadCaster.OpenChannel<Null>("OnSceneLoaderComplete");
            }
            return _eventBroadCaster;
        }

        public void SubOnSceneLoaderOperation(UnityAction<float> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnSceneLoaderProgress");
        }

        public void SubOnSceneLoaderComplete(UnityAction<Null> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnSceneLoaderComplete");
        }

        public void UnSubOnSceneLoaderOperation(UnityAction<float> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnSceneLoaderProgress");
        }

        public void UnSubOnSceneLoaderComplete(UnityAction<Null> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnSceneLoaderComplete");
        }

        
    }
}
 