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
        private CutsceneDirector _director;

        [SerializeField]
        private PersonalityTraitRewardDisplayer _rewardDisplayer;

        List<PersonalityRewardData> _personalityRewards;


        private void Start()
        {
            _personalityRewards = PersistentSceneLoader.Instance.ExtractSavedData<List<PersonalityRewardData>>(PersistentSceneLoader.Instance.GetStackLoadedDataScene().GetSceneID()).GetData();
            
            if (_personalityRewards == null)
                throw new Exception("_personalityRewards is null");

            OnPerformAcivity();
        }
        public void OnPerformAcivity()
        {
            StartCoroutine(StartPlayingTimeline()); 
        }

        public IEnumerator StartPlayingTimeline()
        {
            yield return _director.PlayCutscene(); 

            StartCoroutine(PostPerformActivity()) ; 

        }

        public IEnumerator PostPerformActivity()
        {
            yield return (_rewardDisplayer.DisplayRewardUICoroutine(_personalityRewards) ) ;

            PersistentActiveDayDatabase.Instance.GetDayProgressionHandler.OnPostPerformSchoolAction();

            //finish display(done)(close window) -> (update backend/load new scene)(Potae) -> Persistent
        }

        public void SubmitActionReward()
        {
            throw new NotImplementedException();
        }
    }
}
