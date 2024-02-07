using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine; 
namespace Vanaring 
{
    public abstract class BaseUEXPSystem
    {
        [SerializeField] 
        protected int _currentLevel;

        [SerializeField]
        protected float _currentEXP;

        public BaseUEXPSystem()
        {
            _currentLevel = 0;
            _currentEXP = 0;
        }

        #region GETTER 

        public int GetCurrentLevel
        {
            get
            {
                return _currentLevel; 
            }
        }
        #endregion

        #region EventBroadcaster

        private EventBroadcaster _eventBroadcaster;

        private EventBroadcaster GetEventBroadcaster()
        {
            if (_eventBroadcaster == null)
            {
                _eventBroadcaster = new EventBroadcaster();
                _eventBroadcaster.OpenChannel<int>("OnLevelUp");

            }

            return _eventBroadcaster;
        }

        public void SubOnLevelUp(UnityAction<int> argc)
        {
            GetEventBroadcaster().SubEvent<int>(argc, "OnLevelUp");
        }

        public void UnSubOnLevelUp(UnityAction<int> argc)
        {
            GetEventBroadcaster().UnSubEvent<int>(argc, "OnLevelUp");
        }
         
        #endregion 

        public void ReceiveEXP(float receivedEXP)
        {
            float capEXP = GetEXPCap();
            _currentEXP += receivedEXP;
           
            if (_currentEXP > capEXP)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            _currentEXP = 0 ;
            _currentLevel += 1;

            GetEventBroadcaster().InvokeEvent<int>(_currentLevel, "OnLevelUp"); 
        }

        public abstract float GetEXPCap();

    }
}
