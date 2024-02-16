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
        private AssetReferenceT<SemesterDataSO> _activeSemester;

        private int _currentDate = 0;

        private DailyActionParticipationHandler _dailyActionParticipationHandler;

        public AssetReferenceT<EssentialSceneDataSO> DayProgressionSceneAddress ;
        public AssetReferenceT<SceneDataSO> DormSceneAddress ;

        private void LoadDayProgressionScene()
        {
            //Load to map instead forn now
            //TODO : Load the day progression scene displaying what is previous day and what's next day, and than load the dorm scene

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
            if (_dailyActionParticipationHandler == null)
                _dailyActionParticipationHandler = new DailyActionParticipationHandler(); 

            _dailyActionParticipationHandler.ResetDayAction();

            DayDataSO dayDataSO = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SemesterDataSO>(_activeSemester).GetDayData(_currentDate) ;
            RuntimeDayData newDayData = new RuntimeDayData(dayDataSO) ;

            _currentDate++;

            PersistentActiveDayDatabase.Instance.SetUpNextDay(newDayData);
        }

        public void OnPostPerformSchoolAction()
        {
            _dailyActionParticipationHandler.DecreaseActionPoint(1);

            ///No remaining action point => start new day 
            if (_dailyActionParticipationHandler.IsOutOfActionPoint())
            {
                ClearDayData();
                ProgressToNextDay();
                LoadDayProgressionScene();
                

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
            return _dailyActionParticipationHandler.GetMaxActionPoint() - _dailyActionParticipationHandler.GetRemaiingActionPoint(); 
        }

        

        
        

       
    }
}
