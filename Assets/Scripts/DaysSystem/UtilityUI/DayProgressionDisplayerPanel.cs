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
        
        public void ReceiveDayProgressionDetail(DayProgressionData data)
        {
            progressionData = data;
            dayText.text = "Day " + "<color=#FEFF94>" + (progressionData.PreviousDay + 1).ToString() + "</color>";
            StartCoroutine(AutoRunDisplayerScheme()); 
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
            //Debug.Log("Prev day" + progressionData.PreviousDay.ToString());
            //Debug.Log("Next day" + progressionData.NextDay.ToString());
            
            yield return null; 
        }

        private void EndProgressionDisplay()
        {
            _uiAnimationDone = true;
            _displayingUIDone = true;
        }

        public void SetupDayText()
        {
            dayText.text = "Day " + "<color=#FEFF94>" + (progressionData.NextDay + 1).ToString() + "</color>";
        }
    }
}
