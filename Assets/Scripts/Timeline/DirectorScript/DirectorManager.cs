using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Vanaring
{
    public class DirectorManager : MonoBehaviour
    {
        //[SerializeField] private ActionSignal actionSignaltest;
        [SerializeField] private List<ActionSignal> _currentSignal = new List<ActionSignal>();       //Waiting signal
        //List<PlayableDirector> _usingDirector = new List<PlayableDirector>();
        private Queue<PlayableDirector> _poolDirector = new Queue<PlayableDirector>();
        private TimelineActorSetupHandler timelineActorHandler;
        private TimelineAsset _timelineAsset;
        public void TransmitSignal(int recv)
        {
            for (int i = 0; i < _currentSignal.Count; i++)
            {
                _currentSignal[i].ReceiveSignal(recv);
            }
            Debug.LogError("No Signal ");
        }

        public IEnumerator PlayTimeline(ActionSignal signal)
        {
            // 1.) Create PlayableDirector
            PlayableDirector currentDirector;
            if (_poolDirector.Count > 0)
            {
                currentDirector = _poolDirector.Dequeue();
            }
            else
            {
                PlayableDirector newDirector = gameObject.AddComponent<PlayableDirector>();
                currentDirector = newDirector; 
            }

            // 2.) Set up the TimelineAsset
            timelineActorHandler.SetUpActor(currentDirector, signal.GetActionTimelineSettingStruct);
            currentDirector.playableAsset = _timelineAsset;

            // 3.) Set currentSignal waiting
            currentDirector.Play();

            // 4.) Wait until Timeline is done
            yield return (WaitForTimeline(currentDirector));


            // 5.) get the PlayableDirector back to the pool
            _poolDirector.Enqueue(currentDirector);
        }
        private IEnumerator WaitForTimeline(PlayableDirector director)
        {
            while (director.state == PlayState.Playing)
            {
                yield return null;
            }
        }
    }
}
