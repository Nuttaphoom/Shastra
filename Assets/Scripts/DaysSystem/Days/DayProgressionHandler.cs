using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public class DayProgressionHandler
    {
        private DailyActionParticipationHandler _dailyActionParticipationHandler;

        public void NewDayBegin()
        {
            if (_dailyActionParticipationHandler == null)
                _dailyActionParticipationHandler = new DailyActionParticipationHandler();  

            _dailyActionParticipationHandler.ResetDayAction(); 
        }

        public void OnPostPerformSchoolAction()
        {
            _dailyActionParticipationHandler.DecreaseActionPoint(1);

            ///No remaining action point => start new day 
            if (_dailyActionParticipationHandler.IsOutOfActionPoint())
                throw new NotImplementedException( "new day begin function won't work yet" ) ;

            if (true)
            {
                PersistentSceneLoader.Instance.LoadLastVisitedLocation(); 
            }

        }

        private void GetBackToLastVisitedLocationScene()
        {
            
        }

        public int GetCurrentDayTime()
        {
            return _dailyActionParticipationHandler.GetMaxActionPoint() - _dailyActionParticipationHandler.GetRemaiingActionPoint(); 
        }

        

        
        

       
    }
}
