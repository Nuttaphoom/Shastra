using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    [CreateAssetMenu(fileName = "SemesterDataSO", menuName = "ScriptableObject/DaySystem/SemesterDataSO")]
    public class SemesterDataSO : ScriptableObject
    {
        [SerializeField]
        private List<DayDataSO> _dayDataSOO ;

        public DayDataSO  GetDayData(int date)
        {
            return _dayDataSOO[date]; 
        }

         
    }
}
