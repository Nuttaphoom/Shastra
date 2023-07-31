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
        enum EnergyModifyPeak { 
            Min = 0,
            Max = 100, // 0-100
            lowtomid = 33, // 0-33
            midtohigh = 66, // 34-66
        }

        [SerializeField]
        private BotBehaviorSocketSO _behaviorSocketSOs;

        [SerializeField]
        private GameObject _telegraphyPos;

        private GameObject prefab;

        int _nextBehavior = 0;
        int _currentBehavior = 0;

        public IEnumerator CalculateNextBehavior()
        {
            //Debug.Log("CalculateNextBehavior");
            yield return null;
            _nextBehavior = Random.Range(0, _behaviorSocketSOs.GetBehaviorSize);

            //GetBehaviorEffect
            IEnumerator coroutine = _behaviorSocketSOs.GetBehaviorEffect(_nextBehavior);

            while (coroutine.MoveNext())
            {
                if (coroutine.Current != null && coroutine.Current.GetType().IsSubclassOf(typeof(RuntimeEffectFactorySO)))
                {
                    RuntimeEffectFactorySO spell = coroutine.Current as RuntimeEffectFactorySO;
                    EnergyModifierRuntimeEffectFactory EMspell = spell as EnergyModifierRuntimeEffectFactory;
                    if (EMspell != null)
                    {
                        EnergyModifierData modifier = EMspell.ModifierData;
                        int side = (int)modifier.Side; //0 -light, 1 -dark
                        int amount = modifier.Amount;
                        //magic number :D
                        int index = 0;
                        if ((int)EnergyModifyPeak.Min <= amount && (int)EnergyModifyPeak.Max >= amount)
                        {
                            if (amount >= (int)EnergyModifyPeak.lowtomid)
                            {
                                index = 1;
                            }
                            if (amount >= (int)EnergyModifyPeak.midtohigh)
                            {
                                index = 2;
                            }
                        }
                        index += side * 3;
                        if (prefab != null)
                        {
                            Destroy(prefab);
                        }
                        prefab = Instantiate(VfxPrefabHandler.instance.GetVfxTelegraphPrefab(index),
                            _telegraphyPos.transform.position, _telegraphyPos.transform.rotation);
                        if (amount == 0)
                        {
                            prefab.SetActive(false);
                        }
                        else
                        {
                            prefab.SetActive(true);
                        }

                        //Debug.Log("Summoned");
                        //Destroy(prefab);
                        //prefab.SetActive(false);
                    }
                }
            }
            //yield return null;
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