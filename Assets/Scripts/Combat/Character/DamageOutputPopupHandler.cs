using System;
using TMPro;
using UnityEngine;

namespace Vanaring 
{
    [Serializable]
    public class DamageOutputPopupHandler  
    {
        [SerializeField]
        private DestroyOnTimer _outputDMGTimerPrefab;
        [SerializeField]
        private DestroyOnTimer _outputHealTimerPrefab ; 

        private CombatEntity _entity;

        private float _accumulatedDMG = 0;
        private float _accumulatedHP = 0; 

        public DamageOutputPopupHandler(DamageOutputPopupHandler outputHandler,  CombatEntity _owner)
        {
            this._outputDMGTimerPrefab = outputHandler._outputDMGTimerPrefab;
            this._outputHealTimerPrefab = outputHandler._outputHealTimerPrefab;
            _entity = _owner; 
            _entity.SubOnDamageVisualEvent(OnDMGVisualUpdate);
        }


        ~DamageOutputPopupHandler()
        {
            _entity.UnSubOnDamageVisualEvent(OnDMGVisualUpdate);
        }


        private void OnDMGVisualUpdate(int DONTUSE)
        {
            if (_accumulatedDMG > 0)
            {
                var v = MonoBehaviour.Instantiate(_outputDMGTimerPrefab);
                v.GetComponent<TextMeshPro>().text = _accumulatedDMG.ToString();
                v.transform.position = _entity.CombatEntityAnimationHandler.GetVFXSpawnPos();

                _accumulatedDMG = 0;
            }

            if (_accumulatedHP > 0)
            {
                var vv = MonoBehaviour.Instantiate(_outputHealTimerPrefab);
                vv.GetComponent<TextMeshPro>().text = _accumulatedHP.ToString();
                vv.transform.position = _entity.CombatEntityAnimationHandler.GetVFXSpawnPos();

                _accumulatedHP = 0;
            }
        }

        public void AccumulateDMG(int dmg)
        {
            _accumulatedDMG += dmg; 
        }

        public void AccumulateHP(int hp)
        {
            _accumulatedHP += hp; 
        }
    }
}
