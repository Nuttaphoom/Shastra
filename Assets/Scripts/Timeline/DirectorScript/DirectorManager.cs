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

        private bool _playingTimeline = false;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject) ; 
            }

            Instance = this;

            _signalReceiver = GetComponent<SignalReceiver>();

        }


        [SerializeField] private List<ActionSignal> _currentSignal = new List<ActionSignal>();       
        private SignalReceiver _signalReceiver;

        private TimelineActorSetupHandler _currentTimelineActorSetupHandler;
        private PlayableDirector _currentPlayableDirector; 
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
            if (_currentPlayableDirector != null)
                throw new System.Exception("Try to play multiple timeline simutanouly"); 

            _currentSignal.Add(signal); 

            // 1.) Create PlayableDirector
            PlayableDirector currentDirector ;
 
            //1.1) instantiate TimelineActorSetupHanlder 
            var actorSetupHandler = Instantiate( signal.GetTimelineActorSetupHanlder, signal.GetActionTimelineSettingStruct.GetObjectWithIndex(0).GetComponent<CombatEntityAnimationHandler>().GetVisualMesh().transform.position, signal.GetTimelineActorSetupHanlder.transform.rotation )  ;
            currentDirector = actorSetupHandler.GetComponent<PlayableDirector>() ;
            _currentTimelineActorSetupHandler = actorSetupHandler.GetComponent<TimelineActorSetupHandler>();

            // 2.) Set up the TimelineAsset
            _currentTimelineActorSetupHandler.SetUpActor(currentDirector, signal.GetActionTimelineSettingStruct, _signalReceiver); 

            // 3.) Set currentSignal waiting
            currentDirector.Play();

            // 4.) Wait until Timeline is done
            StartCoroutine (WaitForTimeline(currentDirector));
 
        }

        private IEnumerator WaitForTimeline(PlayableDirector director)
        {
            _playingTimeline = true; 
            while (director.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame() ;
            }
            _playingTimeline = false;
        }

        /// <summary>
        /// will be called in the ActorActioNFactory
        /// </summary>
        public void ClearCurrentTimeline()
        {
            _currentTimelineActorSetupHandler.DestroyTimelineElement();
            Destroy(_currentPlayableDirector);

            _currentPlayableDirector = null;
            _currentPlayableDirector = null; 


        }

        #region GETTER
        public bool IsPlayingTimeline => _playingTimeline;

        #endregion
    }
}
