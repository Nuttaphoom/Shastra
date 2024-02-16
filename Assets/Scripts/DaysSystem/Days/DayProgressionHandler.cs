using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Vanaring.Assets.Scripts.Utilities.Cutscene_System;

namespace Vanaring 
{

    [Serializable]
    public class DayProgressionHandler
    {
        [SerializeField]
        private AssetReferenceT<SemesterDataSO> _activeSemester;

        private int _currentDate = 0;

        private DailyActionParticipationHandler _dailyActionParticipationHandler;

        public AssetReferenceT<SceneDataSO> DayProgressionScene ;

        private void LoadDayProgressionScene()
        {
            //Load to map instead forn now
            //TODO : Load the day progression scene displaying what is previous day and what's next day, and than load the dorm scene
        }

        public RuntimeDayData NewDayBegin()
        {
            PersistentSceneLoader.Instance.LoadLastVisitedLocation();

            if (_dailyActionParticipationHandler == null)
                _dailyActionParticipationHandler = new DailyActionParticipationHandler(); 

            _dailyActionParticipationHandler.ResetDayAction();

            DayDataSO dayDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SemesterDataSO>(_activeSemester).GetDayData(_currentDate) ;
            RuntimeDayData newDayData = new RuntimeDayData(dayDataSO) ;

            _currentDate++;

            return newDayData; 
        }

        public void OnPostPerformSchoolAction()
        {
            _dailyActionParticipationHandler.DecreaseActionPoint(1);

            ///No remaining action point => start new day 
            if (_dailyActionParticipationHandler.IsOutOfActionPoint())
                throw new NotImplementedException( "new day begin function won't work yet" ) ;

            //Action remains
            else 
            {
                PersistentSceneLoader.Instance.LoadLastVisitedLocation(); 
            }

        }

        private void GetBackToLastVisitedLocationScene()
        {
            
        }

        public int GetCurrentDayTime()
        {
            return _dailyActionParticipationHandler.GetMaxActionPoint() - _dailyActionParticipationHandler.GetRemaiingActionPoint(); 
        }

        

        
        

       
    }
}
