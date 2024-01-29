using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Vanaring
{
    public class LectureManager : MonoBehaviour, ISaveable
    {
        // For testing only
        [SerializeField]
        private int increaseAmount = 100;

        // Use this for testing now
        [SerializeField]
        private List<LectureSO> lectures = new List<LectureSO>();

        private List<ProgressData> localSaveProgress = new List<ProgressData>();

        static private List<LectureSubjectRuntime> lectureRuntimes = new List<LectureSubjectRuntime>();

        private void Awake()
        {
            if (lectureRuntimes.Count > 0)
                return;

            InitLectureRuntime();
        }

        [ContextMenu("Load Scene")]
        public void LoadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("PotaeTesterScene");
        }

        [ContextMenu("Init Runtime")]
        public void InitLectureRuntime()
        {
            if(lectureRuntimes.Count > 0)
            {
                return;
            }

            foreach (LectureSO lectureSO in lectures)
            {
                bool lectureAdded = false;
                foreach (ProgressData progressData in localSaveProgress)
                {
                    if (progressData.lectureName == lectureSO.LectureName)
                    {
                        lectureRuntimes.Add(new LectureSubjectRuntime(lectureSO, progressData));
                        lectureAdded = true;
                    }
                }
                if (!lectureAdded)
                {
                    lectureRuntimes.Add(new LectureSubjectRuntime(lectureSO));
                }
            }

        }

        public void IncreaseExp(string LectureName)
        {
            foreach (LectureSubjectRuntime LectureSubjectRuntime in lectureRuntimes)
            {
                if (LectureSubjectRuntime.LectureName == LectureName)
                {
                    LectureSubjectRuntime.RecievePoint(increaseAmount);
                    GetEventBroadcaster().InvokeEvent<int>(increaseAmount, "OnReceiveExp");
                    if (LectureSubjectRuntime.SetRecieveReward())
                    {
                        Debug.Log("real Get: " + LectureSubjectRuntime.GetRecievedReward().Count);
                        GetEventBroadcaster().InvokeEvent<List<string>>(LectureSubjectRuntime.GetRecievedReward(), "OnUnlockReward");
                    }
                }
            }
        }

        //[ContextMenu("IncreaseGAM")]
        //public void IncreaseGAM()
        //{
        //    IncreaseExp("GAM300");
        //}
        //[ContextMenu("IncreaseLOL")]
        //public void IncreaseLOL()
        //{
        //    IncreaseExp("LOL800");
        //}

        //[ContextMenu("Print Runtime")]
        //public void PrintLectureRuntime()
        //{
        //    foreach (LectureSubjectRuntime LectureSubjectRuntime in lectureRuntimes)
        //    {
        //        LectureSubjectRuntime.PrintData();
        //    }
        //}

        #region GetEventBroadcaster Methods 

        private EventBroadcaster _eventBroadcaster;
        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();

                _eventBroadcaster.OpenChannel<int>("OnReceiveExp");
                _eventBroadcaster.OpenChannel<List<string>>("OnUnlockReward");
            }

            return _eventBroadcaster;
        }
        public void SubOnReceiveExpEvent(UnityAction<int> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnReceiveExp");
        }
        public void SubOnUnlockRewardEvent(UnityAction<List<string>> argc)
        {
            GetEventBroadcaster().SubEvent(argc, "OnUnlockReward");
        }
        public void UnSubOnReceiveExpEvent(UnityAction<int> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnReceiveExp");
        }
        public void UnSubOnUnlockRewardEvent(UnityAction<List<string>> argc)
        {
            GetEventBroadcaster().UnSubEvent(argc, "OnUnlockReward");
        }

        #endregion

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

        private List<string> recievedReward = new List<string>();

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

        public List<string> GetRecievedReward()
        {
            List<string> temp = new List<string>();
            foreach (string rew in recievedReward)
            {
                temp.Add(rew);
            }
            recievedReward.Clear();
            return temp;
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
