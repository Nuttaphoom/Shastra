﻿using JetBrains.Annotations;
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

        [SerializeField]
        private StatGUIScaler _statGUIScaler;

        private GameObject prefab;

        int _nextBehavior = 0;
        int _currentBehavior = 0;

        public IEnumerator CalculateNextBehavior()
        {
            _nextBehavior = Random.Range(0, _behaviorSocketSOs.GetBehaviorSize);
            StartTelegraphy();
            yield return null;
        }

        public BotBehaviorSO.ActionData GetBehaviorEffect()
        {
            _currentBehavior = _nextBehavior;
            List<BotBehaviorSO.ActionData> ret = new List<BotBehaviorSO.ActionData>();
            IEnumerator coroutine = _behaviorSocketSOs.GetBehaviorEffect(_currentBehavior);

            while (coroutine.MoveNext())
            {
                Debug.Log("move next"); 
                Debug.Log(coroutine.GetType() ) ;
                if (coroutine.Current != null && coroutine.Current is BotBehaviorSO.ActionData )
                {
                    return ((BotBehaviorSO.ActionData)coroutine.Current);
                }

            }
            Debug.Log("done move next"); 
            throw new System.Exception("GetBehaviorEffect don't return BotBehaviorSO.ActionData");


        }
        
        public void StartTelegraphy ()
        {
            //GetBehaviorEffect
            //IEnumerator coroutine = _behaviorSocketSOs.GetBehaviorEffect(_nextBehavior);

            //while (coroutine.MoveNext())
            //{
            //    if (coroutine.Current != null && coroutine.Current.GetType().IsSubclassOf(typeof(RuntimeEffectFactorySO)))
            //    {
            //        RuntimeEffectFactorySO spell = coroutine.Current as RuntimeEffectFactorySO;
            //        CreatingTelegraphyInstance(spell);
            //    }
            //}

            CreatingTelegraphyInstance(_behaviorSocketSOs); 
        }

        private void CreatingTelegraphyInstance(BotBehaviorSocketSO spell)
        {
            EnergyModifierData modifier =  spell.GetEnergyCost(_nextBehavior) ;
            if (modifier.Amount > 0)
            {
                //Debug.Log("Create Telegraphy");
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

                DestroyTelegraphyVFX();

                if (index >= 0 && index <= 2)
                {
                    _statGUIScaler.SetEnergyModified(StatGUIScaler.ModifiedEnergy.LIGHT);
                }
                else
                {
                    _statGUIScaler.SetEnergyModified(StatGUIScaler.ModifiedEnergy.DARK);
                }
                prefab = Instantiate(VfxTelegraphySingletonHandler.instance.GetVfxTelegraphPrefab(index),
                _telegraphyPos.transform);

                prefab.transform.position = _telegraphyPos.transform.position;
                if (amount == 0)
                {
                    _statGUIScaler.SetEnergyModified(StatGUIScaler.ModifiedEnergy.NONE);
                    prefab.SetActive(false);
                }
                else
                {
                    prefab.SetActive(true);
                }
            }
            else
            {
                if (prefab != null)
                {
                    Destroy(prefab); 
                }
            }
        }

        public void DestroyTelegraphyVFX()
        {
            if (prefab != null)
            {
                Destroy(prefab);
            }
        }
    }
}