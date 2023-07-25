using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


namespace Vanaring_DepaDemo
{
    [Serializable]
    public class EnergyOverflowHandler : MonoBehaviour
    {
        [SerializeField]
        private StatusEffectApplierFactorySO _statusEffectFactory ;

        private CombatEntity _combatEntity;

        private SpellCasterHandler _spellCasterHandler; 
        
        ~EnergyOverflowHandler() {
            _spellCasterHandler.UnSubOnModifyEnergy(OnModifyEnergy);
        }

        #region Method 
        public void OnModifyEnergy(RuntimeMangicalEnergy.EnergySide side, int amount)
        {

            if ( _spellCasterHandler.IsEnergyOverheat() )  
            {
                IEnumerator coroutine = _statusEffectFactory.Factorize(new List<CombatEntity>(){ _combatEntity}); 
                while (coroutine.MoveNext())
                {
                    if (coroutine.Current != null && coroutine.Current.GetType().IsSubclassOf(typeof(RuntimeEffect)) ) {
                        StartCoroutine((coroutine.Current as RuntimeEffect).ExecuteRuntimeCoroutine(_combatEntity)); 
                    }
                }

                _combatEntity.SpellCaster.ResetEnergy(); 
            } 
        }

        private void Awake()
        {
            this._combatEntity = GetComponent<CombatEntity>() ; 
            this._spellCasterHandler = this._combatEntity.SpellCaster;

            _spellCasterHandler.SubOnModifyEnergy(OnModifyEnergy);
        }

       


        #endregion
    }
}
