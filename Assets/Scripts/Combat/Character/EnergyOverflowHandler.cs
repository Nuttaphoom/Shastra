using CustomYieldInstructions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


namespace Vanaring 
{
    [Serializable]
    public class EnergyOverflowHandler : MonoBehaviour
    {
        [SerializeField]
        private AilmentAppilerRuntimeEffectFactorySO _stunApplier;
 
        private CombatEntity _combatEntity;

        private SpellCasterHandler _spellCasterHandler; 
        
        [SerializeField]
        private GameObject _star_circle_stunVFX;
 
        [SerializeField]
        ActionAnimationInfo _actionAnimationInfo;

        private GameObject _starVFX_Instantied ;

        private bool _isOverflow = false ;

        [SerializeField]
        private ActorActionFactory _zoomToSelfAction ;

        #region Method 
        /// <summary>
        /// Call instantly if modify energy  is apply to them 
        /// Call after action is perform when a character use spell 
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="side"></param>
        /// <param name="amount"></param>
        public IEnumerator OverflowResolve()
        {
            if (_spellCasterHandler.IsEnergyOverflow())
                yield return Overflow( ) ;
        }

        //public IEnumerator PostActionOverflowResolve()
        //{
        //    if (_spellCasterHandler.IsEnergyOverflow())
        //    {
        //        if (!_isOverflow)
        //        {
        //            _isOverflow = true;

        //            ColorfulLogger.LogWithColor(_combatEntity + " Overflow", Color.yellow);

        //            RuntimeEffect effect = _stunApplier.Factorize(new List<CombatEntity>() { _combatEntity });
        //            StartCoroutine(effect.ExecuteRuntimeCoroutine(_combatEntity));

        //            _combatEntity.LogicHurt(null, _combatEntity.StatsAccumulator.GetATKAmount());
        //            _combatEntity.ApplyOverflow();

        //            List<IEnumerator> _iEnumerator = new List<IEnumerator>(); 

        //            if (!_combatEntity.IsDead)
        //            {
        //                _iEnumerator.Add((_combatEntity.CombatEntityAnimationHandler.PlayVFXActionAnimation<string>(_actionAnimationInfo.CasterVfxEntity, VisualStunApplier, "Stunt")));
        //                _starVFX_Instantied = Instantiate(_star_circle_stunVFX, _above_head_transform);
        //                _starVFX_Instantied.transform.position = _above_head_transform.position;
        //            }
        //            else
        //            {
        //                _iEnumerator .Add (_combatEntity.VisualHurt(null, "Die"));
        //            }

        //            _iEnumerator.Add ( (_zoomToSelfAction.FactorizeRuntimeAction(_combatEntity)).PerformAction() ) ;

        //            yield return new WaitAll(this,_iEnumerator.ToArray() );
        //        }
        //    } 
        //}

        
        public IEnumerator Overflow()
        {
            if (!_isOverflow)
            {
                _isOverflow = true; 

                RuntimeEffect effect = _stunApplier.Factorize(new List<CombatEntity>() { _combatEntity });
                StartCoroutine(effect.ExecuteRuntimeCoroutine(_combatEntity));
                
                //_combatEntity.LogicHurt(null, _combatEntity.StatsAccumulator.GetATKAmount());
                _combatEntity.ApplyOverflow();

                if (!_combatEntity.IsDead)
                {
                    //yield return (_combatEntity.CombatEntityAnimationHandler.PlayVFXActionAnimation<string>(_actionAnimationInfo.CasterVfxEntity, VisualStunApplier, "Stunt"));
                    yield return VisualStunApplier("Stunt") ; 

                }
                else
                {
                    //yield return (_combatEntity.VisualHurt(null, "Die"));
                } 
            }

            yield return null; 
        }

        public IEnumerator ResetOverflow()
        {
            Debug.Log("reset overflow"); 
            List<IEnumerator> _iEnumerator = new List<IEnumerator>();

            if (_starVFX_Instantied != null)
            {
                Destroy(_starVFX_Instantied.gameObject);
                _starVFX_Instantied = null;
            }
            _combatEntity.SpellCaster.ResetEnergy();
            _isOverflow = false;

            
            _iEnumerator.Add((_zoomToSelfAction.FactorizeRuntimeAction(_combatEntity)).PerformAction());

            _iEnumerator.Add(_combatEntity.CombatEntityAnimationHandler.PlayTriggerAnimation("StuntRelieve"));

            yield return new WaitAll(_combatEntity, _iEnumerator.ToArray() ); 
        
        
        }

        private IEnumerator VisualStunApplier(string s)
        {
            StartCoroutine(RunnintOverheatVisualEffect()); 
            yield return (_combatEntity.VisualHurt("Stun"));

        }

        private IEnumerator RunnintOverheatVisualEffect()
        {
            //_starVFX_Instantied = Instantiate(_star_circle_stunVFX, _above_head_transform);
            //_starVFX_Instantied.transform.position = _above_head_transform.position;

            yield return new WaitForSecondsRealtime(0.2f); 
            Time.timeScale = 0.25f;

            yield return new WaitForSecondsRealtime(0.5f);

            Time.timeScale = 1.0f; 
        }

        private void Awake()
        {
            this._combatEntity = GetComponent<CombatEntity>() ;

            if (_star_circle_stunVFX == null)
                throw new Exception("Star Circle Stun VFX hasnt been assigned in " + _combatEntity); 
        }

        private void Start()
        {
            this._spellCasterHandler = this._combatEntity.SpellCaster;
        }

        #endregion
    }
}
