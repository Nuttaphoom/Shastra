using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;



namespace  Vanaring_DepaDemo {
    [CreateAssetMenu(fileName = "BotBehaviorSO", menuName = "ScriptableObject/BotBehavior/BotBehaviorSO")]
    //Each behavior contain "action" aka RuntimeEffectFacotorySO
    class BotBehaviorSO : ScriptableObject
    {
        [SerializeField]
        private string actionname;

        [SerializeField]
        private List<RuntimeEffectFactorySO> _factories;

        public IEnumerator GetBehaviorEffect()
        {
            foreach (RuntimeEffectFactorySO factory in _factories)
            {
                yield return factory as RuntimeEffectFactorySO  ;
                 
            }
        }

    }
}