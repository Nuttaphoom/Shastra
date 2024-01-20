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

        private List<LectureSubjectRuntime> lectureRuntimes = new List<LectureSubjectRuntime>();

        [ContextMenu("Init Runtime")]
        public void initLectureRuntime()
        {
            foreach (LectureSO lectureSO in lectures)
            {
                // TO DO: prevent multiple ADD
                lectureRuntimes.Add(new LectureSubjectRuntime(lectureSO));
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
            return new SaveData
            {
                savelectureRuntimes = lectureRuntimes
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            lectureRuntimes = saveData.savelectureRuntimes;
        }

        [Serializable]
        private struct SaveData
        {
            public List<LectureSubjectRuntime> savelectureRuntimes;
        }
    }

    public class LectureSubjectRuntime
    {
        private string lectureName = "";
        private int currentPoint = -1;
        private int maxPoint = -1;
        private List<LectureChechpoint> checkpoint = new List<LectureChechpoint>();

        public LectureSubjectRuntime(LectureSO lectureSO, int currentProgress = 0)
        {
            lectureName = lectureSO.LectureName;
            currentPoint = currentProgress;
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
