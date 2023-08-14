using System;
using TMPro;
using UnityEngine;

namespace Vanaring_DepaDemo
{
    [Serializable]
    public class DamageOutputPopupHandler  
    {
        [SerializeField]
        private DestroyOnTimer _outputDMGTimer;

        private CombatEntity _entity;

        private float _accumulatedDMG = 0;

        public DamageOutputPopupHandler(DamageOutputPopupHandler outputHandler,  CombatEntity _owner)
        {
            this._outputDMGTimer = outputHandler._outputDMGTimer;
            _entity = _owner; 
            _entity.SubOnDamageVisualEvent(OnDMGVisualUpdate);
        }


        ~DamageOutputPopupHandler()
        {
            _entity.UnSubOnDamageVisualEvent(OnDMGVisualUpdate);
        }


        private void OnDMGVisualUpdate(int DONTUSE)
        {
            var v = MonoBehaviour.Instantiate(_outputDMGTimer);
            v.GetComponent<TextMeshPro>().text = _accumulatedDMG.ToString()  ;
            v.transform.position = _entity.CombatEntityAnimationHandler.GetVFXSpawnPos()  ;

            _accumulatedDMG = 0;
        
        }

        public void AccumulateDMG(int dmg)
        {
            _accumulatedDMG += dmg; 
        }
    }
}
