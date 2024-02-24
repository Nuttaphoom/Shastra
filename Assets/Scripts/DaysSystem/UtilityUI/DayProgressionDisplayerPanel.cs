using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables; 
using UnityEngine.Timeline;
using TMPro;

namespace Vanaring
{
    public class DayProgressionDisplayerPanel : BaseRewardDisplayerPanel
    {
        DayProgressionData progressionData = new DayProgressionData();

        [SerializeField]
        CutsceneDirector _directorManager;

        [SerializeField]
        private TextMeshProUGUI dayText;

        private string[] weekDays =
        {
            "Monday", "Tueday", "Wednesday", "Thursday",
            "Friday", "Saturday", "Sunday"
        };

        public void ReceiveDayProgressionDetail(DayProgressionData data)
        {
            progressionData = data;
            
            dayText.text = weekDays[progressionData.PreviousDay % 6] + " <color=#FEFF94>" + GetOrdinalNumText((progressionData.PreviousDay + 1).ToString()) + "</color>";
            StartCoroutine(AutoRunDisplayerScheme()); 
        }

        private string GetOrdinalNumText(string dayCount)
        {
            string ordinalNumText = "";
            if (!string.IsNullOrEmpty(dayCount))
            {
                char lastChar = dayCount[dayCount.Length - 1];

                if (lastChar == '1')
                {
                    ordinalNumText = dayCount + "st";
                }
                else if (lastChar == '2')
                {
                    ordinalNumText = dayCount + "nd";
                }
                else if (lastChar == '3')
                {
                    ordinalNumText = dayCount + "rd";
                }
                else
                {
                    ordinalNumText = dayCount + "th";
                }
            }
            return ordinalNumText;
        }

        public override IEnumerator AutoRunDisplayerScheme()
        {
            yield return new WaitForSeconds(2.0f);
            yield return SettingUpNumber();
            yield return _directorManager.PlayCutscene();
            EndProgressionDisplay();
        }
        public override IEnumerator SettingUpNumber()
        {
            yield return null; 
        }

        private void EndProgressionDisplay()
        {
            _uiAnimationDone = true;
            _displayingUIDone = true;
        }

        public void SetupDayText()
        {
            dayText.text = weekDays[progressionData.NextDay % 6] + " <color=#FEFF94>" + GetOrdinalNumText((progressionData.NextDay + 1).ToString()) + "</color>";
        }
    }
}
