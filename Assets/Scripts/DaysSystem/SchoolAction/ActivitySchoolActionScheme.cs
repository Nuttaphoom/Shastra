using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using Vanaring.Assets.Scripts.DaysSystem.SchoolAction;

namespace Vanaring 
{
    public class ActivitySchoolActionScheme : MonoBehaviour, ISchoolAction
    {
        [SerializeField]
        private PlayableDirector _director;

        public void OnPerformAcivity()
        {
            StartCoroutine(StartPlayingTimeline()); 
        }

        public IEnumerator StartPlayingTimeline()
        {
            _director.Play();

            while (_director.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }

            yield return null ;

            PostPerformActivity(); 

        }

        public void PostPerformActivity()
        {
            Debug.Log("Post Perform activity"); 
        }

        public void PrePerformActivity()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            OnPerformAcivity(); 
        }

    }
}
