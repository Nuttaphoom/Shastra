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

        [SerializeField]
        private PersonalityTraitRewardDisplayer _rewardDisplayer ;
        private void Start()
        {
            OnPerformAcivity();
        }
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

            StartCoroutine(_rewardDisplayer.DisplayRewardUICoroutine(new PersonalityRewardData() ) ) ; 
            //finish display(done)(close window) -> (update backend/load new scene)(Potae) -> Persistent
        }
    }
}
