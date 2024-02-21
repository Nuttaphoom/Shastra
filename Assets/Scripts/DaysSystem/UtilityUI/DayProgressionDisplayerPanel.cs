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

            StartCoroutine(AutoRunDisplayerScheme()); 
        }

        public override IEnumerator AutoRunDisplayerScheme()
        {
            yield return SettingUpNumber();
            yield return _directorManager.PlayCutscene();
            EndProgressionDisplay();
        }
        public override IEnumerator SettingUpNumber()
        {
            dayText.text = "Test";
            //Set up number here 
            yield return null; 
        }

        private void EndProgressionDisplay()
        {
            _uiAnimationDone = true;
            _displayingUIDone = true;
        }
    }
}
