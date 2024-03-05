using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Vanaring
{
    [Serializable]
    public class PersonalityTraitUEXPSystem : BaseUEXPSystem
    {
        public PersonalityTraitUEXPSystem() : base() { 
            
        }

        public PersonalityTraitUEXPSystem(int currentLevel , float currentEXP) :  base()
        {
            _currentEXP = currentEXP;
            _currentLevel = currentLevel; 
        }
        
        /// <summary>
        /// Formula is "4 + _currentLevel"
        /// </summary>
        /// <returns></returns>
        public override float GetEXPCap()
        { 
            return  (float) 10 * _currentLevel ;
        }

        [Serializable]
        private struct SaveData
        {
            public int savedLevel;
            public float savedEXP;
        }

        public object CaptureState()
        {
            return new SaveData{
                savedLevel = this._currentLevel,
                savedEXP = this._currentEXP
            };
        }

        public void RestoreState(object state)
        {
            SaveData saveData = (SaveData)state;

            _currentLevel = saveData.savedLevel;
            _currentEXP = saveData.savedEXP;
        }


    }
}
