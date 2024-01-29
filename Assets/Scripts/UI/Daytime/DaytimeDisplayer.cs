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
        private TextMeshProUGUI dayTimeText;
        [SerializeField]
        private GameObject panel;

        private int dayCount = 0;
        private int weekDay = 0; //max 6 mon,tue, etc.
        private int month = 0; // max 11
        private int date = 1; //max 29

        private string[] months =
        {
            "January", "February", "March", "April",
            "May", "June", "July", "August",
            "September", "October", "November", "December"
        };

        private string[] days =
        {
            "Monday", "Tuesday", "Wednesday", "Thursday",
            "Friday", "Saturday", "Sunday"
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
            if (dayCount > 29)
            {
                dayCount = 0;
                month++;
            }
            if (month > 11)
            {
                month = 0;
            }
            InitPanel();
            //Debug.Log("Date: " + date + ", Month: " + months[month] + ", Day: " + days[weekDay]);
        }

        private void Awake()
        {
            InitPanel();
            Debug.Log("Date: " + date + ", Month: " + months[month] + ", Day: " + days[weekDay]);
        }

        private void InitPanel()
        {
            dateText.text = date + "/" + months[month] + " " + days[weekDay];
            dayTimeText.text = dayTime[PersistentActiveDayDatabase.Instance.GetDayProgressionHandler.GetCurrentDayTime()].ToString();
        }

        public void OpenPanel(){
            panel.SetActive(true);
        }

        public void ClosePanel(){
            panel.SetActive(false);
        }

        
    }
}
