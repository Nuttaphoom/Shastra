using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Vanaring_DepaDemo
{
    //Store a behavior socket that contain multiople behaviorSO
    class BotBehaviorHandler : MonoBehaviour
    {
        [SerializeField]
        private BotBehaviorSocketSO _behaviorSocketSOs;

        int _nextBehavior = 0;
        int _currentBehavior = 0;

        public IEnumerator CalculateNextBehavior()
        {
            yield return null;
            _nextBehavior = Random.Range(0, _behaviorSocketSOs.GetBehaviorSize);
        }

        public List<RuntimeEffectFactorySO> GetBehaviorEffect()
        {
            _currentBehavior = _nextBehavior;
            List<RuntimeEffectFactorySO> ret = new List<RuntimeEffectFactorySO>();
            IEnumerator coroutine = _behaviorSocketSOs.GetBehaviorEffect(_currentBehavior);

            while (coroutine.MoveNext())
            {
                if (coroutine.Current != null && coroutine.Current.GetType().IsSubclassOf(typeof(RuntimeEffectFactorySO)))
                {
                    ret.Add(coroutine.Current as RuntimeEffectFactorySO); 
                }
            }
            return ret;
        }
        //TODO - TEMP 
        public int GetCurrentBehaviorIndex => _nextBehavior;
    }
}