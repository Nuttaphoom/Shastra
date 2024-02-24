using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

namespace Vanaring
{
    [Serializable]
    [RequireComponent(typeof(PlayableDirector))]
    public class CutsceneDirector : MonoBehaviour
    {
        private PlayableDirector _director;

        private void Awake()
        {
            _director = GetComponent<PlayableDirector>(); 

            if (_director == null )
                throw new Exception("_director could not be found") ;
            
        }

        public IEnumerator PlayCutscene()
        {
            _director.Play();

            while (_director.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
