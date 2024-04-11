using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using static Cinemachine.DocumentationSortingAttribute;

using UnityEngine;
using Vanaring.Assets.Scripts.Utilities.StringConstant;

namespace Vanaring
{
    [Serializable]
    public class CharacterUEXPSystem : BaseUEXPSystem
    {
        public CharacterUEXPSystem() : base()
        {

        }

        public CharacterUEXPSystem(int currentLevel, float currentEXP) : base()
        {
            _currentEXP = currentEXP;
            _currentLevel = currentLevel;
        }


        public override float GetEXPCap()
        {
            return (float)(75 * (Math.Pow(_currentLevel, 2))) - (75 * _currentLevel)+150; 
        }

        [Serializable]
        private struct SaveData
        {
            public int savedLevel;
            public float savedEXP;
        }

        public object CaptureState()
        {
            return new SaveData
            {
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
