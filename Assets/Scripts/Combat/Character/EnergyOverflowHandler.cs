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
        
        [SerializeField]
        private GameObject _star_circle_stunVFX;
 

        [SerializeField]
        ActionAnimationInfo _actionAnimationInfo;

        [SerializeField]
        private Transform _above_head_transform;

        private GameObject _starVFX_Instantied ; 
 
        ~EnergyOverflowHandler() {
            _spellCasterHandler.UnSubOnModifyEnergy(OnModifyEnergy);
        }



        #region Method 
        public void OnModifyEnergy(CombatEntity caster, RuntimeMangicalEnergy.EnergySide side, int amount)
        {
            if (_starVFX_Instantied != null)
                return;


            if (_spellCasterHandler.IsEnergyOverheat())
            {
                Overheat(caster);
            }
        }

        public void Overheat(CombatEntity caster)
        {
            IEnumerator coroutine = _statusEffectFactory.Factorize(new List<CombatEntity>() { _combatEntity });
            while (coroutine.MoveNext())
            {
                if (coroutine.Current != null && coroutine.Current.GetType().IsSubclassOf(typeof(RuntimeEffect)))
                {
                    StartCoroutine((coroutine.Current as RuntimeEffect).ExecuteRuntimeCoroutine(_combatEntity));
                }
            }

            _combatEntity.LogicHurt(null, caster.StatsAccumulator.GetATKAmount());
            if (!_combatEntity.IsDead)
            {
                //We stunt this turn and the next turn 
                StartCoroutine(_combatEntity.CombatEntityAnimationHandler.PlayVFXActionAnimation<string>(_actionAnimationInfo.CasterVfxEntity, VisualStunApplier, "Stunt"));
                _starVFX_Instantied = Instantiate(_star_circle_stunVFX, _above_head_transform)  ;  
                _starVFX_Instantied.transform.position = _above_head_transform.position;
            }
            else
            {
                StartCoroutine(_combatEntity.VisualHurt(null, "Die"));
            }
        }

        public void ResetOverflow()
        {
            if (_starVFX_Instantied != null)
            {
                Destroy(_starVFX_Instantied.gameObject);
                _starVFX_Instantied = null;
            }
            _combatEntity.SpellCaster.ResetEnergy();
        }

        private IEnumerator VisualStunApplier(string s)
        {

            StartCoroutine(RunnintOverheatVisualEffect()); 
            _combatEntity.StatsAccumulator.ApplyStunt() ; 
            yield return (_combatEntity.VisualHurt(null, "Stunt"));


        }

        private IEnumerator RunnintOverheatVisualEffect()
        {
            yield return new WaitForSecondsRealtime(0.2f); 
            Time.timeScale = 0.25f;

            yield return new WaitForSecondsRealtime(0.5f);

            Time.timeScale = 1.0f; 
        }

        private void Awake()
        {
            this._combatEntity = GetComponent<CombatEntity>() ; 
        }

        private void Start()
        {
            this._spellCasterHandler = this._combatEntity.SpellCaster;
            _spellCasterHandler.SubOnModifyEnergy(OnModifyEnergy);
        }

        #endregion
    }
}
