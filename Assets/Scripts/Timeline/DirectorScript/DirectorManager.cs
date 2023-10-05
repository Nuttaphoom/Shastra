using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Vanaring
{
    public class DirectorManager : MonoBehaviour
    {
        //List<ActionSignal> _currentSignal;
        List<PlayableDirector> _usingDirector;
        Queue<PlayableDirector> _poolDirector;
        private PlayableDirector nuo;
        public void TransmitSignal(SignalType signal)
        {
            //_currentSignal.Add(signal);
            if (signal == SignalType.SignalA)
            {
                
            }
        }

        public IEnumerator PlayTimeline()
        {
            PlayableDirector currentDirector = _poolDirector.Peek();
            currentDirector.Play();

            //1.) Create PlayableDirector
            //2.) Set up the TimelineAsset
            //3.) Set currentSignal waiting
            yield return new WaitForSeconds((float)nuo.time);
            //4.) Wait until Timeline is done
            //5.) get the PlayableDirector back to the pool
            yield return null;
        }
    }
}
