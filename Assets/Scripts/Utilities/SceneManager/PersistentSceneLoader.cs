using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        private SceneDataSO _sceneToLoad;
        private TransitionObject transitionScreenObj;


        [SerializeField]
        private TransitionSceneManager transitionManager;

        //For loading last visisted location scene 
        private SceneDataSO _last_visisted_location; 
    
        //For extract last vissted location save data user 
        private LoaderDataUser _last_visisted_loaderDataUser;

        /// <summary>
        /// Scene index of the user of the data 
        /// if scene A create Object for scene B, key will be the index of scene B 
        /// </summary>
        private Dictionary<string, LoaderDataUser> _userDataScene = new Dictionary<string, LoaderDataUser>(); 

        //Renenber last scene to load, this include both Location and general scene 
        private Stack<SceneDataSO> _stackLoadedSceneData = new Stack<SceneDataSO>()  ;

        #region GETTER
        public SceneDataSO GetStackLoadedDataScene()
        {
            if (_stackLoadedSceneData.TryPeek(out SceneDataSO sceneData))
                return sceneData;

            throw new System.Exception("StackLoadedSceneData is null"); 
        }

        #endregion

        #region Load Location 

        //public void LoadLastVisitedLocation()
        //{
        //   LoadLocation<Null>(_last_visisted_location,
        //        null); 

        //    ;
        //}

        #region Public Methods
        /// <summary>
        /// Clear data related to last visiste location only 
        /// No need to clear _userDataScene and _stackLoadedSceneData
        /// </summary>
        public void CreateTransitionScene()
        {
            if (transitionScreenObj == null)
            {
                transitionScreenObj = Instantiate(transitionManager.TransitionObj, gameObject.transform);
                transitionScreenObj.name = "-- TransitionScreen ----------";
                //transitionManager.SetTransitionObj(transitionScreenObj);
                transitionScreenObj.Init(transitionManager);
            }
        }
        public void OnCompleteLoadedNewLocationScene(AsyncOperation asy)
        {
            _currentLoadedLocationScene = _sceneToLoad.GetSceneName();
            //Debug.Log(_currentLoadedLocationScene); 
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_currentLoadedLocationScene));
            _isLoading = false;

        }

        public void ClearUserDataOfTempVisitedLocationData()
        {
            _last_visisted_loaderDataUser = null;
            _last_visisted_location = null;
        }
        public LoaderDataUser<T> ExtractLastSavedData<T>()
        {
            if (_last_visisted_loaderDataUser == null)
                throw new System.Exception("_last_visisted_loaderDataUser is null");

            return _last_visisted_loaderDataUser as LoaderDataUser<T>;
        }
        public LoaderDataUser<T> ExtractSavedData<T>(string uniqueID)
        {
            if (!_userDataScene.ContainsKey(uniqueID))
                throw new System.Exception("_userDataSccene doesn't contain " + uniqueID);

            //var ret = _userDataScene[uniqueID];
            //////_userDataScene.Remove(uniqueID);

            if (_userDataScene[uniqueID] == null)
                throw new System.Exception("ret is null");

            return _userDataScene[uniqueID] as LoaderDataUser<T>;
        }
        public void LoadLocation<T>(SceneDataSO sceneSO, T transferedData)
        {
            ColorfulLogger.LogWithColor("Load Location", Color.red);

            if (transferedData != null)
                _last_visisted_loaderDataUser = CreateLoaderDataUser(sceneSO.GetSceneID(), transferedData);

            _last_visisted_location = (sceneSO);

            LoadGeneralScene(sceneSO);
        }
        public LoaderDataUser CreateLoaderDataUser<T>(string id, T transferedData)
        {
            if (transferedData != null)
            {
                LoaderDataUser<T> user = new LoaderDataUser<T>(transferedData);

                if (_userDataScene.ContainsKey(id))
                    _userDataScene.Remove(id);

                _userDataScene.Add(id, user);

                return user;
            }

            return null;
        }
        public void LoadGeneralScene(SceneDataSO sceneSO)
        {
            if (_isLoading)
                return;

            _isLoading = true;

            _sceneToLoad = sceneSO;

            if (_currentLoadedLocationScene != null)
            {
                UnloadLocation();
            }
            else
            {
                BeginLoadScene();
            }
        }
        #endregion

        private void UnloadLocation()
        {
            if (_currentLoadedLocationScene != null)
                SceneManager.UnloadSceneAsync(_currentLoadedLocationScene);

            BeginLoadScene() ;
        }

        #endregion

        #region Load Specific Location 
        [SerializeField]
        private AssetReferenceT<SceneDataSO> _mapSceneDataAddress;

        private SceneDataSO _mapSceneDataSO; 
        public void LoadMapScene()
        {
            if (_mapSceneDataSO == null)
                _mapSceneDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(_mapSceneDataAddress);

            LoadGeneralScene(_mapSceneDataSO);
        }

        #endregion

        private void BeginLoadScene()
        {
            if (_sceneToLoad == null)
                throw new System.Exception("_sceneToLoad is null");

            PersistentSaveLoadManager.Instance.CaptureToTemp();
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
            _stackLoadedSceneData.Push(_sceneToLoad);

            transitionManager.TransitionObj.UnSubOnSceneLoaderBegin(LoadNewScene);
            StartCoroutine(transitionManager.LoadSceneAsync(_sceneToLoad));
        }

        #region IAwakeable Call
        public IEnumerator NotifySceneLoadingComplete()
        {
            foreach (var awakeable in FindObjectsOfType<MonoBehaviour>())
            {
                if (awakeable is ISceneLoaderWaitForSignal)
                {
                    yield return (awakeable as ISceneLoaderWaitForSignal).OnNotifySceneLoadingComplete();
                }
            }
        }
        #endregion
    }

    [System.Serializable]
    public class TransitionSceneManager
    {
        [SerializeField]
        private TransitionSceneSO _transitionSceneSO;
        private EventBroadcaster _eventBroadCaster;
        public TransitionSceneSO TransitionSO => _transitionSceneSO;
        public TransitionObject TransitionObj => _transitionSceneSO.GetTransitionObject;

        public IEnumerator LoadSceneAsync(SceneDataSO sceneToLoad)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad.GetSceneName(), LoadSceneMode.Additive);

            while (!operation.isDone)
            {
                float progressVal = Mathf.Clamp01(operation.progress / 0.9f);

                GetEventBroadcaster().InvokeEvent<float>(progressVal, "OnSceneLoaderProgress");

                yield return null;
            }
            PersistentSaveLoadManager.Instance.RestoreFromTemp();

            yield return PersistentSceneLoader.Instance.NotifySceneLoadingComplete();

            PersistentSceneLoader.Instance.OnCompleteLoadedNewLocationScene(operation);

            GetEventBroadcaster().InvokeEvent<Null>(null, "OnSceneLoaderComplete");

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
 