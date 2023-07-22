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
    public class EnergyOverflowHandler : RequireInitializationHandler<CombatEntity, Null,Null> 
    {
        [SerializeField]
        private StatusEffectApplierFactorySO _stuntEffectApplier;

        private CombatEntity _combatEntity;

        private SpellCasterHandler _spellCasterHandler; 
        
        ~EnergyOverflowHandler() {
            _spellCasterHandler.UnSubOnModifyEnergy(OnModifyEnergy);

        }

        #region Method 
        public void OnModifyEnergy(RuntimeMangicalEnergy.EnergySide side, int amount)
        {
            if (! IsInit)
                ThrowInitException() ;

                if ( _spellCasterHandler.IsEnergyOverheat() )  {
                IEnumerator coroutine = _stuntEffectApplier.Factorize(new List<CombatEntity>() { _combatEntity } );
                while (coroutine.MoveNext())
                {
                    if (coroutine.Current != null && coroutine.Current is RuntimeEffect)
                    {
                       IEnumerator ie =  (coroutine.Current as StatusEffectApplierRuntimeEffect).ExecuteRuntimeCoroutine(_combatEntity) ;
                       while (ie.MoveNext())
                       {
                           
                       }
                    }
                }
            }
        }

       

        public override void Initialize(CombatEntity argc, Null argv = null, Null argg = null)
        {
            if (IsInit)
                throw new Exception("Trying to Initialize EnergyOverflowHandler in " + argc.name + "more than once");

            SetInit(true); 

            this._combatEntity = argc;
            this._spellCasterHandler = this._combatEntity.SpellCaster  ;

            _spellCasterHandler.SubOnModifyEnergy(OnModifyEnergy);
        }


        #endregion
    }
}
