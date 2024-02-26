using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

namespace Vanaring
{
    public class PersistentActiveDayDatabase : PersistentInstantiatedObject<PersistentActiveDayDatabase>
    {
        [SerializeReference]
        private RuntimeDayData _runtimeDayData = null;

        [SerializeField] 
        private DayProgressionHandler _dayProgressionHandler;

        private void Awake()
        {
            TestSetUpFirstDay(); 
        }
        
        private void TestSetUpFirstDay()
        {
            //TODO : This should be refactor to a more appropriate solution
            DayDataSO dayDataSO = _dayProgressionHandler.GetSemesterDataSO.GetDayData(0);
            RuntimeDayData newDayData = new RuntimeDayData(dayDataSO);

            SetUpNextDay(newDayData);
        }


        public void SetUpNextDay(RuntimeDayData data)
        {
            _runtimeDayData = data; 

        }

        public void EndDay()
        {
            StartCoroutine(EndDayCoroutine());
        }

        private IEnumerator EndDayCoroutine()
        {
            yield return _dayProgressionHandler.LoadDayProgressionScene();

            _dayProgressionHandler.ProgressToNextDay();
        }

        public void OnPostPerformSchoolAction()
        {
            _runtimeDayData.ProgressCurrentTime(); 
            _dayProgressionHandler.OnPostPerformSchoolAction(); 
        } 

        public void ProgressNextDaySession()
        {
            PersistentSceneLoader.Instance.LoadMapScene(); 
            //PersistentSceneLoader.Instance.LoadLastVisitedLocation();
        }

        #region Getter 
        //public DayProgressionHandler GetDayProgressionHandler
        //{
        //    get
        //    {
        //        if (_dayProgressionHandler == null)
        //            _dayProgressionHandler = new DayProgressionHandler();

        //        return _dayProgressionHandler;
        //    }
        //}

        public EDayTime GetCurrentDayTime()
        {
            return _runtimeDayData.GetCurrentDayTime; 
        }
        public RuntimeDayData GetActiveDayData
        {
            get
            {
                if (_runtimeDayData == null)
                    throw new System.Exception("RuntimeDayData is null");

                return _runtimeDayData; 
            }
        }
        #endregion
    }
}