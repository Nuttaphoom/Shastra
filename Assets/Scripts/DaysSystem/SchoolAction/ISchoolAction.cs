using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring.Assets.Scripts.DaysSystem.SchoolAction
{

    public interface ISchoolAction 
    {
        public void OnPerformAcivity(); 
        
        //Submit reward and than display the reward
        public void PostPerformActivity();

        //Called in the PostPerformActivity 
        public void SubmitActionReward(); 
    }
}
