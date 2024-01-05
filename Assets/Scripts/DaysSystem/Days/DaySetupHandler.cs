using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vanaring.Assets.Scripts.DaysSystem.Days;
using Vanaring.Assets.Scripts.Utilities;

namespace Vanaring 
{
    public class DaySetupHandler: MonoBehaviour
    {
        [SerializeField]
        private SemesterDataSO _activeSemester;

        private int _currentDate = 0 ;
        
        private void Awake()
        {
            NewDayBegin(); 
        }
 
        public void NewDayBegin()
        {
            RuntimeDayData newDayData = new RuntimeDayData(_activeSemester.GetDayData(_currentDate)); 
            PersistentActiveDayDatabase.Instance.SetActiveDayData(newDayData) ; 
        }
    }
}
