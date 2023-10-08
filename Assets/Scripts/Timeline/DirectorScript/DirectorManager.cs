using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using Unity.VisualScripting;

namespace Vanaring
{
    [RequireComponent(typeof(SignalReceiver))]
    public class DirectorManager : MonoBehaviour
    {

        public static DirectorManager Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject) ; 
            }

            Instance = this;

            _signalReceiver = GetComponent<SignalReceiver>();

        }
        //[SerializeField] private ActionSignal actionSignaltest;
        [SerializeField] private List<ActionSignal> _currentSignal = new List<ActionSignal>();       //Waiting signal
        //List<PlayableDirector> _usingDirector = new List<PlayableDirector>();
        private Queue<PlayableDirector> _poolDirector = new Queue<PlayableDirector>();
        private SignalReceiver _signalReceiver;

        public void TransmitSignal(SignalType signal)
        {
            if (_currentSignal.Count == 0)
                Debug.LogError("No registered signal");
            
            for (int i = 0; i < _currentSignal.Count; i++)
            {
                _currentSignal[i].ReceiveSignal(signal);
            }
        }

        public void PlayTimeline(ActionSignal signal)
        {

            _currentSignal.Add(signal); 

            // 1.) Create PlayableDirector
            PlayableDirector currentDirector ;
            TimelineActorSetupHandler timelineActorSetupHandler ; 
            if (_poolDirector.Count > 0)
            {
                currentDirector = _poolDirector.Dequeue();
                currentDirector = currentDirector.GetComponent<PlayableDirector>();
                timelineActorSetupHandler = currentDirector.GetComponent<TimelineActorSetupHandler>();
            }
            else
            {
                //Fix : Instantiate 
                GameObject emptyObj = new GameObject("Director " + (_poolDirector.Count + 1)  );
                currentDirector =  emptyObj.AddComponent<PlayableDirector>() ;
                timelineActorSetupHandler = emptyObj.AddComponent<TimelineActorSetupHandler>(); 
            }

            // 2.) Set up the TimelineAsset
    
            currentDirector.playableAsset = signal.TimelineAsset;

            timelineActorSetupHandler.SetUpActor(currentDirector, signal.GetActionTimelineSettingStruct, _signalReceiver); 

            // 3.) Set currentSignal waiting
            currentDirector.Play();

            // 4.) Wait until Timeline is done
            StartCoroutine (WaitForTimeline(currentDirector));
 
        }
        private IEnumerator WaitForTimeline(PlayableDirector director)
        {
            while (director.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame() ;
            }
            _poolDirector.Enqueue(director);

        }
    }
}
