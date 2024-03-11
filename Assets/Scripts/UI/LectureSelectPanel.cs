using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Playables;
using Unity.VisualScripting;

namespace Vanaring
{
    public class LectureSelectPanel : MonoBehaviour
    {
        [SerializeField] private List<Button> lectureButtonList = new List<Button>();
        [SerializeField] private List<TextMeshProUGUI> lectureNameList = new List<TextMeshProUGUI>();
        [SerializeField] private List<TextMeshProUGUI> lectureDesList = new List<TextMeshProUGUI>();
        [SerializeField] private List<GameObject> lectureObjList = new List<GameObject>();
        [SerializeField] private List<Image> lectureIconList = new List<Image>();
        [SerializeField] private TextMeshProUGUI pageText;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private GameObject prevButton;
        [SerializeField] private GameObject gfx;
        [SerializeField] private PlayableDirector inDirector;
        [SerializeField] private PlayableDirector outDirector;
        private GameObject confirmPanel;
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
            foreach (GameObject ui in lectureObjList)
            {
                ui.SetActive(false);
            }
            int buttonIndex = 0;
            for (int i = (pageCount * 3) - 3; i < (pageCount * 3); i++)
            {
                if (i < availableLectures.Count)
                {
                    int currentIndex = i;
                    //lectureButtonList[buttonIndex].onClick.AddListener(() => action.OnSelectLecture(availableLectures[currentIndex]));
                    lectureButtonList[buttonIndex].onClick.AddListener(() => PerformAction(action, currentIndex));
                    lectureButtonList[buttonIndex].gameObject.SetActive(true);
                    lectureObjList[buttonIndex].SetActive(true);
                    lectureNameList[buttonIndex].text = availableLectures[i].GetAvailableLecture.GetLectureName;
                    lectureDesList[buttonIndex].text = availableLectures[i].GetAvailableLecture.GetLectureDestcription;
                    lectureIconList[buttonIndex].sprite = availableLectures[i].GetAvailableLecture.GetLectureLogoIcon;
                }
                buttonIndex++;
            }
        }

        private void PerformAction(LectureParticipationActionCommand action, int index)
        {
            if (confirmPanel == null)
            {
                confirmPanel = MonoBehaviour.Instantiate(PersistentAddressableResourceLoader.Instance.LoadResourceOperation<GameObject>("ConfirmationPanel"));
            }
            confirmPanel.GetComponent<LocationConfirmationPanel>().GFX.SetActive(true);
            string ct = string.Format("This action will consume <color=#FF0000FF>1 time slot</color>, are you sure to perform this action ? ");
            confirmPanel.GetComponent<LocationConfirmationPanel>().SetButtonListerner(() => StartCoroutine(OnConfirmMenu(availableLectures[index])));

            confirmPanel.GetComponent<LocationConfirmationPanel>().WarningText.text = ct;
        }

        private IEnumerator OnConfirmMenu(LectureParticipationActionCommand.ParticpationLectureData lecture)
        {
            confirmPanel.gameObject.SetActive(false);
            yield return CloseLecturePanel();
            yield return null; 
            action.OnSelectLecture(lecture); 
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
            outDirector.Stop();
            inDirector.Play();
        }

        public void ClosePanel()
        {
            gameObject.SetActive(true);
            StartCoroutine(CloseLecturePanel());
        }

        private IEnumerator CloseLecturePanel()
        {
            inDirector.Stop();
            outDirector.Play();
            while(outDirector.state == PlayState.Playing)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
