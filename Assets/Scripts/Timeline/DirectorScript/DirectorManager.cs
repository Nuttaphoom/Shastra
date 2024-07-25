using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System.Management.Instrumentation;
using UnityEngine.Events;
using Language.Lua;

namespace Vanaring
{
    [RequireComponent(typeof(SignalReceiver))]
    public class DirectorManager : MonoBehaviour
    {
        #region Event Broadcaster 
        private EventBroadcaster _eventBroadcaster;

        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
                _eventBroadcaster.OpenChannel<(List<CombatEntity>, ActionTimelinePrefab)>("OnPlayTimelineWithActor");
        
            }

            return _eventBroadcaster;
        }

        public void SubOnPlayTimelineWithActor(UnityAction<(List<CombatEntity>, ActionTimelinePrefab)> argc)
        {
            GetEventBroadcaster().SubEvent< (List<CombatEntity>, ActionTimelinePrefab)> (argc, "OnPlayTimelineWithActor");
        }

        public void UnSubOnPlayTimelineWithActor(UnityAction<(List<CombatEntity>, ActionTimelinePrefab)> argc)
        {
            GetEventBroadcaster().UnSubEvent<(List<CombatEntity>, ActionTimelinePrefab)>(argc, "OnPlayTimelineWithActor");
        }

        #endregion
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

        private void Start()
        {
            CombatReferee.Instance.SubOnNewRoundBegin(OnNewRoundBegin_DestroyUnusedAnimationPrefab);
        }

        private void OnNewRoundBegin_DestroyUnusedAnimationPrefab(ECompetatorSide side)
        {
            ClearCurrentTimeline();
        }

        [SerializeField] private List<ActionSignal> _currentSignal = new List<ActionSignal>();       
        private SignalReceiver _signalReceiver;

        private ActionTimelinePrefab _currentTimelineActorSetupHandler;
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
            ClearCurrentTimeline(); 

            if (_currentPlayableDirector != null)
                throw new System.Exception("Try to play multiple timeline simutanouly");


            _currentSignal.Add(signal); 

            // 1.) Create PlayableDirector
            PlayableDirector currentDirector ;
 
            //1.1) instantiate TimelineActorSetupHanlder 
            var actorSetupHandler = Instantiate( signal.GetActionTimelinePrefab, Vector3.zero /* signal.GetActionTimelineSettingStruct.GetTimelineActorWithIndex(0).GetComponent<CombatEntityAnimationHandler>().GetVisualMesh().transform.position*/, Quaternion.identity /*signal.GetActionTimelinePrefab.transform.rotation*/ )  ;
            currentDirector = actorSetupHandler.GetComponent<PlayableDirector>() ;
            _currentTimelineActorSetupHandler = actorSetupHandler.GetComponent<ActionTimelinePrefab>();

            List<CombatEntity> combatActors = new List<CombatEntity>();
            foreach (var obj in signal.GetActionTimelineSettingStruct.GetAllTimelineActors())
                combatActors.Add(obj.GetComponent<CombatEntity>());
            
            GetEventBroadcaster().InvokeEvent<(List<CombatEntity>, ActionTimelinePrefab)>((combatActors, _currentTimelineActorSetupHandler), "OnPlayTimelineWithActor");

            // 2.) Set up the TimelineAsset
            _currentTimelineActorSetupHandler.SetUpActor(currentDirector, signal.GetActionTimelineSettingStruct, _signalReceiver);

      
            

            // 3.) Set currentSignal waiting
            currentDirector.Play();


            // 4.) Wait until Timeline is done
            StartCoroutine(WaitForTimeline(currentDirector));
 
        }

        /// <summary>
        /// Calling Timeline without Action won't make the flow of the game stop 
        /// make sure  that at least one Actor is given  
        /// </summary>
        /// <param name="timelineActorSetupHandler"></param>
        /// <param name="actionTimelineSettingStruct"></param>
        public IEnumerator PlayTimelineCoroutine(TimelineInfo info, List<CombatEntity> actors )
        {
            ClearCurrentTimeline();

            // 1.) Create PlayableDirector
            PlayableDirector currentDirector;

            //As actor used in timeline won't be set up from Action class, we need to set them up here

            var timelineSettingStruct = new ActionTimelineSettingStruct(info.GetActionTimeLineSettingStruct) ;

            foreach (var actor in actors)
                timelineSettingStruct.AddActors(actor.gameObject); 

            
            //1.1) instantiate TimelineActorSetupHanlder 
            var actorSetupHandler = Instantiate(info.GetTimelineActorSetupHandler, timelineSettingStruct.GetTimelineActorWithIndex(0).GetComponent<CombatEntityAnimationHandler>().GetVisualMesh().transform.position, info.GetTimelineActorSetupHandler.transform.rotation);
            currentDirector = actorSetupHandler.GetComponent<PlayableDirector>();
            _currentTimelineActorSetupHandler = actorSetupHandler.GetComponent<ActionTimelinePrefab>();

            // 3.) signal broadcast that we want to play
            GetEventBroadcaster().InvokeEvent<(List<CombatEntity>, ActionTimelinePrefab)>((actors, _currentTimelineActorSetupHandler), "OnPlayTimelineWithActor");


            // 2.) Set up the TimelineAsset
            _currentTimelineActorSetupHandler.SetUpActor(currentDirector, timelineSettingStruct, _signalReceiver);

            Debug.Log("_currentPlayableDirector  is " + _currentPlayableDirector); 

        
            // 4.) Set currentSignal waiting
            currentDirector.Play(); 


            //// 5.) Wait until Timeline is done
            yield return (WaitForTimeline(currentDirector));
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
            
            if (_currentTimelineActorSetupHandler == null)
                return; 

            _currentTimelineActorSetupHandler.DestroyTimelineElement();

            if (_currentPlayableDirector == null)
                return;

            Destroy(_currentPlayableDirector);

            _currentPlayableDirector = null;
            _currentPlayableDirector = null; 


        }

        #region GETTER
        public bool IsPlayingTimeline => _playingTimeline;

        #endregion
    }
}
