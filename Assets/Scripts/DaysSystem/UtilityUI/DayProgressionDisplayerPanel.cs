using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Vanaring
{
    public class DayProgressionDisplayerPanel : BaseRewardDisplayerPanel
    {
        DayProgressionData progressionData = new DayProgressionData();
        public void ReceiveDayProgressionDetail(DayProgressionData data)
        {
            progressionData = data; 
        }
        public override void ForceSetUpNumber()
        {
            throw new NotImplementedException();
        }

        public override void OnContinueButtonClick()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator SettingUpNumber()
        {
            throw new NotImplementedException();
        }
    }
}
