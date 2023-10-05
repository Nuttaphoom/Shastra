using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Rendering;

namespace Vanaring
{
    public class DirectorManager : MonoBehaviour
    {
        //[SerializeField] private ActionSignal actionSignaltest;
        [SerializeField] private List<ActionSignal> _currentSignal = new List<ActionSignal>();       //Waiting signal
        //List<PlayableDirector> _usingDirector = new List<PlayableDirector>();
        private Queue<PlayableDirector> _poolDirector = new Queue<PlayableDirector>();
        private TimelineActorSetupHandler timelineActorHandler;
        public void TransmitSignal(int recv)
        {
            if (_currentSignal.Count == 0)
                Debug.LogError("No registered signal");
            
            for (int i = 0; i < _currentSignal.Count; i++)
            {
                _currentSignal[i].ReceiveSignal(recv);
            }
        }

        public IEnumerator PlayTimeline(ActionSignal signal)
        {
            _currentSignal.Add(signal); 

            // 1.) Create PlayableDirector
            PlayableDirector currentDirector;
            if (_poolDirector.Count > 0)
            {
                currentDirector = _poolDirector.Dequeue();
            }
            else
            {
                //Fix : Instantiate 
                PlayableDirector newDirector = gameObject.AddComponent<PlayableDirector>();
                currentDirector = newDirector; 
            }

            // 2.) Set up the TimelineAsset
            timelineActorHandler.SetUpActor(currentDirector, signal.GetActionTimelineSettingStruct);
            currentDirector.playableAsset = signal.TimelineAsset;

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
                yield return new WaitForEndOfFrame() ;
            }
        }
    }
}
