using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace Vanaring_DepaDemo
{
    //Store a behavior socket that contain multiople behaviorSO
    class BotBehaviorHandler : MonoBehaviour
    {
        [SerializeField]
        private BotBehaviorSocketSO _behaviorSocketSOs;

        [SerializeField]
        private GameObject _telegraphyPrefab;

        private VisualEffect _vfxObject = null;

        int _nextBehavior = 0;
        int _currentBehavior = 0;

        public IEnumerator CalculateNextBehavior()
        {
            yield return null;
            _nextBehavior = Random.Range(0, _behaviorSocketSOs.GetBehaviorSize);
            if (_vfxObject == null)
            {
                _telegraphyPrefab.SetActive(true);
                _vfxObject = _telegraphyPrefab.GetComponent<VisualEffect>();
            }
            Debug.Log("100");
            _vfxObject.playRate = 100;
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