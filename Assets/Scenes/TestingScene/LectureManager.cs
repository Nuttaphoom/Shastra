using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        private List<ProgressData> tempSaveProgress = new List<ProgressData>();

        private List<LectureSubjectRuntime> lectureRuntimes = new List<LectureSubjectRuntime>();

        private void Awake()
        {
            //if (lectureRuntimes.Count > 0)
            //{
            //    foreach (LectureSO lectureSO in lectures)
            //    {
            //        LectureSubjectRuntime lectureSubjectRuntime = gameObject.AddComponent<LectureSubjectRuntime>();
            //        lectureSubjectRuntime.InitData(lectureSO);
            //        lectureRuntimes.Add(lectureSubjectRuntime);
            //    }
            //}
        }

        [ContextMenu("Load Scene")]
        public void LoadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("PotaeTesterScene");
        }

        [ContextMenu("Init Runtime")]
        public void initLectureRuntime()
        {
            if(lectureRuntimes.Count > 0)
            {
                // TO DO: prevent multiple ADD
                return;
            }

            for (int i = 0; i < lectures.Count; i++)
            {
                // TO DO: prevent multiple ADD
                if (tempSaveProgress.Count > 0)
                {
                    lectureRuntimes.Add(new LectureSubjectRuntime(lectures[i], tempSaveProgress[i]));
                }
                else 
                {
                    lectureRuntimes.Add(new LectureSubjectRuntime(lectures[i]));
                }
            }

        }

        [ContextMenu("IncreaseExpGAM")]
        public void IncreaseExpGAM()
        {
            IncreaseExp("GAM300");
        }
        [ContextMenu("IncreaseExpLOL")]
        public void IncreaseExpLOL()
        {
            IncreaseExp("LOL800");
        }

        public void IncreaseExp(string LectureName)
        {
            foreach (LectureSubjectRuntime LectureSubjectRuntime in lectureRuntimes)
            {
                if (LectureSubjectRuntime.LectureName == LectureName)
                {
                    LectureSubjectRuntime.RecievePoint(increaseAmount);
                }
            }
        }

        [ContextMenu("Print Runtime")]
        public void PrintLectureRuntime()
        {
            foreach (LectureSubjectRuntime LectureSubjectRuntime in lectureRuntimes)
            {
                LectureSubjectRuntime.PrintData();
            }
        }

        public object CaptureState()
        {
            List<ProgressData> tempProgress = new List<ProgressData>();
            foreach (LectureSubjectRuntime LectureSubjectRuntime in lectureRuntimes)
            {
                ProgressData progressData;

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

            tempSaveProgress = saveData.savedProgress;
        }

        [Serializable]
        public struct ProgressData
        {
            public int progessPoint;
        }

        [Serializable]
        private struct SaveData
        {
            public List<ProgressData> savedProgress;
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

            PrintReward();
        }

        public void PrintReward()
        {
            foreach (LectureChechpoint lectureChechpoint in checkpoint)
            {
                if (lectureChechpoint.Received == true)
                {
                    continue;
                }
                else if (currentPoint >= lectureChechpoint.RequirePoint)
                {
                    lectureChechpoint.SetReceived(true);
                    Debug.Log("Reward : " + lectureChechpoint.Reward);
                }
            }
        }

        public void PrintData()
        {
            Debug.Log(LectureName + "," + currentPoint + "," + maxPoint);
        }

        #region Getter
        public string LectureName => lectureName;
        public int CurrentPoint => currentPoint;
        public int MaxPoint => maxPoint;
        #endregion
    }
}
