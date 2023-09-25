using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace  Vanaring  {
    [CreateAssetMenu(fileName = "BotBehaviorSO", menuName = "ScriptableObject/BotBehavior/BotBehaviorSO")]
    //Each behavior contain "action" aka RuntimeEffectFacotorySO
    class BotBehaviorSO : ScriptableObject
    {
        [SerializeField]
        private string actionname;

        [SerializeField]
        private List<RuntimeEffectFactorySO> _factories;

        [SerializeField]
        [Header("This variable is used for tepegrahy only : ")]
        private EnergyModifierData _actionEnergyCost ;

        [SerializeField]
        public ActorActionFactory action;


        public ActorActionFactory GetAction()
        {
            return action; 
        }

        #region GETTER 
        public EnergyModifierData ActionEnergyCost => _actionEnergyCost ; 

        #endregion
    }
}