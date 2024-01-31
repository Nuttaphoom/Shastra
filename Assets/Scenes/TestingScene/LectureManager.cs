using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Data;
using Vanaring.Assets.Scripts.DaysSystem.SchoolAction;
using Language.Lua;
using static UnityEditor.PlayerSettings;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;
using System.Diagnostics.Eventing.Reader;
using Vanaring;

namespace Vanaring
{
    public class LectureManager : MonoBehaviour, ISaveable, ISchoolAction
    {
        [SerializeField]
        private LectureRewardDisplayer _rewardDisplayer ; 
        //the default increased amount 
        private const int increaseAmount = 1;

        [SerializeField]
        private CutsceneDirector _director;

        // Lecture to study 

        //private List<LectureSO> lectures = new List<LectureSO>();

        [Header("DONT ASSIGN ANYTHING")] 
        [SerializeField] 
        private LectureSO _lectureToStudy ; 

        private List<ProgressData> localSaveProgress = new List<ProgressData>();

        static private List<LectureSubjectRuntime> lectureRuntimes = new List<LectureSubjectRuntime>();

        private void Awake()
        {
            if (lectureRuntimes.Count > 0)
                return;

            InitLectureRuntime();

            OnPerformAcivity(); 
        }

        ////[ContextMenu("Load Scene")]
        //public void LoadScene()
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene("PotaeTesterScene");
        //}

        //[ContextMenu("Init Runtime")]
        public void InitLectureRuntime()
        {
            _lectureToStudy = (PersistentSceneLoader.Instance.ExtractSavedData<LectureSO>(PersistentSceneLoader.Instance.GetStackLoadedDataScene().GetSceneID())).GetData();

            if (_lectureToStudy == null)
                throw new Exception("lectureToStudy is null") ;

            if (lectureRuntimes.Count > 0)
                return;

            bool lectureAdded = false; 
            foreach (ProgressData progressData in localSaveProgress)
            {
                if (progressData.lectureName == _lectureToStudy.LectureName)
                {
                    lectureRuntimes.Add(new LectureSubjectRuntime(_lectureToStudy, progressData));
                    lectureAdded = true;
                }
            }
            if (!lectureAdded)
            {
                lectureRuntimes.Add(new LectureSubjectRuntime(_lectureToStudy));
            }

         

        }

        public int CalculateReceivedReward()
        {
            return increaseAmount;
        }

        public void IncreaseExp(string LectureName)
        {
            foreach (LectureSubjectRuntime LectureSubjectRuntime in lectureRuntimes)
            {
                if (LectureSubjectRuntime.LectureName == LectureName)
                {
                    LectureSubjectRuntime.RecievePoint(CalculateReceivedReward()) 
                        ;
                    if (LectureSubjectRuntime.SetRecieveReward())
                    {
                        Debug.Log("real Get: " + LectureSubjectRuntime.GetRecievedReward().Count);
                    }
                }
            }
        }

      

        #region Save System
        public object CaptureState()
        {
            List<ProgressData> tempProgress = new List<ProgressData>();
            foreach (LectureSubjectRuntime LectureSubjectRuntime in lectureRuntimes)
            {
                ProgressData progressData;

                progressData.lectureName = LectureSubjectRuntime.LectureName;
                progressData.progessPoint = LectureSubjectRuntime.CurrentPoint;

                tempProgress.Add(progressData);
            }

            return new SaveData
            {
                savedProgress = tempProgress
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            localSaveProgress = saveData.savedProgress;
        }

        [Serializable]
        public struct ProgressData
        {
            public string lectureName;
            public int progessPoint;
        }

        [Serializable]
        private struct SaveData
        {
            public List<ProgressData> savedProgress;
        }

        #endregion

        #region ISchoolAction Methods 
        public void OnPerformAcivity()
        {
            StartCoroutine(StartPlayingTimeline());
        }

        public IEnumerator StartPlayingTimeline()
        {
            yield return _director.PlayCutscene();

            PostPerformActivity();
        }

        public void PostPerformActivity()
        {

            foreach (LectureSubjectRuntime lectureRuntime in lectureRuntimes)
            {

                if (lectureRuntime.LectureName != _lectureToStudy.LectureName) 
                    continue;

                var reward = new LectureRewardStruct()
                {
                    maxEXP = lectureRuntime.MaxPoint,
                    currentEXP = lectureRuntime.CurrentPoint,
                    gainedEXP = CalculateReceivedReward(),
                    rewardData = lectureRuntime.GetEventReward()   
                }; 

             
                StartCoroutine(_rewardDisplayer.DisplayRewardUICoroutine(reward));

                break; 
            }
        }
        #endregion
    }

    public class LectureSubjectRuntime
    {
        [SerializeField]
        private string lectureName = "";
        [SerializeField]
        private int currentPoint = -1;
        [SerializeField]
        private int maxPoint = -1;
        [SerializeField]
        private List<LectureChechpoint> checkpoint = new List<LectureChechpoint>();

        private List<EventReward> recievedReward = new List<EventReward>();
        
        public LectureSubjectRuntime()
        {

        }

        public LectureSubjectRuntime(LectureSO lectureSO)
        {
            lectureName = lectureSO.LectureName;
            currentPoint = 0;
            maxPoint = lectureSO.maxPoint;
            checkpoint = lectureSO.Checkpoint;
        }
        public LectureSubjectRuntime(LectureSO lectureSO, LectureManager.ProgressData currentProgress)
        {
            lectureName = lectureSO.LectureName;
            currentPoint = currentProgress.progessPoint;
            maxPoint = lectureSO.maxPoint;
            checkpoint = lectureSO.Checkpoint;
        }

        public void RecievePoint(int amount)
        {
            currentPoint += amount;

            if (currentPoint > maxPoint)
                currentPoint = maxPoint;

            //GetReward();
        }

        public bool SetRecieveReward()
        {
            bool getReward = false;
            foreach (LectureChechpoint lectureChechpoint in checkpoint)
            {
                if (lectureChechpoint.Received == true)
                {
                    continue;
                }
                else if (currentPoint >= lectureChechpoint.RequirePoint)
                {
                    lectureChechpoint.SetReceived(true);
                    recievedReward.Add(lectureChechpoint.Reward);
                    getReward = true;
                }
            }
            return getReward;
        }

        public List<EventReward> GetRecievedReward()
        {
            List<EventReward> temp = new List<EventReward>();
            foreach (var rew in recievedReward)
            {
                temp.Add(rew);
            }
            recievedReward.Clear();
            return temp;
        }

        public List<EventReward> GetEventReward()
        {
            List<EventReward> ret = new List<EventReward>(); 
            foreach (var point in checkpoint)
            {
                ret.Add(point.Reward); 
            } 
            return ret ; 
        }

        //public void PrintData()
        //{
        //    Debug.Log(LectureName + "," + currentPoint + "," + maxPoint);
        //}

        #region Getter
        public string LectureName => lectureName;
        public int CurrentPoint => currentPoint;
        public int MaxPoint => maxPoint;
        #endregion
    }
}
