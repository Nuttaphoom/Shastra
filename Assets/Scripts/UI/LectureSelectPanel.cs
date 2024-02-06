using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Vanaring
{
    public class LectureSelectPanel : MonoBehaviour
    {
        [SerializeField] private List<Button> lectureButtonList = new List<Button>();
        [SerializeField] private List<TextMeshProUGUI> lectureNameList = new List<TextMeshProUGUI>();
        [SerializeField] private List<Image> lectureIconList = new List<Image>();
        [SerializeField] private TextMeshProUGUI pageText;
        private int pageCount = 1;
        private LectureParticipationActionCommand action;
        private List<LectureParticipationActionCommand.ParticpationLectureData> availableLectures;
        public void InitPanel(List<LectureParticipationActionCommand.ParticpationLectureData> availableLectures, LectureParticipationActionCommand action)
        {
            this.action = action;
            this.availableLectures = availableLectures;
            gameObject.SetActive(true);

            double count = Math.Ceiling(availableLectures.Count / 3.0f);
            pageText.text = "Page 1/" + count;
            pageCount = (int)count;

            foreach (Button button in lectureButtonList)
            {
                button.gameObject.SetActive(false);
                button.onClick.RemoveAllListeners();
            }
            Debug.Log(availableLectures.Count);
            for (int i = 0; i < availableLectures.Count; i++)
            {
                int currentIndex = i;
                lectureButtonList[i].onClick.AddListener(() => action.OnSelectLecture(availableLectures[currentIndex]));
                lectureButtonList[i].gameObject.SetActive(true);
                lectureNameList[i].text = availableLectures[i].GetAvailableLecture.GetLectureName;
            }
            
        }
        public void NextPage()
        {
            pageCount++;
            foreach (Button button in lectureButtonList)
            {
                button.gameObject.SetActive(false);
                button.onClick.RemoveAllListeners();
            }
            for (int i = (pageCount * 3) - 3; i < (pageCount * 3); i++)
            {
                if(i < availableLectures.Count)
                {
                    int currentIndex = i;
                    lectureButtonList[i].onClick.AddListener(() => action.OnSelectLecture(availableLectures[currentIndex]));
                    lectureButtonList[i].gameObject.SetActive(true);
                    lectureNameList[i].text = availableLectures[i].GetAvailableLecture.GetLectureName;
                }
            }
        }

        public void PreviousPage()
        {
            pageCount--;
            foreach (Button button in lectureButtonList)
            {
                button.gameObject.SetActive(false);
                button.onClick.RemoveAllListeners();
            }
            for (int i = (pageCount * 3) - 3; i < (pageCount * 3); i++)
            {
                if (i < availableLectures.Count)
                {
                    int currentIndex = i;
                    lectureButtonList[i].onClick.AddListener(() => action.OnSelectLecture(availableLectures[currentIndex]));
                    lectureButtonList[i].gameObject.SetActive(true);
                    lectureNameList[i].text = availableLectures[i].GetAvailableLecture.GetLectureName;
                }
            }
        }
    }
}
