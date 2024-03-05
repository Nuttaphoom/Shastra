using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Data;
using Vanaring.Assets.Scripts.DaysSystem.SchoolAction;
using Language.Lua;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;
using System.Diagnostics.Eventing.Reader;
using Vanaring;

namespace Vanaring
{
    public class LectureParticipationScheme : MonoBehaviour, ISaveable, ISchoolAction, ISceneLoaderWaitForSignal
    {
        [SerializeField]
        private LectureRewardDisplayer _rewardDisplayer ; 
        //the default increased amount 
        private const int increaseAmount = 10 ;
        private const int statsBootsModifer = 20 ; 

        [SerializeField]
        private CutsceneDirector _director;

        // Lecture to study 

        //private List<LectureSO> lectures = new List<LectureSO>();

        [Header("DONT ASSIGN ANYTHING")] 
        [SerializeField] 
        private LectureSO _lectureToStudy ; 

        private List<ProgressData> localSaveProgress = new List<ProgressData>();

        private List<LectureSubjectRuntime> lectureRuntimes = new List<LectureSubjectRuntime>();

        //private void Start()
        //{
        //    InitLectureRuntime();
        //    OnPerformAcivity();
        //}

        [ContextMenu("Init Runtime")]
        public void InitLectureRuntime()
        {
            _lectureToStudy = (PersistentSceneLoader.Instance.ExtractSavedData<LectureSO>(PersistentSceneLoader.Instance.GetStackLoadedDataScene().GetSceneID())).GetData();

            if (_lectureToStudy == null)
                throw new Exception("lectureToStudy is null") ;


            if (ContainLectureInLectureRuntime(_lectureToStudy))
                return; 

            bool lectureAdded = false; 

            foreach (ProgressData progressData in localSaveProgress)
            {
                if (progressData.lectureName == _lectureToStudy.GetLectureName)
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

        private int CalculateBonusEXPPoint(LectureSO calculatedBootsLecture)
        {
            int ret = 0 ; 
            foreach (LectureRequieBoost booster in calculatedBootsLecture.GetBooster)
            {
                //If booster condition doesn't match 
                if (PersistentPlayerPersonalDataManager.Instance.GetPersonalityTrait.GetStat(booster.GetTrait).GetCurrentexp() < (float)booster.RequireLevel)
                    continue;

                ret += statsBootsModifer; 

            }

            return ret ; 
        }
        private int CalculateReceivedEXPPoint(LectureSO calculatedBootsLecture)
        {
            //Potae add personality trait multiplier here
            int statsBootsModifer = CalculateBonusEXPPoint(calculatedBootsLecture);



            return increaseAmount + statsBootsModifer ;
        }

        private void IncreaseExp()
        {
            foreach (LectureSubjectRuntime LectureSubjectRuntime in lectureRuntimes)
            {
                if (LectureSubjectRuntime.LectureName == _lectureToStudy.GetLectureName)
                {
                    LectureSubjectRuntime.RecievePoint(CalculateReceivedEXPPoint(_lectureToStudy));  
                    LectureSubjectRuntime.CalculateRecievedEventReward();
                }
            }
        }

        //[ContextMenu("Print Debug Lecture")]
        //public void PrintDebugLecture()
        //{
        //    foreach (LectureSubjectRuntime LectureSubjectRuntime in lectureRuntimes)
        //    {
        //        LectureSubjectRuntime.PrintDebug();
        //    }
        //}

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

        #region IAwakeable Methods
        IEnumerator ISceneLoaderWaitForSignal.OnNotifySceneLoadingComplete() 
        {
            InitLectureRuntime();
            OnPerformAcivity();

            yield return null; 
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

            yield return PostPerformActivity();
        }

        public IEnumerator PostPerformActivity()
        {
            SubmitActionReward(); 

            foreach (LectureSubjectRuntime lectureRuntime in lectureRuntimes)
            {

                if (lectureRuntime.LectureName != _lectureToStudy.GetLectureName) 
                    continue;

                var reward = new LectureRewardStruct()
                {
                    maxEXP = lectureRuntime.MaxPoint,
                    currentEXP = lectureRuntime.CurrentPoint,
                    gainedEXP = CalculateReceivedEXPPoint(_lectureToStudy),
                    bonusEXP =  CalculateBonusEXPPoint(_lectureToStudy), 
                    allRewardData = lectureRuntime.GetEventReward(),
                    alreadyReceivedRewardData = lectureRuntime.GetAlreadyRecievedReward(), 
                    checkpoints = lectureRuntime.GetCheckPoints , 
                    boostLecture = _lectureToStudy.GetBooster 
                };
                ColorfulLogger.LogWithColor("gained EXP is " + reward.gainedEXP, Color.blue) ;
                ColorfulLogger.LogWithColor("bonus EXP is " + reward.bonusEXP, Color.blue);

                yield return (_rewardDisplayer.DisplayRewardUICoroutine(reward));


            }

            PersistentActiveDayDatabase.Instance.OnPostPerformSchoolAction();

            yield return null; 
        }

        
        #endregion

        private bool ContainLectureInLectureRuntime(LectureSO conernedLecture)
        {
            foreach (LectureSubjectRuntime lectureRuntime in lectureRuntimes)
            {
                if (lectureRuntime.LectureName == conernedLecture.GetLectureName)
                    return true;
            }

            return false;
        }

        public void SubmitActionReward()
        {
            IncreaseExp();
        }
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

        public List<LectureChechpoint> GetCheckPoints
        {
            get
            {
                return checkpoint;
            }
        }
        public LectureSubjectRuntime()
        {

        }

        public LectureSubjectRuntime(LectureSO lectureSO)
        {
            lectureName = lectureSO.GetLectureName;
            currentPoint = 0;
            maxPoint = lectureSO.maxPoint;
            checkpoint = lectureSO.Checkpoint;
        }
        public LectureSubjectRuntime(LectureSO lectureSO, LectureParticipationScheme.ProgressData currentProgress)
        {
            lectureName = lectureSO.GetLectureName;
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

        public void CalculateRecievedEventReward()
        {
            foreach (LectureChechpoint lectureChechpoint in checkpoint)
            {
                if (lectureChechpoint.Received == true)
                    continue;

                if (currentPoint >= lectureChechpoint.RequirePoint)
                {
                    lectureChechpoint.ReceiveReward();
                }
            }
        }

        public List<EventReward> GetAlreadyRecievedReward()
        {
            List<EventReward> ret = new List<EventReward>();

            foreach (LectureChechpoint lectureChechpoint in checkpoint)
            {
                if (lectureChechpoint.Received == true)
                    ret.Add(lectureChechpoint.Reward);

            }
             
            return ret;
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

        public void PrintDebug()
        {
            Debug.Log(lectureName + ", " + currentPoint + ", " + maxPoint);
        }

        #region Getter
        public string LectureName => lectureName;
        public int CurrentPoint => currentPoint;
        public int MaxPoint => maxPoint;
        #endregion
    }
}
