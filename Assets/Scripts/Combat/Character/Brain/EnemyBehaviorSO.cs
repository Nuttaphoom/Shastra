using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace  Vanaring  {
    [CreateAssetMenu(fileName = "BotBehaviorSO", menuName = "ScriptableObject/BotBehavior/BotBehaviorSO")]
    //Each behavior contain "action" aka RuntimeEffectFacotorySO
    class EnemyBehaviorSO : ScriptableObject
    {
        [SerializeField]
        private string actionname;

        [SerializeField]
        private List<RuntimeEffectFactorySO> _factories;

        [SerializeField]
        [Header("This variable is used for tepegrahy only : ")]
        private EnergyModifierData _actionEnergyCost ;

        [Serializable]
        public struct ActionData
        {
            public List<RuntimeEffectFactorySO> Effects; 
            public EnergyModifierData EnergyCost;
            
        }

        public IEnumerator GetBehaviorEffect()
        {
            ActionData actionData = new ActionData();
            actionData.Effects = new List<RuntimeEffectFactorySO>();
            actionData.EnergyCost = _actionEnergyCost; 
            foreach (RuntimeEffectFactorySO factory in _factories)
            {
                actionData.Effects.Add(factory);
            }

            yield return actionData; 
        }

        #region GETTER 
        public EnergyModifierData ActionEnergyCost => _actionEnergyCost ; 

        #endregion
    }
}