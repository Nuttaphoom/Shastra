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
        enum EteleIndicator
        {
            Light_low = 0,
            Light_mid,
            Light_high,
            Dark_low,
            Dark_mid,
            Dark_high
        }

        [SerializeField]
        private string actionname;

        [SerializeField]
        EteleIndicator indicator;

        [SerializeField]
        private List<RuntimeEffectFactorySO> _factories;

        public int GetIndicatorIndex() {
            return ((int)indicator);
        }
        public IEnumerator GetBehaviorEffect()
        {
            foreach (RuntimeEffectFactorySO factory in _factories)
            {
                yield return factory as RuntimeEffectFactorySO  ;
            }
        }

    }
}