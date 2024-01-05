using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring.Assets.Scripts.DaysSystem.SchoolAction
{

    public interface ISchoolAction 
    {
        public void PrePerformActivity();
        public void OnPerformAcivity(); 
        public void PostPerformActivity(); 
    }
}
