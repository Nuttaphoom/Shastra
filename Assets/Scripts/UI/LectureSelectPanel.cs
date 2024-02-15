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
        [SerializeField] private List<TextMeshProUGUI> lectureDesList = new List<TextMeshProUGUI>();
        [SerializeField] private List<Image> lectureIconList = new List<Image>();
        [SerializeField] private TextMeshProUGUI pageText;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject prevButton;
        [SerializeField] private GameObject gfx;
        private int pageCount = 1;
        private int maxPage = 1;
        private LectureParticipationActionCommand action;
        private List<LectureParticipationActionCommand.ParticpationLectureData> availableLectures;
        public void InitPanel(List<LectureParticipationActionCommand.ParticpationLectureData> availableLectures, LectureParticipationActionCommand action)
        {
            this.action = action;
            this.availableLectures = availableLectures;
            gameObject.SetActive(true);

            double count = Math.Ceiling(availableLectures.Count / 3.0f);
            pageText.text = "Page 1/" + count;
            maxPage = (int)count;

            CalculatePageButton();
            GenerateLectureButton();
        }
        public void NextPage()
        {
            if (pageCount < maxPage)
            {
                pageCount++;
                pageText.text = "Page " + pageCount + "/" + maxPage;
                CalculatePageButton();
            }
            GenerateLectureButton();
        }

        public void PreviousPage()
        {
            if (pageCount > 1)
            {
                pageCount--;
                pageText.text = "Page " + pageCount + "/" + maxPage;
                CalculatePageButton();
            }
            GenerateLectureButton();
        }

        private void GenerateLectureButton()
        {
            foreach (Button button in lectureButtonList)
            {
                button.gameObject.SetActive(false);
                button.onClick.RemoveAllListeners();
            }
            int buttonIndex = 0;
            for (int i = (pageCount * 3) - 3; i < (pageCount * 3); i++)
            {
                if (i < availableLectures.Count)
                {
                    int currentIndex = i;
                    lectureButtonList[buttonIndex].onClick.AddListener(() => action.OnSelectLecture(availableLectures[currentIndex]));
                    lectureButtonList[buttonIndex].gameObject.SetActive(true);
                    lectureNameList[buttonIndex].text = availableLectures[i].GetAvailableLecture.GetLectureName;
                    lectureDesList[buttonIndex].text = availableLectures[i].GetAvailableLecture.GetLectureDestcription;
                    lectureIconList[buttonIndex].sprite = availableLectures[i].GetAvailableLecture.GetLectureLogoIcon;
                }
                buttonIndex++;
            }
        }

        private void CalculatePageButton()
        {
            prevButton.SetActive(false);
            nextButton.SetActive(false);
            if (pageCount == 1)
            {
                prevButton.SetActive(false);
            }
            else
            {
                prevButton.SetActive(true);
            }
            if (pageCount == maxPage)
            {
                nextButton.SetActive(false);
            }
            else
            {
                nextButton.SetActive(true);
            }
        }

        public void OpenPanel()
        {
            gfx.SetActive(true);
        }
    }
}
