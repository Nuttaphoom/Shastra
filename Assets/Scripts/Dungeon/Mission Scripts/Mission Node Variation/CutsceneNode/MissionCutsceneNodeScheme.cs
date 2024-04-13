using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public class MissionCutsceneNodeScheme : MonoBehaviour, ISceneLoaderWaitForSignal
    {
        [SerializeField]
        private CutsceneDirector _director;

        public IEnumerator OnNewSceneLoad_BeforeSaveLoadPerform()
        {
            yield return null; 
        }

        public IEnumerator OnNotifySceneLoadingComplete()
        {
            OnPerformAcivity(); 

            yield return null; 
        }

        private void OnPerformAcivity()
        {
            StartCoroutine(StartPlayingTimeline());
        }

        private void PostPerformActivity()
        {
            PersistentSceneLoader.Instance.LoadGeneralScene(PersistentSceneLoader.Instance.GetStackLoadedDataScene(1));
        }

        private IEnumerator StartPlayingTimeline()
        {
            yield return _director.PlayCutscene();

            PostPerformActivity() ;

        }
    }
}
