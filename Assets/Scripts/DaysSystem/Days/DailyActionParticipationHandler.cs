﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine; 

namespace Vanaring 
{
    [Serializable]
    public class DailyActionParticipationHandler
    {
        //[SerializeField]
        //[Header("Max Action Point should be copied and used throughout the whole application life time")]
        private int _maxActionPoint = 3;

        private int _remainingAction; 
        public DailyActionParticipationHandler() {
            _remainingAction = _maxActionPoint; 
        } 

        public void DecreaseActionPoint(int point) {  
            _remainingAction -= point;

            if (_remainingAction < 0)
                _remainingAction = 0; 
        }

        public void IncreaseActionPoint(int point)
        {
            _remainingAction = (_remainingAction + point) % (_maxActionPoint+1) ;  
        }
        
        public bool IsOutOfActionPoint()
        {
            return _remainingAction <= 0; 
        }
    }
}