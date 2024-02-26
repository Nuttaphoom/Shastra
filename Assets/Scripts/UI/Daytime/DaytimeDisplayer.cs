using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Vanaring
{
    public class DaytimeDisplayer : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI dateText;
        [SerializeField]
        private TextMeshProUGUI weekdayText;
        [SerializeField]
        private TextMeshProUGUI dayTimeText;
        [SerializeField]
        private GameObject panel;

        [SerializeField]
        private Color[] weekColor;

        private int dayCount = 0;
        private int weekDay = 0; //max 6 mon,tue, etc.
        private int month = 0; // max 11
        private int date = 1; //max 29
        private int time = 0;

        private string[] months =
        {
            "January", "February", "March", "April",
            "May", "June", "July", "August",
            "September", "October", "November", "December"
        };

        private string[] days =
        {
            "Mon", "Tue", "Wed", "Thu",
            "Fri", "Sat", "Sun"
        };

        private string[] dayTime =
        {
            "Morning", "Noon", "Evening"
        };

        [ContextMenu("NextDay")]
        public void AdvanceDay()
        {
            date++;
            weekDay++;
            if (weekDay > 6)
            {
                weekDay = 0;
            }
            if (date > 29)
            {
                date = 0;
                month++;
            }
            if (month > 11)
            {
                month = 0;
            }
            InitPanel();
            //Debug.Log("Date: " + date + ", Month: " + months[month] + ", Day: " + days[weekDay]);
        }
        public void AdvanceDayNight()
        {
            time++;
        }

        private void Awake()
        {
            InitPanel();
            
            //Debug.Log(PersistentActiveDayDatabase.Instance.GetDayProgressionHandler.GetCurrentDayTime());
        }

        [ContextMenu("ShowPanelDetail")]
        private void InitPanel()
        {
            dateText.text = date + "/" + (month + 1);
            weekdayText.text = days[weekDay];
            weekdayText.color = weekColor[weekDay];
            dayTimeText.text = dayTime[(int)PersistentActiveDayDatabase.Instance.GetDayProgressionHandler.GetCurrentDayTime()].ToString();
            //Debug.Log("Date: " + date + ", Month: " + month+1 + ", Day: " + days[weekDay]);
        }

        public void OpenPanel(){
            panel.SetActive(true);
        }

        public void ClosePanel(){
            panel.SetActive(false);
        }

        
    }
}
