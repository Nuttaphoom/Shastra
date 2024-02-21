using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
        private AssetReferenceT<SemesterDataSO> _activeSemesterAddress ;
        private SemesterDataSO _currentSemester;

        #region GETTER 
        public SemesterDataSO GetSemesterDataSO
        {
            get
            {
                if (_currentSemester == null)
                {
                    _currentSemester  = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SemesterDataSO>(_activeSemesterAddress);
                }
                return _currentSemester;
            }
        }
        #endregion
        private int _currentDate = 0;

        private DailyActionParticipationHandler _dailyActionParticipationHandler;

        public DailyActionParticipationHandler GetDailyActionParticipationHandler
        {
            get
            {
                if (_dailyActionParticipationHandler == null)
                    _dailyActionParticipationHandler = new DailyActionParticipationHandler();

                return _dailyActionParticipationHandler; 
            }
        } 
 

        [SerializeField]
        private AssetReferenceT<EssentialSceneDataSO> DayProgressionSceneAddress ;
        [SerializeField] 
        private AssetReferenceT<SceneDataSO> DormSceneAddress ;

        [SerializeField]
        private DayProgressionDisplayer _progressionDisplayer;

        public IEnumerator LoadDayProgressionScene()
        {
            //Load to map instead forn now
            //TODO : Load the day progression scene displaying what is previous day and what's next day, and than load the dorm scene
            SceneDataSO dayDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(DayProgressionSceneAddress);
            PersistentSceneLoader.Instance.LoadGeneralScene(dayDataSO);

            DayDataSO PreviousDayData  = GetSemesterDataSO.GetDayData(_currentDate);
            DayDataSO NextDayData = GetSemesterDataSO.GetDayData(_currentDate + 1); 
            yield return (_progressionDisplayer.DisplayRewardUICoroutine(new DayProgressionData() { 
                NextDay = _currentDate + 1 , 
                PreviousDay = _currentDate, 
                NextDayDataSO = NextDayData  , 
                PreviousDayDataSO = PreviousDayData 
            })); 

            //Right now just load the correspond scene 
            LoadCorrespondScene();

        }

        /// <summary>
        /// This is either Dorm scene or special cutscene before starting the scene 
        /// </summary>
        private void LoadCorrespondScene()
        {
            SceneDataSO dayDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SceneDataSO>(DormSceneAddress);

            PersistentSceneLoader.Instance.LoadGeneralScene(dayDataSO);
        }

        private void ClearDayData()
        {
            PersistentSceneLoader.Instance.ClearUserDataOfTempVisitedLocationData();
        } 
        public void ProgressToNextDay()
        {
            ClearDayData();  

            GetDailyActionParticipationHandler.ResetDayAction();

            DayDataSO dayDataSO = GetSemesterDataSO.GetDayData(_currentDate) ;
            RuntimeDayData newDayData = new RuntimeDayData(dayDataSO) ;

            _currentDate++;

            PersistentActiveDayDatabase.Instance.SetUpNextDay(newDayData);

            LoadCorrespondScene(); 
        }

        public void OnPostPerformSchoolAction()
        {
            GetDailyActionParticipationHandler.DecreaseActionPoint(1);

            ///No remaining action point => start new day 
            if (_dailyActionParticipationHandler.IsOutOfActionPoint())
            {
                PersistentActiveDayDatabase.Instance.EndDay(); 
            }

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
            return GetDailyActionParticipationHandler.GetMaxActionPoint() - _dailyActionParticipationHandler.GetRemaiingActionPoint(); 
        }

        

        
        

       
    }
}
