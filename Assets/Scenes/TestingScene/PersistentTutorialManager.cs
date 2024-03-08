using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using Vanaring.Assets.Scripts.Utilities.StringConstant;
using static Vanaring.TuitorialDatabaseSO;
using System.Linq;

namespace Vanaring
{
    public class PersistentTutorialManager : PersistentInstantiatedObject<PersistentTutorialManager>
    {
        [SerializeField]
        private bool EnableDebuggingMode = false;

        private List<GameObject> tutorials = new List<GameObject>();

        private List<string> _dirtyKey = new List<string>();

        private TuitorialDatabaseSO _tuitorialDatabaseSO ;


        [SerializeField]
        private List<TuitorialInstanceData> _tuitorialInstanceDatas;


        private void Awake()
        {
            if (_tuitorialInstanceDatas != null)
            {
                foreach (var data in _tuitorialInstanceDatas)
                {
                    data.TuitorialCutscene.gameObject.SetActive(false);
                }
            }
        }
        public IEnumerator CheckTuitorialNotifier(string tuitorialKey)
        {
            if (EnableDebuggingMode)
                goto Ret;

                //if (_tuitorialDatabaseSO == null)
                //    _tuitorialDatabaseSO =  PersistentAddressableResourceLoader.Instance.LoadResourceOperation<TuitorialDatabaseSO>(DatabaseAddressLocator.GetTuitorialDatabaseSOAddress) ;
            if (_tuitorialInstanceDatas == null)
                goto Ret ; 

            TuitorialInstanceData tuitorialData = null;

            foreach (var data in _tuitorialInstanceDatas)
            {
                if (data.TutorialNotifyKey != tuitorialKey)
                    continue; 

                tuitorialData = data ; 
            }
            if (tuitorialData != null)
                yield return PlayTutorial(tuitorialData);

            Ret:
            yield return null; 

        }

        private IEnumerator PlayTutorial(TuitorialInstanceData tuitorialData)
        {
            tuitorialData.TuitorialCutscene.gameObject.SetActive(true) ;
            yield return tuitorialData.TuitorialCutscene.PlayCutscene();

            Destroy(tuitorialData.TuitorialCutscene.gameObject);

            _tuitorialInstanceDatas.Remove(tuitorialData);
        }


    }
}
