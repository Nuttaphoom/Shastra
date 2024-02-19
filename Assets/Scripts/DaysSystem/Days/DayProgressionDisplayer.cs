using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Vanaring 
{
    public struct DayProgressionData
    {
        //int prevDay;
        //int currentDay; 
    }

    [Serializable]
    public class DayProgressionDisplayer : BaseRewardDisplayer<DayProgressionData, DayProgressionDisplayerPanel>
    {
        private DayProgressionData _dayProgressionData; 
        public override IEnumerator DisplayRewardUICoroutine(DayProgressionData data)
        {
            _dayProgressionData  = data;
            yield return CreateRewardDisplayPanel();


        }

        protected override IEnumerator SettingUpRewardDisplayPanel(DayProgressionDisplayerPanel rewardDisplayGOTemplate)
        {
            rewardDisplayGOTemplate.ReceiveDayProgressionDetail(_dayProgressionData);
            yield return null;
        }
    }


}
